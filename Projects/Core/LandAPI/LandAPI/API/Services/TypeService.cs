namespace LandAPI.API
{
    public class TypeService
    {
        private readonly TypeRepository _typeRepository;

        public TypeService(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public List<Type> GetAllTypes()
            => _typeRepository.Types;

        public Type? GetTypeById(int id)
            => _typeRepository.Types.FirstOrDefault(p => p.ID == id);


        public Type? GetTypeByName(string name)
            => _typeRepository.Types.First(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    }
}
