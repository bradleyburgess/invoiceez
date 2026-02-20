using Logic.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Logic.Services;

public class InvoiceGenerationService : IInvoiceGenerationService
{
    public byte[] Generate(Invoice invoice)
    {
        var document = new InvoiceDocument(invoice);
        var bytes = document.GeneratePdf();
        return bytes;
    }


}

public interface IInvoiceGenerationService
{
    byte[] Generate(Invoice Invoice);
}

public class InvoiceDocument(Invoice invoice) : IDocument
{
    private readonly Invoice Invoice = invoice;

    private DocumentMetadata GetMetadata() => new DocumentMetadata()
    {
        Title = Invoice.InvoiceNumber,
        Author = Invoice.Business?.Name,
        Subject = $"Invoice for {Invoice.CustomerName}",
        Creator = "Invoiceez",
        CreationDate = DateTimeOffset.Now,
        ModifiedDate = DateTimeOffset.Now,
    };

    private static DocumentSettings GetDocumentSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(d => d.FontSize(11));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().PaddingBottom(2).Text(Invoice.BusinessName)
                    .FontSize(18).SemiBold();

                if (!string.IsNullOrWhiteSpace(Invoice.BusinessTagline))
                    col.Item().PaddingBottom(8).Text(Invoice.BusinessTagline)
                        .FontSize(12).SemiBold();

                if (!string.IsNullOrWhiteSpace(Invoice.BusinessAddress))
                    ComposeBusinessInfoRow(col, pinSvg, Invoice.BusinessAddress, 3);

                if (!string.IsNullOrWhiteSpace(Invoice.BusinessEmail))
                    ComposeBusinessInfoRow(col, mailSvg, Invoice.BusinessEmail);

                if (!string.IsNullOrWhiteSpace(Invoice.BusinessWebsite))
                    ComposeBusinessInfoRow(col, globeSvg, Invoice.BusinessWebsite);

                if (!string.IsNullOrWhiteSpace(Invoice.BusinessPhone))
                    ComposeBusinessInfoRow(col, phoneSvg, Invoice.BusinessPhone);
            });

            row.ConstantItem(200).Column(col =>
            {
                col.Item().PaddingBottom(8).AlignRight().Text("INVOICE")
                    .FontSize(20).SemiBold();

                col.Item().AlignRight().Text(text =>
                {
                    text.Span("Invoice #: ").SemiBold();
                    text.Span(Invoice.InvoiceNumber);
                });

                col.Item().AlignRight().Text(text =>
                {
                    text.Span("Date: ").SemiBold();
                    text.Span(FormatDate(Invoice.InvoiceDate));
                });
            });
        });
    }

    private void ComposeBusinessInfoRow(ColumnDescriptor col, string svg, string text, int paddingTop = 3) =>
            col.Item().PaddingBottom(1).Row(row =>
            {
                row.ConstantItem(12).PaddingRight(4).PaddingTop(paddingTop).Svg(svg);
                row.AutoItem().Text(text).FontSize(10);
            });


    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(20).Column(column =>
        {
            column.Item().PaddingVertical(20).Element(ComposeCustomerDetails);

            // Items Table
            column.Item().Element(ComposeItemsTable);

            // Totals
            var subtotal = Invoice.Items.Sum(i => i.Rate * i.Quantity);
            var discountTotal = Invoice.Discounts.Sum(d =>
                d.Type == InvoiceDiscountType.Fixed ? d.Amount
                : subtotal * (d.Amount / 100));

            var grandTotal = subtotal - discountTotal;

            column.Item().PaddingTop(30).Row(row =>
            {
                row.RelativeItem();
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Subtotal:").AlignRight().SemiBold();
                        r.ConstantItem(100).AlignRight().Text(FormatCurrency(subtotal));
                    });

                    foreach (var discount in Invoice.Discounts)
                    {
                        var discountValue = discount.Type == InvoiceDiscountType.Fixed
                            ? discount.Amount
                            : subtotal * (discount.Amount / 100);

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text($"Discount ({discount.Description}):").AlignRight();
                            r.ConstantItem(100).AlignRight().Text($"- {FormatCurrency(discountValue)}");
                        });
                    }

                    col.Item().PaddingTop(5).BorderTop(1).Row(r =>
                    {
                        r.RelativeItem().Text("Grand Total:").AlignRight().SemiBold();
                        r.ConstantItem(100).AlignRight().Text(FormatCurrency(grandTotal)).SemiBold();
                    });
                });
            });

            column.Item().PaddingVertical(50);
            column.Item().ShowOnce().Element(ComposePaymentInformation);
        });
    }

    private void ComposeCustomerDetails(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().PaddingTop(1).Text("BILLED TO:").Bold();
            column.Item().PaddingTop(1).Text(Invoice.CustomerName);
            column.Item().PaddingTop(1).Text(Invoice.CustomerAddress);
            column.Item().PaddingTop(1).Text(Invoice.CustomerEmail);
            column.Item().PaddingTop(1).Text(Invoice.CustomerPhone);
        });
    }

    private void ComposeItemsTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);   // #
                columns.RelativeColumn(3);    // Description
                columns.ConstantColumn(80);   // Quantity
                columns.ConstantColumn(80);   // Rate
                columns.ConstantColumn(100);  // Total
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#").SemiBold();
                header.Cell().Element(CellStyle).Text("Description").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Qty").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Rate").SemiBold();
                header.Cell().Element(CellStyle).AlignRight().Text("Total").SemiBold();
            });

            var lineNumber = 1;
            foreach (var item in Invoice.Items.OrderBy(i => i.Order))
            {
                table.Cell().Element(CellStyle).Text(lineNumber.ToString());
                table.Cell().Element(CellStyle).Text(item.Description);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text(FormatCurrency(item.Rate));
                table.Cell().Element(CellStyle).AlignRight().Text(
                    FormatCurrency(item.Quantity * item.Rate)
                );
                lineNumber++;
            }
        });
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(5)
            .PaddingHorizontal(2);
    }

    private CultureInfo GetCultureForCurrency()
    {
        return Invoice.Currency switch
        {
            CurrencyCode.USD => new CultureInfo("en-US"),
            CurrencyCode.EUR => new CultureInfo("de-DE"),
            CurrencyCode.ZAR => new CultureInfo("en-ZA"),
            _ => CultureInfo.InvariantCulture
        };
    }

    private string FormatCurrency(decimal amount)
    {
        var culture = GetCultureForCurrency();
        return string.Format(culture, "{0:C}", amount);
    }

    private string FormatDate(DateTime date)
    {
        var culture = GetCultureForCurrency();

        // Locale-specific "long" date without weekday
        string format = culture.Name switch
        {
            "en-US" => "MMMM d, yyyy",    // October 15, 2025
            "en-ZA" => "d MMMM yyyy",     // 15 October 2025
            "de-DE" => "d. MMMM yyyy",    // 15. Oktober 2025
            _ => "d MMMM yyyy"             // fallback
        };

        return date.ToString(format, culture);
    }

    private void ComposePaymentInformation(IContainer container)
    {
        container.PreventPageBreak().PaddingTop(30).Column(col =>
        {
            col.Item().Text("PAYMENT INFORMATION").Bold();
            col.Item().Text(Invoice.PaymentInstructions);
        });
    }

    private static readonly string pinSvg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-geo-alt-fill\" viewBox=\"0 0 16 16\"><path d=\"M8 16s6-5.686 6-10A6 6 0 0 0 2 6c0 4.314 6 10 6 10m0-7a3 3 0 1 1 0-6 3 3 0 0 1 0 6\"/></svg>";

    private static readonly string mailSvg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-envelope\" viewBox=\"0 0 16 16\"><path d=\"M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1zm13 2.383-4.708 2.825L15 11.105zm-.034 6.876-5.64-3.471L8 9.583l-1.326-.795-5.64 3.47A1 1 0 0 0 2 13h12a1 1 0 0 0 .966-.741M1 11.105l4.708-2.897L1 5.383z\" /></svg>";

    private static readonly string globeSvg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-globe-americas\" viewBox=\"0 0 16 16\"><path d=\"M8 0a8 8 0 1 0 0 16A8 8 0 0 0 8 0M2.04 4.326c.325 1.329 2.532 2.54 3.717 3.19.48.263.793.434.743.484q-.121.12-.242.234c-.416.396-.787.749-.758 1.266.035.634.618.824 1.214 1.017.577.188 1.168.38 1.286.983.082.417-.075.988-.22 1.52-.215.782-.406 1.48.22 1.48 1.5-.5 3.798-3.186 4-5 .138-1.243-2-2-3.5-2.5-.478-.16-.755.081-.99.284-.172.15-.322.279-.51.216-.445-.148-2.5-2-1.5-2.5.78-.39.952-.171 1.227.182.078.099.163.208.273.318.609.304.662-.132.723-.633.039-.322.081-.671.277-.867.434-.434 1.265-.791 2.028-1.12.712-.306 1.365-.587 1.579-.88A7 7 0 1 1 2.04 4.327Z\"/></svg>";

    private static readonly string phoneSvg = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-telephone\" viewBox=\"0 0 16 16\"><path d=\"M3.654 1.328a.678.678 0 0 0-1.015-.063L1.605 2.3c-.483.484-.661 1.169-.45 1.77a17.6 17.6 0 0 0 4.168 6.608 17.6 17.6 0 0 0 6.608 4.168c.601.211 1.286.033 1.77-.45l1.034-1.034a.678.678 0 0 0-.063-1.015l-2.307-1.794a.68.68 0 0 0-.58-.122l-2.19.547a1.75 1.75 0 0 1-1.657-.459L5.482 8.062a1.75 1.75 0 0 1-.46-1.657l.548-2.19a.68.68 0 0 0-.122-.58zM1.884.511a1.745 1.745 0 0 1 2.612.163L6.29 2.98c.329.423.445.974.315 1.494l-.547 2.19a.68.68 0 0 0 .178.643l2.457 2.457a.68.68 0 0 0 .644.178l2.189-.547a1.75 1.75 0 0 1 1.494.315l2.306 1.794c.829.645.905 1.87.163 2.611l-1.034 1.034c-.74.74-1.846 1.065-2.877.702a18.6 18.6 0 0 1-7.01-4.42 18.6 18.6 0 0 1-4.42-7.009c-.362-1.03-.037-2.137.703-2.877z\"/></svg>";
}