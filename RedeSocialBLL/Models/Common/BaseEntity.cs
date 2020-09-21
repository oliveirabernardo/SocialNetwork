using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RedeSocialBLL.Models.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
