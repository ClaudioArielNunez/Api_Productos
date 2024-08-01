using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace Productos.Server.Models
{
    public class ProductosContext : DbContext
    {
        public ProductosContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        //este código asegura que cualquier configuración básica del modelo de datos
        //definida en la clase base (DbContext) se mantenga y permite agregar configuraciones
        //adicionales específicas del modelo a través del parámetro modelBuilder.
        //Si planeas agregar configuraciones personalizadas como definir relaciones entre
        //entidades, configuraciones o restricciones, entonces sí es útil para incluir esas configuraciones.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Producto>().HasIndex(p => p.Nombre).IsUnique();
            //Evitamos duplicados de nombre:
            //.HasIndex(p => p.Nombre): Indica que se debe crear un índice en la columna Nombre de la tabla Producto.
            //.IsUnique(): Especifica que el índice debe ser único, lo que significa que no puede haber dos productos con el mismo valor en la columna Nombre.
        }
        
    }
}
