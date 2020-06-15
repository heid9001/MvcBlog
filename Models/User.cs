using BlogMVC.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;


namespace BlogMVC.Models
{

    [Table("users")]
    public class User : IIdentity
    {

        string _role;


        public User()
        {
            Articles = new List<Article>();
        }

        public User(string name, JwtPair pair, bool isAuthenticated)
        {
            Name = name;
            AuthorizeToken = pair.Token;
            IdentityKey = pair.Secret;
            IsAuthenticated = isAuthenticated;
        }

        public string AuthenticationType => "jwt";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column]
        [Required]
        public string Name { get; set; }

        [Column]
        public bool IsAuthenticated { get; set; } = false;

        [Column]
        public string IdentityKey { get; set; }

        [Column]
        public string AuthorizeToken { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string Role {

            get {
                if (_role != null)
                {
                    return _role;
                }
                if (AuthorizeToken == null)
                {
                    return null;
                }
                var result = new JwtSecurityTokenHandler().ReadJwtToken(AuthorizeToken);
                _role = (from c in result.Claims
                         where c.Type.Equals("Role")
                         select c.Value).SingleOrDefault<string>();
                return _role;
            }
            set {
                _role = value;
            } 
        }

        public virtual ICollection<Article> Articles { get; set; }
    }

}
