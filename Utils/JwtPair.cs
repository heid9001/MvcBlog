using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogMVC.Utils
{
    public class JwtPair
    {
        public string Token { get; set; }
        public string Secret { get; set; }
    }
}