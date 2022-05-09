using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyRate> IsCurrencyCheckAsync(string fromCurrency, string toCurrency);

        Task<IReadOnlyList<CurrencyRate>> UpdateCurrencyRates(IReadOnlyList<CurrencyRate> currencyRates);
    }
}
