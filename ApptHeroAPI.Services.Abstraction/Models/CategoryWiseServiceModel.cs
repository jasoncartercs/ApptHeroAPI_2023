using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class CategoryWiseServiceModel
    {
        public string CategoryName { get; set; }

        public long CategoryId { get; set; }
        public List<ProductModel> Products { get; set; }
        //public string ServiceName { get; set; }
        //public int Duration { get; set; }
        //public decimal Price { get; set; }
        //public long ProductId { get; set; }
        //public long AppointmentTypeId { get; set; }
        //public long AppointmentTypeCategoryId { get; set; }
    }

    public class ProductModel
    {
        public string ServiceName { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public long ProductId { get; set; }
        public long AppointmentTypeId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public long CompanyID { get; set; }
        //public long AppointmentTypeCategoryId { get; set; }
    }
}
