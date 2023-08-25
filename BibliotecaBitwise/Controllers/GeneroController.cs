﻿using AutoMapper;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.DTO;
using BibliotecaBitwise.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly IGenericRepository<Genero> _repository;
        private readonly IGeneroRepository _generoRepository;
        private readonly IMapper _mapper;
        public GeneroController(IGenericRepository<Genero> repository, 
                                IMapper mapper,
                                IGeneroRepository generoRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _generoRepository = generoRepository;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroDTO>>> ObtenerTodos()
        {
            var generos = await _repository.ObtenerTodos();
            var generosDTO = _mapper.Map<IEnumerable<GeneroDTO>>(generos);
            return Ok(generosDTO);
        }

        [HttpGet("{id}", Name = "GetGenero")]
        public async Task<ActionResult<GeneroDTO>> Obtener(int id)
        {
            var genero = await _repository.Obtener(id);
            if (genero == null)
                return NotFound();

            var generoDto = _mapper.Map<GeneroDTO>(genero);
            return Ok(generoDto);
        }

        [HttpGet("conlibros")]
        public async Task<ActionResult<IEnumerable<GeneroDTO>>> GeneroConLibros()
        {
            var generosconLibros = await _generoRepository.ObtenerConLibros();
            var generosconLibrosDto = _mapper.Map<IEnumerable<GeneroDTO>>(generosconLibros);
            return Ok(generosconLibrosDto);
        }

        [HttpPost]
        public async Task<ActionResult<GeneroDTO>>Crear(GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _mapper.Map<Genero>(generoCreacionDTO);

            await _repository.Insertar(genero);
            var generoDTO = _mapper.Map<GeneroDTO>(genero);
            return Ok(generoDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await _repository.Obtener(id);
            if (genero == null)
                return NotFound();

            _mapper.Map(generoCreacionDTO, genero);
            var resultado = await _repository.Actualizar(genero);
            if (resultado)
                return NoContent();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var generoDesdeRepo = await _repository.Obtener(id);
            if (generoDesdeRepo == null)
                return NotFound();

            var resultado = await _repository.Eliminar(id);
            if (resultado)
                return NoContent();

            return BadRequest();
        }
    }
}
