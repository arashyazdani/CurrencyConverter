using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICurrencyConverterRepository
    {
        Task<CurrencyRate> GetCurrencyRateByIdAsync(int id);
        Task<IReadOnlyList<CurrencyRate>> GetCurrencyRates();

    }
}
