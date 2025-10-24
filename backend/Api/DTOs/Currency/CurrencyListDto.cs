using Logic.Models;

namespace Api.DTOs.Currency;

public class CurrencyListDto
{
    public IEnumerable<CurrencyCode> Currencies { get; set; } = [];
}
