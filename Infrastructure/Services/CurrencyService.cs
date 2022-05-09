using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CurrencyRate> IsCurrencyCheckAsync(string fromCurrency, string toCurrency)
        {
            var allCurrencies = await _unitOfWork.Repository<CurrencyRate>().ListAllAsync();
            
            if(!allCurrencies.Any()) return null;

            var currencyReturn = allCurrencies.FirstOrDefault(x => x.FromCurrency.ToUpper() == fromCurrency.ToUpper() && x.ToCurrency.ToUpper() == toCurrency.ToUpper());
            
            if (currencyReturn!=null) return currencyReturn;

            currencyReturn = allCurrencies.FirstOrDefault(x => x.ToCurrency.ToUpper() == fromCurrency.ToUpper() && x.FromCurrency.ToUpper() == toCurrency.ToUpper());

            if (currencyReturn != null)
            {
                var newCurrencyRate = new CurrencyRate
                {
                    FromCurrency = fromCurrency.ToUpper(),
                    ToCurrency = toCurrency.ToUpper(),
                    Rate = (1/currencyReturn.Rate)
                };

                _unitOfWork.Repository<CurrencyRate>().Add(newCurrencyRate);

                var registerdCurrency = await _unitOfWork.Complete();

                if(registerdCurrency<=0) return null;

                return newCurrencyRate;
            }

            var fromCurrenciesGroup = allCurrencies.Where(x=>x.ToCurrency.ToUpper()==toCurrency.ToUpper() || x.ToCurrency.ToUpper()==fromCurrency.ToUpper()).GroupBy(item => item.FromCurrency.ToUpper())
                .Select(group => new { ToCurrency = group.Key, Items = group.ToList() })
                .ToList();

            if (fromCurrenciesGroup.Count()>1)
            {
                var fromCurrencyFound = fromCurrenciesGroup[0].Items.FirstOrDefault(z => z.ToCurrency == fromCurrency.ToUpper());
                var toCurrencyFound = fromCurrenciesGroup[0].Items.FirstOrDefault(z => z.ToCurrency == toCurrency.ToUpper());
                if (fromCurrencyFound != null && toCurrencyFound != null)
                {
                    var newCurrencyRate = new CurrencyRate
                    {
                        FromCurrency = fromCurrency.ToUpper(),
                        ToCurrency = toCurrency.ToUpper(),
                        Rate = ((1 / fromCurrencyFound.Rate) / (1/ toCurrencyFound.Rate))
                    };

                    _unitOfWork.Repository<CurrencyRate>().Add(newCurrencyRate);

                    var registerdCurrency = await _unitOfWork.Complete();

                    if (registerdCurrency <= 0) return null;

                    return newCurrencyRate;
                }
            }

            var toCurrenciesGroup = allCurrencies.Where(x => x.FromCurrency.ToUpper() == toCurrency.ToUpper() || x.FromCurrency.ToUpper() == fromCurrency.ToUpper()).GroupBy(item => item.ToCurrency.ToUpper())
                .Select(group => new { ToCurrency = group.Key, Items = group.ToList() })
                .ToList();

            if (toCurrenciesGroup.Count() > 1)
            {
                var fromCurrencyFound = toCurrenciesGroup[0].Items.FirstOrDefault(z => z.FromCurrency == fromCurrency.ToUpper());
                var toCurrencyFound = toCurrenciesGroup[0].Items.FirstOrDefault(z => z.FromCurrency == toCurrency.ToUpper());
                if (fromCurrencyFound != null && toCurrencyFound != null)
                {
                    var newCurrencyRate = new CurrencyRate
                    {
                        FromCurrency = fromCurrency.ToUpper(),
                        ToCurrency = toCurrency.ToUpper(),
                        Rate = ((1 / fromCurrencyFound.Rate) / (1 / toCurrencyFound.Rate))
                    };

                    _unitOfWork.Repository<CurrencyRate>().Add(newCurrencyRate);

                    var registerdCurrency = await _unitOfWork.Complete();

                    if (registerdCurrency <= 0) return null;

                    return newCurrencyRate;
                }
            }

            return null;
        }

        public Task<IReadOnlyList<CurrencyRate>> UpdateCurrencyRates(IReadOnlyList<CurrencyRate> currencyRates)
        {
            throw new NotImplementedException();
        }
    }
}
