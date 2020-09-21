using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RedeSocialBLL.Models;
using RedeSocialDAL;

namespace RedeSocialWEB.Controllers
{
    public class PostagensController : Controller
    {

        // GET: Postagens
        public async Task<IActionResult> Index()
        {
            List<Postagem> postagens = new List<Postagem>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Postagens/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postagens = JsonConvert.DeserializeObject<List<Postagem>>(apiResponse);
                }
            }
            return View(postagens);
        }

        // GET: Postagens/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            Postagem postagem = new Postagem();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Postagens/"+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postagem = JsonConvert.DeserializeObject<Postagem>(apiResponse);
                }
            }
            return View(postagem);
        }

        // GET: Postagens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Postagens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Postagem postagem)
        {
            Postagem novaPostagem = new Postagem();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient()) {

                    postagem.EmailUsuario = User.Identity.Name;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(postagem), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://localhost:44370/api/Postagens/", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        novaPostagem = JsonConvert.DeserializeObject<Postagem>(apiResponse);
                    }

                }
            }
            return View(novaPostagem);
        }

        // GET: Postagens/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            Postagem postagem = new Postagem();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Postagens/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postagem = JsonConvert.DeserializeObject<Postagem>(apiResponse);
                }
            }
            return View(postagem);
        }

        // POST: Postagens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EmailUsuario,Mensagem,URLImagem,HoraPostagem,Id")] Postagem postagem)
        {
            if (id != postagem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Postagem editarPostagem = new Postagem();
                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(postagem), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync("https://localhost:44370/api/Postagens/"+id, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        editarPostagem = JsonConvert.DeserializeObject<Postagem>(apiResponse);
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(postagem);
        }

        // GET: Postagens/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            Postagem postagem = new Postagem();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Postagens/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postagem = JsonConvert.DeserializeObject<Postagem>(apiResponse);
                }
            }
            return View(postagem);
        }

        // POST: Postagens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44370/api/Postagens/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
