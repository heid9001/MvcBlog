using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BlogMVC.Models
{
    [Table("articles")]
    public class Article
    {

        public Article()
        {
        }

        public Article(User user)
        {
            User = user;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        
        [ForeignKey("User")]
        public long? UserFK {
            get
            {
                return User.Id;
            }
        }

        public virtual User User { get; set; }

        [Column]
        [Required]
        public string Title{ get; set; }
        
        [Column]
        [Required]
        public string Content { get; set; }
    }
}
