using LandAPI.Models;

namespace LandAPI.Data
{
    public class LanderRepository
    {
        private readonly TypeRepository _typeRepository;

        private List<Lander> _landers;

        public LanderRepository(TypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
            InitializeLanders();
        }

        private void InitializeLanders()
        {
            var types = _typeRepository.GetAllTypes();

            var waterType = types.FirstOrDefault(t => t.Name.Equals("water", StringComparison.OrdinalIgnoreCase));
            var fireType = types.FirstOrDefault(t => t.Name.Equals("fire", StringComparison.OrdinalIgnoreCase));
            var grassType = types.FirstOrDefault(t => t.Name.Equals("grass", StringComparison.OrdinalIgnoreCase));
            var lightType = types.FirstOrDefault(t => t.Name.Equals("light", StringComparison.OrdinalIgnoreCase));
            var darkType = types.FirstOrDefault(t => t.Name.Equals("dark", StringComparison.OrdinalIgnoreCase));

            _landers = new List<Lander>
            {
                new Lander
                {
                    ID = 1,
                    Name = "Aquapix",
                    Description = "Aquapix est un Pokémon farceur qui vit au bord des rivières et des lacs. Grâce à sa queue en forme de fontaine, il peut projeter de fines gouttelettes scintillantes, capables de calmer les esprits agités. Les marins racontent que les arcs-en-ciel apparaissent souvent après le passage d'un Aquapix.",
                    PV = 44,
                    PhysicalAttack = 48,
                    PhysicalDefense = 65,
                    SpecialAttack = 50,
                    SpecialDefense = 64,
                    Speed = 43,
                    Types = new List<Models.Type> { waterType }
                },
                new Lander
                {
                    ID = 2,
                    Name = "Luminorine",
                    Description = "Luminorine est vénérée dans certaines cultures côtières comme le gardien des océans. Grâce à sa danse aquatique, elle est capable de calmer même les tempêtes les plus furieuses. Ses chants, portés par le vent marin, inspirent espoir et courage à ceux qui les entendent.",
                    PV = 59,
                    PhysicalAttack = 63,
                    PhysicalDefense = 80,
                    SpecialAttack = 65,
                    SpecialDefense = 80,
                    Speed = 58,
                    Types = new List<Models.Type> { waterType, lightType }
                },
                new Lander
                {
                    ID = 3,
                    Name = "Ocealythe",
                    Description = "Ocealythe est vénéré comme un protecteur divin des océans et des créatures marines. Ses pouvoirs mystiques sont capables d'influencer les courants océaniques et même de contrôler le climat. Les légendes racontent que lorsque Ocealythe apparaît, les tempêtes s'apaisent et les eaux deviennent claires et lumineuses.",
                    PV = 79,
                    PhysicalAttack = 83,
                    PhysicalDefense = 83,
                    SpecialAttack = 85,
                    SpecialDefense = 105,
                    Speed = 78,
                    Types = new List<Models.Type> { waterType, lightType }
                }
            };
        }

        public List<Lander> GetAllLanders() => _landers;

        public Lander GetLanderById(int id) => _landers.FirstOrDefault(p => p.ID == id);

        public IEnumerable<Lander> GetLanderByType(string type)
        {
            return _landers.Where(p =>
                p.Types.Any(t => t.Name.Equals(type, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
