import {
  InvoiceDiscountType,
  type InvoiceDiscountEditDto,
  type InvoiceItemEditDto,
  type InvoiceSummaryDto,
} from "~/api-client";

export function calculateInvoiceTotal(
  items: InvoiceItemEditDto[],
  discounts: InvoiceDiscountEditDto[]
) {
  let total = 0;
  items?.forEach((item) => (total += (item.quantity ?? 0) * (item.rate ?? 0)));
  let amountToDiscount = 0;
  discounts?.forEach((discount) => {
    if (discount.type == InvoiceDiscountType.Fixed) {
      amountToDiscount += discount.amount ?? 0;
    }
    if (discount.type == InvoiceDiscountType.Percentage) {
      amountToDiscount += (total * 100) / (discount.amount ?? 1);
    }
  });
  return total - amountToDiscount;
}

export async function generateInvoiceNumber(date: string): Promise<string> {
  const api = useApi();
  const response = await api.invoices.generateInvoiceNumber(
    new Date(date).toISOString()
  );
  return response.data.data ?? "";
}

export async function validateInvoiceNumber(
  invoiceNumber: string,
  invoiceId?: string | undefined | null
): Promise<[boolean, string | null]> {
  if (!invoiceNumber) return [false, ""];
  const api = useApi();
  const { data: result } = await api.invoices.validateInvoiceNumber(
    invoiceNumber,
    invoiceId ?? undefined
  );
  if (result.data) return [true, null];
  return [false, result.message ?? "Invalid invoice number"];
}

export const sortInvoiceSummariesByModified = (
  a: InvoiceSummaryDto,
  b: InvoiceSummaryDto
) =>
  new Date(b.modifiedAtUtc!).getTime() - new Date(a.modifiedAtUtc!).getTime();

export const getDestructiveWord = (
  newEntity: boolean,
  capitalize: boolean = false
): string =>
  newEntity
    ? capitalize
      ? "Discard"
      : "discard"
    : capitalize
      ? "Delete"
      : "delete";
