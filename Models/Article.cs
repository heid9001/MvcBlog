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
        private User _user;

        public Article()
        {
        }

        public Article(User user)
        {
            _user = user;
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

        public User User {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        [Column]
        [Required]
        public string Title{ get; set; }
        
        [Column]
        [Required]
        public string Content { get; set; }
    }
}
