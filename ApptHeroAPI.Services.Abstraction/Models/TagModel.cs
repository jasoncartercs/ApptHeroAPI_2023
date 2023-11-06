using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ApptHeroAPI.Services.Abstraction.Models
{
    public class TagModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class TagComparer : IEqualityComparer<TagModel>
    {

        public bool Equals([AllowNull] TagModel x, [AllowNull] TagModel y)
        {
            if (x.Id == y.Id) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] TagModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
