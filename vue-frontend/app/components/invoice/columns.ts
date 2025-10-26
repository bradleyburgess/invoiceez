import type { ColumnDef } from "@tanstack/vue-table";
import { InvoicePaymentStatus, type InvoiceSummaryDto } from "~/api-client";
import { InvoiceTableCell, InvoiceTableColumnHeader } from "./table";
import { Button } from "../ui/button";
import { ArrowUpDown } from "lucide-vue-next";
import PaidBadge from "../PaidBadge.vue";
import UnpaidBadge from "../UnpaidBadge.vue";

export const invoiceTableColumns: ColumnDef<InvoiceSummaryDto>[] = [
  {
    accessorKey: "invoiceNumber",
    header: () => InvoiceTableColumnHeader("Invoice #"),
    cell: ({ row }) => InvoiceTableCell(row.getValue("invoiceNumber")),
  },
  {
    accessorKey: "invoiceDate",
    header: ({ column }) => {
      return h(
        Button,
        {
          variant: "ghost",
          onClick: () => column.toggleSorting(column.getIsSorted() === "asc"),
        },
        () => ["Date", h(ArrowUpDown, { class: "ml-2 h-4 w-4" })]
      );
    },
    cell: ({ row }) =>
      h("div", { class: "px-4" }, displayDate(row.getValue("invoiceDate"))),
  },
  {
    accessorKey: "totalAmount",
    header: () => InvoiceTableColumnHeader("Amount"),
    cell: ({ row }) => {
      const c = useCurrency();
      const currency: string = row.original.currency ?? "";
      const amount = c.format(row.getValue("totalAmount"), currency);
      return InvoiceTableCell(amount);
    },
  },
  {
    accessorKey: "customerName",
    header: () => InvoiceTableColumnHeader("Customer"),
    cell: ({ row }) => InvoiceTableCell(row.getValue("customerName")),
  },
  {
    accessorKey: "paymentStatus",
    header: () => InvoiceTableColumnHeader("Status"),
    cell: ({ row }) =>
      row.getValue("paymentStatus") == InvoicePaymentStatus.Paid
        ? h(PaidBadge)
        : h(UnpaidBadge),
  },
  {
    accessorKey: "businessName",
    header: () => InvoiceTableColumnHeader("Business"),
    cell: ({ row }) => InvoiceTableCell(row.getValue("businessName")),
  },
];
