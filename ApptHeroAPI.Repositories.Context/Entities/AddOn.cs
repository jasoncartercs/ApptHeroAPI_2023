using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Addon
    {
        [Key]
        public int AddonId { get; set; }

        public int Duration { get; set; }
        public bool IsVisible { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; }
    }
}
