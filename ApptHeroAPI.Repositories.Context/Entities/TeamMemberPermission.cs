namespace ApptHeroAPI.Repositories.Context.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TeamMemberPermission
    {
        [Key]
        [Column(Order = 0)]
        public int PermissionId { get; set; }

        [Key]
        [Column(Order = 1)]
        public long PersonId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
