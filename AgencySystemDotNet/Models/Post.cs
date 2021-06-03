using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class Post : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EditorId { get; set; }
        [MaxLength(50)]
        public string StatusId { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public string Body { get; set; }
        public string Photo { get; set; }
        public int ViewersNumber{ get; set; }
        public int DislikesNumber { get; set; }
        public int LikesNumber { get; set; }
        public int CategoryId { get; set; }
        public virtual User Editor { get; set; }
        public virtual LookupPostCategory Category { get; set; }
        public virtual LookupPostStatus Status{ get; set; }
        public virtual ICollection<PostView> Views { get; set; }
        public virtual ICollection<PostInteraction> Interactions { get; set; }
        public virtual ICollection<PostQuestion> Questions{ get; set; }
        public virtual ICollection<SavedPost> Saved{ get; set; }
    }
}
