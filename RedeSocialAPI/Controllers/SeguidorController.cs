using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedeSocialBLL.Models;
using RedeSocialDAL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RedeSocialAPI.Controllers
{
    // [Authenticate]
    [Route("api/[controller]")]
    [ApiController]
    public class SeguidorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeguidorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Index()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetSeguidores(Guid id)
        {
            List<Usuario> seguidores = new List<Usuario>();

            foreach (Seguindo seguindo in await _context.Seguindo.ToListAsync())
            {
                if (seguindo.UsuarioId.Equals(id))
                {
                    seguidores.Add(await _context.Users.FindAsync(seguindo.SegueId.ToString()));
                }
            }

            return seguidores;
        }

        [HttpPost("{following}")]
        public async Task<ActionResult> Seguir(Guid? following, [FromBody] Guid follower)
        {
            Usuario usuario = new Usuario();
            List<Usuario> usuarios = await _context.Users.ToListAsync();

            foreach (Usuario user in usuarios)
            {
                if (Guid.Parse(user.Id) == following)
                {
                    usuario = user;
                }
            }

            Seguindo seguindo = new Seguindo();


            seguindo.UsuarioId = follower;
            seguindo.SegueId = Guid.Parse(usuario.Id);


            if (await _context.Seguindo.ContainsAsync(seguindo) != true)
            {
                _context.Seguindo.Add(seguindo);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{identifier}")]
        public async Task<ActionResult> Unfollow(String identifier)
        {
            string[] ids = identifier.Split("|");
            if (await _context.Seguindo.AnyAsync(s => s.SegueId == Guid.Parse(ids[0]) && s.UsuarioId == Guid.Parse(ids[1])))
            {
                _context.Seguindo.Remove(await _context.Seguindo.FirstAsync(s => s.SegueId == Guid.Parse(ids[0]) && s.UsuarioId == Guid.Parse(ids[1])));
                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
