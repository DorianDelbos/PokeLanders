using LandAPI.Models;

namespace LandAPI.Data
{
    public class LanderRepository
    {
        private List<Lander> _landers;

        public LanderRepository()
        {
            InitializeLanders();
        }

        private void InitializeLanders()
        {
			_landers = new List<Lander>
            {
                new Lander
                {
                    ID = 1,
                    Name = "Aquapix",
                    Description = "Aquapix is a mischievous Pokémon that lives by rivers and lakes. Thanks to its fountain-shaped tail, it can spray fine, shimmering droplets that have the power to calm restless minds. Sailors often say that rainbows appear after an Aquapix passes by.",
                    Stats = new List<Stats>
                    {
						new Stats { Base_Stat = 44, Stat = "pv" },
						new Stats { Base_Stat = 48, Stat = "attack" },
						new Stats { Base_Stat = 65, Stat = "defence" },
						new Stats { Base_Stat = 50, Stat = "special-attack" },
						new Stats { Base_Stat = 64, Stat = "special-defence" },
						new Stats { Base_Stat = 43, Stat = "speed" }
					},
                    Base_Experience = 63,
                    Base_Height = 5,
                    Base_Weight = 90,
                    Types = new List<string> { "water" },
                    Model = "https://localhost:7041/api/model/1.glb"
                },
                new Lander
                {
                    ID = 2,
                    Name = "Luminorine",
                    Description = "\r\nLuminorine is revered in certain coastal cultures as the guardian of the oceans. Through its aquatic dance, it can calm even the fiercest storms. Its songs, carried by the sea breeze, inspire hope and courage in those who hear them.",
					Stats = new List<Stats>
					{
						new Stats { Base_Stat = 59, Stat = "pv" },
						new Stats { Base_Stat = 63, Stat = "attack" },
						new Stats { Base_Stat = 80, Stat = "defence" },
						new Stats { Base_Stat = 65, Stat = "special-attack" },
						new Stats { Base_Stat = 80, Stat = "special-defence" },
						new Stats { Base_Stat = 58, Stat = "speed" }
					},
					Base_Experience = 142,
					Base_Height = 10,
					Base_Weight = 225,
					Types = new List<string> { "water", "light" },
                    Model = "https://localhost:7041/api/model/2.glb"
                },
                new Lander
                {
                    ID = 3,
                    Name = "Ocealythe",
                    Description = "Ocealythe is revered as a divine protector of the oceans and marine creatures. Its mystical powers can influence ocean currents and even control the weather. Legends say that when Ocealythe appears, storms subside, and the waters become clear and luminous.",
					Stats = new List<Stats>
					{
						new Stats { Base_Stat = 79, Stat = "pv" },
						new Stats { Base_Stat = 83, Stat = "attack" },
						new Stats { Base_Stat = 100, Stat = "defence" },
						new Stats { Base_Stat = 85, Stat = "special-attack" },
						new Stats { Base_Stat = 105, Stat = "special-defence" },
						new Stats { Base_Stat = 78, Stat = "speed" }
					},
					Base_Experience = 265,
					Base_Height = 16,
					Base_Weight = 855,
					Types = new List<string> { "water", "light" },
                    Model = "https://localhost:7041/api/model/3.glb"
                }
            };
        }

        public List<Lander> GetAllLanders() => _landers;

        public Lander GetLanderById(int id) => _landers.FirstOrDefault(p => p.ID == id);

        public IEnumerable<Lander> GetLanderByType(string type)
        {
            return _landers.Where(p =>
                p.Types.Any(t => t.Equals(type, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
