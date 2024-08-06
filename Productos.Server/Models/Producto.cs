using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Server.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es un campo obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caratéres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La {0} es un campo obligatorio")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")] //Formato moneda
        [Required(ErrorMessage = "El precio es un campo obligatorio")]
        public decimal? Precio { get; set; }
        //El signo  ? es necesario para que el mensaje de error se muestre correctamente con las prop numericas
    }
}
