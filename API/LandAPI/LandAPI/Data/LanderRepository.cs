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
                    Description = "Aquapix is a mischievous Pokémon that lives by rivers and lakes. Thanks to its fountain-shaped tail, it can spray fine, shimmering droplets that have the power to calm restless minds. Sailors often say that rainbows appear after an Aquapix passes by.",
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
                    Description = "\r\nLuminorine is revered in certain coastal cultures as the guardian of the oceans. Through its aquatic dance, it can calm even the fiercest storms. Its songs, carried by the sea breeze, inspire hope and courage in those who hear them.",
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
                    Description = "Ocealythe is revered as a divine protector of the oceans and marine creatures. Its mystical powers can influence ocean currents and even control the weather. Legends say that when Ocealythe appears, storms subside, and the waters become clear and luminous.",
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
