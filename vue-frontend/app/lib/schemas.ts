import * as z from "zod";

export const emailSchema = z
  .string()
  .min(1, "Required")
  .max(128)
  .email({ message: "Please enter a valid email" });

export const passwordSchema = z.string().min(1, "Required").max(128);

export const strongPasswordSchema = z
  .string()
  .min(12, "Password must be at least 12 characters")
  .max(128)
  .regex(/[A-Z]/, "Password must contain at least one uppercase letter.")
  .regex(/[a-z]/, "Password must contain at least one lowercase letter.")
  .regex(/[0-9]/, "Password must contain at least one number.")
  .regex(
    /[^A-Za-z0-9]/,
    "Password must contain at least one special character."
  );

export const nameSchema = z.string().min(1, "Required").max(50);

export const currencySchema = z
  .number()
  .positive()
  .refine(
    (value) => {
      const multiplier = 100;
      const fractionalPart =
        value * multiplier - Math.trunc(value * multiplier);
      return Math.abs(fractionalPart) < Number.EPSILON;
    },
    {
      message: "Number must have up to two decimal places",
    }
  );

export const requiredStringSchema = z.string().min(1, "Required");
