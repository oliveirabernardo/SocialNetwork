using RedeSocialBLL.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RedeSocialBLL.Models
{
    public class Seguindo : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        public Guid SegueId { get; set; }
    }
}
