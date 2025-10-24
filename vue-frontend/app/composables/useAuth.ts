import axios, { AxiosError } from "axios";
import {
  ApiResponseCode,
  AuthApi,
  Configuration,
  type AuthLoginRequestDto,
  type AuthRegisterRequestDto,
  type AuthResponseDtoApiResponse,
  type TokenResponseDto,
  type UserDto,
} from "~/api-client";
import { NetworkError } from "~/lib/errors";

export default function useAuth() {
  const axiosClient = axios.create({
    baseURL: useRuntimeConfig().public.apiBaseUrl,
    withCredentials: true,
    validateStatus: (status) => status >= 200 && status < 500,
  });

  const client = new AuthApi(
    new Configuration({
      basePath: useRuntimeConfig().public.apiBaseUrl,
      baseOptions: {
        withCredentials: true,
      },
    }),
    undefined,
    axiosClient
  );

  const _isAuthenticated = useState<boolean>("isAuthenticated", () => false);
  const _currentUser = useState<UserDto | null>("currentUser", () => null);
  const _accessToken = useState<string | null>("accessToken", () => null);
  const _tokenExpiresAt = useState<number | null>(() => null);

  function isLoggedIn(): boolean {
    return _isAuthenticated.value && !!_currentUser.value;
  }

  function getCurrentUser(): UserDto | null {
    return _currentUser.value;
  }

  function setUser(user: UserDto | null): void {
    _currentUser.value = user;
    _isAuthenticated.value = !!user;
  }

  function getAccessToken(): string | null {
    return _accessToken.value;
  }

  function setAccessToken(dto: TokenResponseDto | null): void {
    _accessToken.value = dto?.accessToken ?? null;
    _tokenExpiresAt.value = dto?.expiresAtUtc
      ? Date.parse(dto.expiresAtUtc)
      : null;
  }

  async function login(
    dto: AuthLoginRequestDto
  ): Promise<AuthResponseDtoApiResponse> {
    try {
      const res = await client.login(dto);
      const { data, code } = res.data;
      if (code === ApiResponseCode.Ok && data?.tokens) {
        _isAuthenticated.value = true;
        _currentUser.value = data.user || null;
        _accessToken.value = data.tokens.accessToken || null;
      }
      return res.data;
    } catch (e) {
      handleError(e);
    }
  }

  function handleError(e: unknown): never {
    if (e instanceof AxiosError) {
      throw new NetworkError();
    }
    throw e;
  }

  async function logout(): Promise<void> {
    await client.logout();
    _isAuthenticated.value = false;
    _currentUser.value = null;
    _accessToken.value = null;
    navigateTo("/auth/login");
  }

  async function register(
    dto: AuthRegisterRequestDto
  ): Promise<AuthResponseDtoApiResponse | null> {
    const res = await client.register(dto);
    const { data, code } = res.data;
    if (code === ApiResponseCode.Ok && data) {
      setUser(data.user);
      setAccessToken(data.tokens || null);
      navigateTo("/");
      return null;
    }
    return res.data;
  }

  async function refreshToken(): Promise<void> {
    const res = await client.refreshToken({});
    const { data, code } = res.data;
    if (code === ApiResponseCode.Ok && data) {
      setAccessToken(data.tokens || null);
      setUser(data.user || null);
    } else {
      await logout();
    }
  }

  function isTokenExpired(): boolean {
    if (!_accessToken.value || !_tokenExpiresAt.value) return true;
    return Date.now() >= _tokenExpiresAt.value;
  }

  async function checkRegistrationAccepted(): Promise<boolean> {
    const res = await client.checkRegistrationAccepted();
    return res.data;
  }

  return {
    checkRegistrationAccepted,
    getAccessToken,
    getCurrentUser,
    isLoggedIn,
    isTokenExpired,
    login,
    logout,
    refreshToken,
    register,
  };
}
