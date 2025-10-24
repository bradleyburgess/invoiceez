export default defineNuxtRouteMiddleware((to) => {
  if (to.path === "/auth/register") {
    const allowed = useRegistrationAllowed();
    if (allowed.value === false) {
      return navigateTo("/auth/login");
    }
  }
});
