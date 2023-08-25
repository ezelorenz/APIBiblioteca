using AutoMapper;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BibliotecaBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IGenericRepository<Usuario> _repository;
        private readonly IUsuarioRepository _userRepository;
        private readonly IMapper _mapper;
        protected RespuestaApi _respuesta;

        public UsuarioController(IGenericRepository<Usuario> repository,
                                IUsuarioRepository userRepository,
                                IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _userRepository = userRepository;
            this._respuesta = new ();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerTodos()
        {
            var usuarios = await _repository.ObtenerTodos();
            var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(usuariosDto);
        }

        [HttpPost("registro")]

        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            var validacionNombre = await _userRepository.IsUniqueUser(usuarioRegistroDto.NombreUsuario);
            if (!validacionNombre)
            {
                _respuesta.StatusCode = HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMenssages.Add("El nombre del usuario ya existe");
                return BadRequest(_respuesta);
            }

            var usuario = await _userRepository.Registro(usuarioRegistroDto);
            if(usuario == null)
            {
                _respuesta.StatusCode = HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMenssages.Add("Error en el registro");
                return BadRequest(_respuesta);
            }

            _respuesta.StatusCode = HttpStatusCode.OK;
            _respuesta.IsSuccess = true;
            return Ok(_respuesta);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _userRepository.Login(usuarioLoginDto);
            if(respuestaLogin.Usuario == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuesta.StatusCode =HttpStatusCode.BadRequest;
                _respuesta.IsSuccess = false;
                _respuesta.ErrorMenssages.Add("El nombre de usuario o el password son incorrectos");
                return BadRequest(_respuesta);
            }

            _respuesta.StatusCode=HttpStatusCode.OK;
            _respuesta.IsSuccess = true;
            _respuesta.Result = respuestaLogin;
            return Ok(_respuesta);
        }

    }
}
