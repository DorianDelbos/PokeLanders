using LandAPI.Models;

namespace LandAPI.Data
{
    public class TypeRepository
    {
        private List<Models.Type> _types;

        public TypeRepository()
        {
            InitializeTypes();
        }

        private void InitializeTypes()
        {
            _types = new List<Models.Type>
        {
            new Models.Type { ID = 1, Name = "water", Url = "https://localhost:7041/api/v1/type/1/" },
            new Models.Type { ID = 2, Name = "fire", Url = "https://localhost:7041/api/v1/type/2/" },
            new Models.Type { ID = 3, Name = "grass", Url = "https://localhost:7041/api/v1/type/3/" },
            new Models.Type { ID = 4, Name = "light", Url = "https://localhost:7041/api/v1/type/4/" },
            new Models.Type { ID = 5, Name = "dark", Url = "https://localhost:7041/api/v1/type/5/" }
        };
        }

        public List<Models.Type> GetAllTypes() => _types;

        public Models.Type GetTypeById(int id) => _types.FirstOrDefault(p => p.ID == id);

        public IEnumerable<Models.Type> GetTypeByName(string name)
        {
            return _types.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

}
