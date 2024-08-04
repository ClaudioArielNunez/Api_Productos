using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using Productos.Server.Migrations;
using Productos.Server.Models;
using System.Text;

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

        //Metodo Get
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                //Convierte el objeto producto a una cadena JSON
                var json = JsonConvert.SerializeObject(producto);
                /*
                 StringContent: Es una clase en C# que se utiliza para representar el contenido de una solicitud HTTP en formato de texto.
                 json es el contenido que quieres enviar en el cuerpo de la solicitud HTTP.
                 Encoding.UTF8: Especifica el tipo de codificación para convertir la cadena json en bytes.
                 Tipo de contenido: Se indica que el contenido es de tipo application/json, lo cual es crucial para que el servidor entienda que estás enviando datos en formato JSON.
                 Sin la creación de StringContent, no podrías enviar el JSON correctamente en la solicitud POST. 
                 */
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Usa _httpClient para enviar una solicitud HTTP POST al endpoint api/Productos/crear
                var response = await _httpClient.PostAsync("api/Productos/crear", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //agrega un mensaje de error al ModelState.
                    ModelState.AddModelError(string.Empty, "Error al crear producto");
                }
            }

            return View(producto);
        }

        //httpGet
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Productos/ver/{id}"); //api/Productos/ver/3

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(json);

                return View(producto);
            }
            else
            {
                return RedirectToAction("Details"); 
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductoViewModel producto)//api/Productos/editar/3     
        {                                                                                 
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Productos/editar/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar producto");
                }
            }
            return View(producto);
        }

        //HttpGet
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/Productos/ver/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(json);

                return View(producto);
            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        //HttpGet
        public async Task<IActionResult> Delete(int id) //api/Productos/eliminar/{id}
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Productos/eliminar/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "El producto ha sido eliminado con exito!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "No se ha podido eliminar el producto";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;

                return RedirectToAction("Index");
            }
        }
    }
}
