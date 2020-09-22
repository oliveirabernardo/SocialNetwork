using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedeSocialBLL.Models;

namespace RedeSocialWEB.Controllers
{
    public class UsuarioController : Controller
    {
        public class UserLists
        {
            public List<Usuario> UnfilteredUsers { get; set; }

            public List<Usuario> FilteredUsers { get; set; }
        }

        public async Task<IActionResult> Index()
        {
            UserLists listaUsers = new UserLists();
            List<Usuario> unfiltered = new List<Usuario>();
            List<Usuario> filtered = new List<Usuario>();

            if (User.Identity.IsAuthenticated)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44370/api/Seguidor/"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        unfiltered = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse);
                    }
                }
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44370/api/Seguidor/" + User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        filtered = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse);
                    }

                }
                listaUsers.UnfilteredUsers = unfiltered;
                listaUsers.FilteredUsers = filtered;
            }
            return View(listaUsers);
        }

        public async Task<ActionResult<List<Usuario>>> Seguindo(Guid? id)
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44370/api/Seguidor/"+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse);
                }

            }
            return usuarios;
        }


        [HttpGet]
        public async Task<IActionResult> Seguir(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44370/api/Seguidor/" + id, content))
                {
                    Ok();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Unfollow(Guid? id)
        {
            using (var httpClient = new HttpClient())
            {
                String identifier = id + "|" + User.FindFirst(ClaimTypes.NameIdentifier).Value;

                using (var response = await httpClient.DeleteAsync("https://localhost:44370/api/Seguidor/" + identifier))
                {
                    Ok();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
