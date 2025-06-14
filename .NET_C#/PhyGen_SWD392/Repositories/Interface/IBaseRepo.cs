namespace PhyGen_SWD392.Repositories.Interface
{
    public interface IBaseRepo<T, TKey> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<bool> UpdateAsync(TKey id, T entity);
        Task<bool> Delete(TKey id);
        Task<T?> GetByIdAsync(TKey id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
