using System;
using System.ComponentModel.DataAnnotations;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class ApiErrorLog
    {
        [Key]
        public long Id { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string ApiVersion { get; set; }
        public string Source { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
