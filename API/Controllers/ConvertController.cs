using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ConvertController : BaseApiController
    {
        private readonly IGenericRepository<CurrencyRate> _repo;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public ConvertController(IGenericRepository<CurrencyRate> repo,ICurrencyService currencyService, IMapper mapper)
        {
            _repo = repo;
            _currencyService = currencyService;
            _mapper = mapper;
        }

        [HttpGet("convert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Convert(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                var convertRate = _currencyService.IsCurrencyCheckAsync(fromCurrency.ToUpper(), toCurrency.ToUpper());

                if (convertRate.Result != null)
                {
                    var returnAmount = amount * convertRate.Result.Rate;
                    return Ok(returnAmount.ToString());
                }

                return BadRequest(new ApiResponse(404, "Entered exchange rate has not found."));
            }
            catch (Exception ex)
            {
                if (ex.HResult >= 500)
                {
                    return BadRequest(new ApiResponse(500, ex.Message));
                }
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            
        }

        [HttpGet("getallrates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyList<CurrencyRate>>> GetRates()
        {
            try
            {
                var rates = await _repo.ListAllAsync();
                //rates = null;
                if (rates == null) return NotFound(new ApiResponse(404));
                var data = _mapper.Map<IReadOnlyList<CurrencyRate>, IReadOnlyList<CurrencyRateToReturnDto>>(rates);
                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.HResult>=500)
                {
                    return BadRequest(new ApiResponse(500, ex.Message));
                }
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("updaterates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IReadOnlyList<CurrencyRate>>> UpdateCurrencyRates([FromBody] List<CurrencyRateCheckDto> currencies)
        {
            try
            {
                var rates = await _repo.ListAllAsync();
                //rates = null;
                if (rates == null) return NotFound(new ApiResponse(404));
                return Ok(rates);
            }
            catch (Exception ex)
            {
                if (ex.HResult >= 500)
                {
                    return BadRequest(new ApiResponse(500, ex.Message));
                }
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
    }
}