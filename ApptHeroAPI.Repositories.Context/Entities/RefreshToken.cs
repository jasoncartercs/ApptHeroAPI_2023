using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApptHeroAPI.Repositories.Context.Entities
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefreshTokenId { get; set; }

        [ForeignKey("Person")]
        public long PersonId { get; set; }
        public virtual Person Person { get; set; }  

        [Required]
        [MaxLength(450)]
        public string Token { get; set; }

        [Required]
        public DateTimeOffset Expiration { get; set; }

        [Required]
        public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? Revoked { get; set; }

        [ForeignKey("ReplacedByToken")]
        public int? ReplacedByTokenId { get; set; }
        public virtual RefreshToken ReplacedByToken { get; set; }
    }
}
