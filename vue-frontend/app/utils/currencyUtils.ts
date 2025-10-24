import { CurrencyCode } from "~/api-client";

export const Currencies: CurrencyDetail[] = [
  { code: CurrencyCode.Usd, display: "United States Dollar ($)" },
  { code: CurrencyCode.Eur, display: "Euro (€)" },
  { code: CurrencyCode.Zar, display: "South African Rand (R)" },
];

export type CurrencyDetail = {
  code: string;
  display: string;
};

export type AcceptedCurrency = CurrencyCode;
