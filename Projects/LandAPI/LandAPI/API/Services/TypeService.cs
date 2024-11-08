using LandAPI.API.Data;

namespace LandAPI.API.Services
{
    public class TypeService
    {
        private readonly TypeRepository _typeRepository;

        public TypeService(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public List<Models.Type> GetAllTypes()
            => _typeRepository.Types;

        public Models.Type GetTypeById(int id)
            => _typeRepository.Types.FirstOrDefault(p => p.ID == id);


        public IEnumerable<Models.Type> GetTypeByName(string name)
            => _typeRepository.Types.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    }
}
