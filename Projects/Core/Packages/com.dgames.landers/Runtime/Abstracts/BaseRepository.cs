using System.Threading.Tasks;

namespace Landers
{
    public abstract class BaseRepository<T>
    {
        protected T[] modelList;
        public abstract Task<bool> Initialize();

        public T[] GetAll() => modelList;
    }
}
