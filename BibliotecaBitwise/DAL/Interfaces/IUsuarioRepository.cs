using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;

namespace BibliotecaBitwise.DAL.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<bool> IsUniqueUser(string usuario);
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);
        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);
    }
}
