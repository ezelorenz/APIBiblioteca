using AutoMapper;
using BibliotecaBitwise.DAL.DataContext;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace BibliotecaBitwise.DAL.Implementaciones
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UsuarioRepository(ApplicationDbContext context, IConfiguration config
            ,UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper) : base(context)
        {
            _context = context;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<AppUsuario> GetUsuario(string usuarioId)
        {
            return await _context.AppUsuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
        }

        public async Task<bool> IsUniqueUser(string usuario)
        {
            var usuarioDb = await _context.AppUsuarios.FirstOrDefaultAsync(u => u.UserName == usuario);
            if (usuarioDb == null)
                return true;

            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passworEncriptado = ObtenerMD5(usuarioLoginDto.Password);

            var usuarioEncontrado = await _context.AppUsuarios.FirstOrDefaultAsync(
                                            u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuarioEncontrado, usuarioLoginDto.Password);

            if( usuarioEncontrado == null || isValid == false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var roles = await _userManager.GetRolesAsync(usuarioEncontrado);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuarioEncontrado)
            };
            return usuarioLoginRespuestaDto;
        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            //var passwordEncriptador = ObtenerMD5(usuarioRegistroDto.Password);

            var usuarioNuevo = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuarioNuevo, usuarioRegistroDto.Password);

            if(result.Succeeded)
            {
                //Solo la primera vez y es para crear los roles  
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                await _userManager.AddToRoleAsync(usuarioNuevo, "admin");
                var usuarioRetornado = await _context.AppUsuarios.FirstOrDefaultAsync(u => u.UserName == usuarioRegistroDto.NombreUsuario);

                //mapeo manual
                /*return new UsuarioDatosDto()
                {
                    Id = usuarioRetornado.Id,
                    UserName = usuarioRetornado.UserName,
                    Nombre = usuarioRetornado.Nombre
                };*/

                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }
            return null;
        }

        
    }
}
