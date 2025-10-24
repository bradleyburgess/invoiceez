import { CurrencyCode } from "~/api-client";

function getFormatter(currencyCode: string | null | undefined) {
  switch (currencyCode) {
    case CurrencyCode.Usd:
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      });
    case CurrencyCode.Eur:
      return new Intl.NumberFormat("en-GB", {
        style: "currency",
        currency: "EUR",
      });
    case CurrencyCode.Zar:
      return new Intl.NumberFormat("af-ZA", {
        style: "currency",
        currency: "ZAR",
      });
    case null:
    case undefined:
      return new Intl.NumberFormat("en-US", {
        style: "decimal",
        maximumFractionDigits: 2,
      });

    default:
      throw new Error(`"${currencyCode}" is not an accepted currency`);
  }
}

export function useCurrency() {
  function format(
    amount: number,
    currencyCode: string | null | undefined
  ): string {
    const formatter = getFormatter(currencyCode);
    return formatter.format(amount);
  }

  return {
    format,
  };
}
