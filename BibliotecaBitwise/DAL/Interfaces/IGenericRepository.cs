namespace BibliotecaBitwise.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodos();
        Task<T>Obtener(int  id);
        Task<bool> Insertar(T entity);
        Task<bool> Actualizar(T entity);
        Task<bool> Eliminar(int id);
    }
}
