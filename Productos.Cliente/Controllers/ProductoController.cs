﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using Productos.Server.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Productos.Cliente.Controllers
{
    public class ProductoController : Controller
    {
        //HttpClient es una clase usada para enviar solicitudes HTTP 
        private readonly HttpClient _httpClient;

        //IHttpClientFactory es una interfaz que crea instancias de HttpClient        
        //lo usamos para crear una instancia de HttpClient y asignarla

        //BaseAddress configura la dirección base que se usará para todas las solicitudes
        //realizadas con este HttpClient
        public ProductoController(IHttpClientFactory httpClientFactory)
        {            
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7277/api");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Productos/lista");

            if (response.IsSuccessStatusCode) //si da 200-299
            {
                //leemos contenido de response
                var content = await response.Content.ReadAsStringAsync();
                //Deserializamos, instalando Newtonsoft.Json;
                var productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);
                //retornamos la lista
                return View("Index", productos);
            }

            return View(new List<ProductoViewModel>()); //devuelve una lista vacia
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Productos/crear", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear producto");
                }
            }

            return View(producto);
        }
    }
}
