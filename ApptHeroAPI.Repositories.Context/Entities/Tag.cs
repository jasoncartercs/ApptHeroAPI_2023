using System.ComponentModel.DataAnnotations;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class Tag
    {
        [Key]
        public long TagId { get; set; }
        public string TagName { get; set; }
        public int TagTypeId { get; set; }
    }
}
