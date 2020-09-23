using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RedeSocialBLL.Models;
using RedeSocialDAL;

namespace RedeSocialAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostagensController : ControllerBase
    {
        private readonly RedeSocialContext _context;

        public PostagensController(RedeSocialContext context)
        {
            _context = context;
        }

        // GET: api/Postagens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postagem>>> GetPostagens()
        {
            return await _context.Postagens.FromSqlRaw("EXEC GetPostagens").ToListAsync<Postagem>();
        }

        // GET: api/Postagens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postagem>> GetPostagem(Guid id)
        {
            var postagem = await _context.Postagens.FromSqlRaw("EXEC dbo.GetPostagem @Id", new SqlParameter("@Id", id)).ToListAsync();

            if (postagem == null)
            {
                return NotFound();
            }

            return postagem.First();
        }

        // PUT: api/Postagens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<Postagem>> PutPostagem(Guid id, Postagem postagem)
        {
            postagem.Id = id;
            _context.Database.ExecuteSqlRaw("EXEC dbo.UpdatePostagem @Id, @UsuarioId, @Mensagem, @URLImagem, @HoraPostagem",
            new SqlParameter("@Id", postagem.Id),
            new SqlParameter("@UsuarioId", postagem.UsuarioId),
            new SqlParameter("@Mensagem", postagem.Mensagem),
            new SqlParameter("@URLImagem", postagem.URLImagem),
            new SqlParameter("@HoraPostagem", postagem.HoraPostagem)
            );

            return postagem;
        }

        // POST: api/Postagens
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Postagem>> PostPostagem(Postagem postagem)
        {
            if (postagem.UsuarioId == null) { postagem.UsuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); }
            postagem.Id = Guid.NewGuid();
            postagem.HoraPostagem = DateTime.Now;

            _context.Database.ExecuteSqlRaw("EXEC dbo.PostPostagem @Id, @UsuarioId, @Mensagem, @URLImagem, @HoraPostagem",
            new SqlParameter("@Id", postagem.Id),
            new SqlParameter("@UsuarioId", postagem.UsuarioId),
            new SqlParameter("@Mensagem", postagem.Mensagem),
            new SqlParameter("@URLImagem", postagem.URLImagem),
            new SqlParameter("@HoraPostagem", postagem.HoraPostagem)
 );

            return CreatedAtAction("GetPostagem", new { id = postagem.Id }, postagem);
        }

        // DELETE: api/Postagens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Postagem>> DeletePostagem(Guid id)
        {
            var postagem = await _context.Postagens.FromSqlRaw("EXEC dbo.GetPostagem @Id", new SqlParameter("@Id", id)).ToListAsync();
            if (postagem == null)
            {
                return NotFound();
            }

            _context.Database.ExecuteSqlRaw("EXEC dbo.DeletePostagem @Id", new SqlParameter("@Id", id));

            return postagem.First();
        }

    }
}
