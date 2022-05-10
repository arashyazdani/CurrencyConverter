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
using Core.Specifications;
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
        public async Task<ActionResult<string>> Convert([FromQuery] GetConvertDto convertItems)
        {
            try
            {
                var convertRate = _currencyService.IsCurrencyCheckAsync(convertItems.FromCurrency.ToUpper(), convertItems.ToCurrency.ToUpper());

                if (convertRate.Result != null)
                {
                    var returnAmount = convertItems.Amount * convertRate.Result.Rate;
                    return Ok(new ApiResponse(200, "Your amount is: " + returnAmount.ToString()));
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
        public async Task<ActionResult<IReadOnlyList<CurrencyRate>>> UpdateCurrencyRates([FromBody] IList<CurrencyRateSpec> currencies)
        {
            try
            {
                var rates = await _currencyService.UpdateCurrencyRatesAsync(currencies);

                if (rates.Any())
                {
                    return Ok(rates);
                }
                
                return BadRequest(new ApiResponse(400));
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

        [HttpDelete("deleteallrates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteAllCurrencyRates()
        {
            try
            {
                var deletedRates = await _currencyService.DeleteAllCurrencyRatesAsync();
                
                if (!deletedRates) return BadRequest(new ApiResponse(400));

                return Ok(new ApiResponse(200, "All rates has been deleted."));
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