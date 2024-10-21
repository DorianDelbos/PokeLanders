namespace LandAPI.Models
{
    public class Lander
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Type> Types { get; set; }
        public int PV { get; set; }
        public int PhysicalAttack { get; set; }
        public int PhysicalDefense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
    }
}