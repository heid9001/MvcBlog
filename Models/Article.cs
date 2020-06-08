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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("User")]
        public User user;

        [Column]
        [Required]
        public string Title{ get; set; }
        
        [Column]
        [Required]
        public string Content { get; set; }
    }
}
