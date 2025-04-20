using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class APIResult<TPayLoad>
    {
        public int statusCode { get; set; }
        public string Message { get; set; }  = string.Empty;
        public TPayLoad? Payload { get; set; }
        public bool isSuccess { get; set; }

    }
}