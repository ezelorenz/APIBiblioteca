using Microsoft.AspNetCore.Identity;

namespace BibliotecaBitwise.Models
{
    public class AppUsuario : IdentityUser
    {
        //Añadir campos personalizados
        public string Nombre { get; set; }
    }
}
