using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;

namespace BibliotecaBitwise.DAL.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<bool> IsUniqueUser(string usuario);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
    }
}
