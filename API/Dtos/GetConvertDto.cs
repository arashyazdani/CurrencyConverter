using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class GetConvertDto
    {
        [Required(ErrorMessage = "From Currency is required.")]
        [RegularExpression(@"^[A-Z]{3,3}$", ErrorMessage = "From Currency must be 3 uppercase capital letters.")]
        [Display(Name = "From Currency")]
        public string FromCurrency { get; set; }

        [Required(ErrorMessage = "ToCurrency is required.")]
        [RegularExpression(@"^[A-Z]{3,3}$", ErrorMessage = "To Currency must be 3 uppercase capital letters.")]
        [Display(Name = "To Currency")]
        public string ToCurrency { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [DisplayFormat(DataFormatString = "{0:000000}")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a decimal number.")]
        [RegularExpression(@"^(?=.*[1-9])[0-9]*[.,]?[0-9]{1,7}$", ErrorMessage = "Amount must be a decimal number.")]
        public decimal Amount { get; set; }
    }
}
