using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        [Column("AppUserId")]
        public string AppUserId { get; set; }
        [Column("StockId")]
        public int StockId { get; set; }
        public AppUser AppUser { get; set; }
        public Stocks Stock { get; set; }

    }
}