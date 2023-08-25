using AutoMapper;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace BibliotecaBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IGenericRepository<Libro> _repository;
        private readonly ILibroRepository _libroRepository;
        
        private readonly IMapper _mapper;

        public LibroController(IGenericRepository<Libro> repositor, 
                                IMapper mapper,
                                ILibroRepository libroRepository)
        {
            _mapper = mapper;
            _repository = repositor;
            _libroRepository = libroRepository;
        }

        [ResponseCache(Duration = 30)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> ObtenerTodos()
        {
            var libros = await _repository.ObtenerTodos();
            var librosDTO = _mapper.Map<IEnumerable<LibroDTO>>(libros);
            return Ok(librosDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>>Obtener(int id)
        {
            var libro = await _repository.Obtener(id);
            if(libro == null)
                return NotFound();

            var libroDto = _mapper.Map<LibroDTO>(libro);
            return Ok(libroDto);
        }

        [ResponseCache(Duration = 30)]
        [HttpGet("dataRelacionada/{id}")]
        public async Task<ActionResult<LibroDTO>> ObtenerRelacionada(int id)
        {
            var libro = await _libroRepository.ObtenerPorIdConRelacion(id);
            if( libro == null)
                return NotFound();

            var libroDto = _mapper.Map<LibroDTO>(libro);
            return Ok(libroDto);
        }

        [HttpPost]
        public async Task<ActionResult<LibroDTO>>Crear(LibroCreacionDTO libroCreacionDTO)
        {
            var libro = _mapper.Map<Libro>(libroCreacionDTO);
            await _repository.Insertar(libro);

            var libroDTO = _mapper.Map<LibroDTO>(libro);
            return CreatedAtAction(nameof(Obtener), new { id = libro.Id }, libroDTO);    
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroDesdeRepo = await _repository.Obtener(id);
            if (libroDesdeRepo == null)
                return NotFound();

            _mapper.Map(libroCreacionDTO, libroDesdeRepo);
            var resultado = await _repository.Actualizar(libroDesdeRepo);
            if (resultado)
                return NoContent();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var libroDesdeRepo = await _repository.Obtener(id);
            if (libroDesdeRepo == null)
                return NotFound();

            var resultado = await _repository.Eliminar(id);
            if (resultado)
                return NoContent();

            return BadRequest();
        }
    }
}
