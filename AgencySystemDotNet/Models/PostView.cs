using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class PostView : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PostId { get; set; }
        public virtual User Customer{ get; set; }
        public virtual Post Post{ get; set; }
    }
}
