const publicPages = ["/auth/login", "/auth/register", "/auth/logout"];

export default defineNuxtRouteMiddleware((to, _) => {
  const auth = useAuth();
  if (!auth.isLoggedIn() && !publicPages.includes(to.path)) {
    return navigateTo("/auth/login");
  }
});
