using BibliotecaBitwise.Models;

namespace BibliotecaBitwise.DTO
{
    public class UsuarioLoginRespuestaDto
    {
        public UsuarioDatosDto Usuario { get; set; }
        public string Token { get; set; }
    }
}
