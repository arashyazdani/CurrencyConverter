using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class CurrencyConverterDbContextSeed
    {
        public static async Task SeedAsync(CurrencyConverterDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!context.CurrencyRates.Any())
                {
                    var currencyRatesData = File.ReadAllText(path + @"/Data/SeedData/CurrencyRate.json");
                    var currencyRates = JsonSerializer.Deserialize<List<CurrencyRate>>(currencyRatesData);

                    foreach (var item in currencyRates)
                    {
                        await context.CurrencyRates.AddAsync(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<CurrencyConverterDbContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
