using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stocks
{
    public class CreateStockRequestDTO
    {
        [Required]
        [MaxLength(10, ErrorMessage ="Symbol can't be that long")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(50, ErrorMessage ="Company Name can't be that long")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(000.1, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage ="Industry can't be that long")]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(1 , 5000000000000)]
        public long MarketCap { get; set; }
    }
}