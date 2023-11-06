using System;
using System.Collections.Generic;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class TeammateModel
    {

        public TeammateModel()
        {
            AddonModels = new List<AddOnModel>();
            PersonModel = new PersonModel();
            AdminModel  = new PersonModel();
        }
        public PersonModel PersonModel { get; set; }
        public PersonModel AdminModel { get; set; }
        public List<AddOnModel> AddonModels { get; }
    }
}
