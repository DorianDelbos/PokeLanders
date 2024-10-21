using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
    public class TypeService
    {
        private readonly TypeRepository _typeRepository;

        public TypeService(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public List<Models.Type> GetAllTypes()
        {
            return _typeRepository.GetAllTypes();
        }

        public Models.Type GetLanderById(int id)
        {
            return _typeRepository.GetTypeById(id);
        }

        public IEnumerable<Models.Type> GetTypeByName(string name)
        {
            return _typeRepository.GetTypeByName(name);
        }
    }
}
