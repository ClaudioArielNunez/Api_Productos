using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Productos.Cliente.Models
{
    public class ProductoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caratéres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.MultilineText)]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El {0} es un campo obligatorio")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")] //Formato moneda
        public decimal? Precio { get; set; }
    }
}
