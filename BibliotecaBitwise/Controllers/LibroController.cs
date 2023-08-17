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

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>>Obtener(int id)
        {
            var libro = await _repository.Obtener(id);
            if(libro == null)
                return NotFound();

            var libroDto = _mapper.Map<LibroDTO>(libro);
            return Ok(libroDto);
        }

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
    }
}
