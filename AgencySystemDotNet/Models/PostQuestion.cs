using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class PostQuestion : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int PostId { get; set; }
        public bool IsAnswered { get; set; }
        [Required]
        public string Question { get; set; }
        public string Answer { get; set; }
        public virtual Post Post { get; set; }
        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }
    }
}
