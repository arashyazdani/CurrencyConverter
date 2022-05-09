using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class CurrencyRateToReturnDto
    {
        public CurrencyRateToReturnDto(int id, string fromCurrency, string toCurrency, decimal rate)
        {
            Id = id;
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Rate = rate;
        }

        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
