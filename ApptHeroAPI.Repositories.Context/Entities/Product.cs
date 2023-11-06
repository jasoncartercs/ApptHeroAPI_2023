using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }
        public string Description { get; set; }
        
        [DataType("decimal(18,2)")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CompanyID { get; set; }
       
        [MaxLength(150)]
        public string Name { get; set; }

        public ICollection<AppointmentType> AppointmentType { get; set; }
    }
}
