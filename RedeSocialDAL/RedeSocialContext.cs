using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RedeSocialBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedeSocialDAL
{
    public class RedeSocialContext : DbContext
    {
        public RedeSocialContext(DbContextOptions<RedeSocialContext> options) : base(options) { }
        public DbSet<Postagem> Postagens { get; set; }
    }
}
