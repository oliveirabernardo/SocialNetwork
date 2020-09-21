using RedeSocialBLL.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocialBLL.Models
{
    public class Postagem : BaseEntity
    {
        public string EmailUsuario { get; set; }
        public string Mensagem { get; set; }
        public string URLImagem { get; set; }
        public DateTime HoraPostagem { get; set; }
    }
}
