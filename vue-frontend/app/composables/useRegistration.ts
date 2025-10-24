export const useRegistrationAllowed = () =>
  useState<boolean | null>("registration-allowed", () => null);
