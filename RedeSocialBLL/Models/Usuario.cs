using Microsoft.AspNetCore.Identity;
using RedeSocialBLL.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RedeSocialBLL.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        public string PrimeiroNome { get; set; }
        [Required]
        public string UltimoNome { get; set; }
        [Required]
        public DateTime Aniversario { get; set; }
    }
}
