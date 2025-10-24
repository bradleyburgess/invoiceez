export default defineNuxtPlugin(async () => {
  const allowed = useRegistrationAllowed();
  const auth = useAuth();

  if (allowed.value === null) {
    try {
      const res = await auth.checkRegistrationAccepted();
      allowed.value = res;
    } catch {
      allowed.value = false;
    }
  }
});
