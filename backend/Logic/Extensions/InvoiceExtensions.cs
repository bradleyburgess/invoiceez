using Logic.Models;

namespace Logic.Extensions;

public static class InvoiceExtensions
{
    public static decimal CalculateTotalAmount(this Invoice invoice)
    {
        var itemsTotal = invoice.Items.Sum(item => item.Quantity * item.Rate);

        decimal discountsTotal = 0;
        foreach (var discount in invoice.Discounts)
        {
            if (discount.Type == InvoiceDiscountType.Fixed)
            {
                discountsTotal += discount.Amount;
            }
            else if (discount.Type == InvoiceDiscountType.Percentage)
            {
                discountsTotal += itemsTotal * discount.Amount / 100;
            }
        }

        return itemsTotal - discountsTotal;
    }
}
