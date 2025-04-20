using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Must be atleast 5 characters long")]
        [MaxLength(10, ErrorMessage = "Title can't be too long")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(30, ErrorMessage = "Must be atleast 30 characters long and makes sense")]
        [MaxLength(280, ErrorMessage = "Content can't be just about yapping")]
        public string Content { get; set; } = string.Empty;
    }
}