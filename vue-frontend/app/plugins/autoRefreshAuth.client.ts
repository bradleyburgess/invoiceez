export default defineNuxtPlugin(async (_) => {
  const auth = useAuth();

  try {
    await auth.refreshToken();
  } catch {
    navigateTo("/auth/login");
  }
});
