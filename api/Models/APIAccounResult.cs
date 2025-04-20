using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api.Models
{
    public class APIAccounResult<TPayLoad>
    {
        public int statusCode { get; set; }
        public List<Microsoft.AspNetCore.Identity.IdentityError>? Error { get; set; }
        public string? Message { get; set; } = string.Empty;
        public TPayLoad? Payload { get; set; }
        public bool isSuccess { get; set; }

    }
}