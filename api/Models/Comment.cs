using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key] 
        [Column("Id")]
        public int Id { get; set; }
        [Column("Title")]
        public string Title { get; set; } = string.Empty;
        [Column("Content")]
        public string Content { get; set; } = string.Empty;
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [Column("StockId")]
        public int? StockId { get; set; }
        //Navigation Property
        public Stocks? Stock { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}