using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class CurrencyConverterRepository : ICurrencyConverterRepository
    {
        private readonly CurrencyConverterDbContext _context;

        public CurrencyConverterRepository(CurrencyConverterDbContext context)
        {
            _context = context;
        }

        public async Task<CurrencyRate> GetCurrencyRateByIdAsync(int id)
        {
            return await _context.CurrencyRates.FindAsync(id);
        }

        public async Task<IReadOnlyList<CurrencyRate>> GetCurrencyRates()
        {
            return await _context.CurrencyRates.ToListAsync();
        }
    }
}
