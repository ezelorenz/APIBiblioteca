using AutoMapper;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaBitwise.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IGenericRepository<Autor> _repository;
        private readonly IMapper _mapper;

        public AutorController(IGenericRepository<Autor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> ObtenerTodos()
        {
            var autores = await _repository.ObtenerTodos();
            var autoresDTO = _mapper.Map<IEnumerable<AutorDTO>>(autores);
            return Ok(autoresDTO);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("soloAutores")]
        public async Task<ActionResult<IEnumerable<Autor>>> obtener()
        {
            var autores = await _repository.ObtenerTodos();
            
            return Ok(autores);
        }

        [HttpPost]
        public async Task<ActionResult>Crear(AutorCreacionDTO autorCreacionDTO)
        {
            var autor = _mapper.Map<Autor>(autorCreacionDTO);

            var resultado = await _repository.Insertar(autor);
            if (!resultado)
            {
                return NotFound();
            }
            var dto = _mapper.Map<AutorDTO>(autor);

            return Ok(dto);
        }
    }
}
