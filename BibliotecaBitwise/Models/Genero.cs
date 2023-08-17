
namespace BibliotecaBitwise.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public HashSet<Libro> Libros { get; set; } = new HashSet<Libro>();
    }
}
