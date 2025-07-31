using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paessler.Task.Model
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
    }
}