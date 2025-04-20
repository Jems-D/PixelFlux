using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace api.Models
{
    [Table("Stock")]
    public class Stocks
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Symbol")]
        public string Symbol { get; set; } = string.Empty;
        [Column("CompanyName")]
        public string CompanyName { get; set; } = string.Empty;
        [Column("Purchase", TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        [Column("LastDiv",TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        [Column("Industry")]
        public string Industry { get; set; } = string.Empty;
        [Column("MarketCap")]
        public long MarketCap { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}