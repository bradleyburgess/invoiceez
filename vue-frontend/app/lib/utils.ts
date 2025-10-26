import type { ClassValue } from "clsx";
import { clsx } from "clsx";
import { twMerge } from "tailwind-merge";
import type { UserDto } from "~/api-client";

import type { Updater } from "@tanstack/vue-table";

import type { Ref } from "vue";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const makePageTitle = (title?: string) =>
  title ? `${title} | Invoiceez` : "Invoiceez";

export function getUserDisplayName(user: UserDto): string {
  if (user.firstName && user.lastName)
    return `${user.firstName} ${user.lastName}`;
  return user.email ?? "";
}

export function getUserInitials(user: UserDto): string {
  const displayName = getUserDisplayName(user);
  return displayName
    .split(" ")
    .map((n) => n[0])
    .join("")
    .toUpperCase();
}

export function valueUpdater<T extends Updater<unknown>>(
  updaterOrValue: T,
  ref: Ref
) {
  ref.value =
    typeof updaterOrValue === "function"
      ? updaterOrValue(ref.value)
      : updaterOrValue;
}
