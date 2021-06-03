using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PressAgencyApp.Models
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(50)]
        [Required]
        public string Phonenumber { get; set; }

        public string Photo { get; set; }

        [MaxLength(50)]
        [Required]
        public string Role { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] Salt { get; set; }
        //for editor ( created posts)
        public virtual ICollection<Post> Posts { get; set; }
        //for customer 
        public virtual ICollection<PostQuestion> PostQuestions { get; set; }
        //for customer( the posts he viewed)
        public virtual ICollection<PostView> PostsViews { get; set; }
        //for customer ( the posts he interacted with ) dislike or like
        public virtual ICollection<PostInteraction> PostsInteractions { get; set; }
    }
}