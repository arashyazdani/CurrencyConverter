using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface ICurrencyService
    {
        Task<CurrencyRate> IsCurrencyCheckAsync(string fromCurrency, string toCurrency);

        Task<IList<CurrencyRateSpec>> UpdateCurrencyRatesAsync(IList<CurrencyRateSpec> currencyRates);

        Task<bool> DeleteAllCurrencyRatesAsync();
    }
}
