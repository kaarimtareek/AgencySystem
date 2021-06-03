using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class LookupPostStatus : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
