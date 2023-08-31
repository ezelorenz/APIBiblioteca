using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;

namespace BibliotecaBitwise.DAL.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<bool> IsUniqueUser(string usuario);
        Task<AppUsuario> GetUsuario(string usuarioId);
        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

    }
}
