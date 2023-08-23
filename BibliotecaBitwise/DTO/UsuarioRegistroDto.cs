using System.ComponentModel.DataAnnotations;

namespace BibliotecaBitwise.DTO
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        [MinLength(10, ErrorMessage = "El password es muy corto. Debe tener al menos {1} caracteres.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
