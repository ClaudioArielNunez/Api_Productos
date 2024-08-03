using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Server.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caratéres")]
        public string Nombre { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres")]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName ="decimal(18,2)")]
        [DisplayFormat(DataFormatString ="{0:C2}")] //Formato moneda        
        public decimal Precio { get; set; }
    }
}
