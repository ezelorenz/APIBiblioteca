using BibliotecaBitwise.DAL.DataContext;
using BibliotecaBitwise.DAL.Interfaces;
using BibliotecaBitwise.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaBitwise.DAL.Implementaciones
{
    public class LibroRepository : GenericRepository<Libro>, ILibroRepository
    {
        private readonly ApplicationDbContext _context;
        public LibroRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<Libro> ObtenerPorIdConRelacion(int id)
        {
            var query = await _context.Libros
                            .Include(a =>a.Autor)
                            .Include(g => g.Genero)
                            .Include(l => l.Comentarios)
                            .FirstOrDefaultAsync(l=> l.Id == id);

            return query;
        }
    }
}
