using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Server.Models;
using System.ComponentModel;

namespace Productos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductosContext _context;

        public ProductosController(ProductosContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Los datos del producto no son válidos", //ModelState.Values: Accede a todos los valores del estado del modelo,
                    Errors = ModelState.Values.SelectMany(x => x.Errors) //SelectMany combina todos esos errores en una única colección.
                                                                         //Así, Errors es una lista de todos los errores de validación que se han producido.
                });
            }
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();
            //creando un objeto anonimo para devolver un mensaje rapido
            var response = new
            {
                Message = "La solicitud se completó correctamente.",
                Data = $"El producto {producto.Nombre} se ha creado con exito"
            };

            return Ok(response);
            
        }

        [HttpGet]
        [Route("lista")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListarProductos()
        {
            List<Producto> lista = await _context.Productos.ToListAsync();

            return Ok(lista);
            //cuando necesitamos poner en la firma el tipo de dato a devolver siempre debe ser
            //ActionResult<T>, ya que IActionResult no tiene información sobre el tipo de datos
            //que se está devolviendo,lo que lo hace más flexible, pero menos específico.
        }

        [HttpGet]
        [Route("ver/{id}")]
        public async Task<IActionResult> ListarPorId(int? id)
        {

            if (id != null)
            {
                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
                if (producto != null)
                {
                    return Ok(producto);
                }
                else
                {
                    var response = new
                    {
                        Message = $"No hay producto con el id {id}",
                        Data = id,
                    };

                    return NotFound(response);//error 404
                }
            }
            else
            {
                var response = new
                {
                    Message = $"El id {id} es nulo",
                    Data = id,
                };
                return BadRequest(response);//error 400
            }
        }

        [HttpPut]
        [Route("editar/{id}")]
        public async Task<IActionResult> ActualizarProducto(int? id, Producto producto)
        {
            if (id != null)
            {
                var prodExistente = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
                if (prodExistente != null)
                {
                    prodExistente.Nombre = producto.Nombre;
                    prodExistente.Descripcion = producto.Descripcion;
                    prodExistente.Precio = producto.Precio;
                    await _context.SaveChangesAsync();
                    var response = new
                    {
                        Message = "La solicitud se completó correctamente.",
                        Data = $"El producto {id} se ha modificado con exito"
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        Message = $"No hay producto con el id {id}",
                        Data = id,
                    };

                    return NotFound(response);//error 404
                }
            }
            else
            {
                var response = new
                {
                    Message = $"El id {id} es nulo",
                    Data = id,
                };
                return BadRequest(response);
            }
        }


        [HttpDelete]
        [Route("eliminar/{id}")]
        public async Task<IActionResult> EliminarProducto(int? id)
        {
            if (id != null)
            {
                var productoPoreliminar = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
                if (productoPoreliminar != null)
                {
                    _context.Remove(productoPoreliminar);
                    await _context.SaveChangesAsync();
                    var response = new
                    {
                        Message = "La solicitud se completó correctamente.",
                        Data = $"El producto {id} se ha eliminado con exito"
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        Message = $"No hay producto con el id {id}",
                        Data = id,
                    };
                    return NotFound(response);
                }
            }
            else
            {
                var response = new
                {
                    Message = $"El id {id} es nulo",
                    Data = id,
                };
                return BadRequest(response);
            }
        }

    }
}
