namespace LandAPI.Models
{
    public class EvolutionDetails
    {
        public int? Min_Level { get; set; }
    }

    public class Chain
    {
        public string Species { get; set; }
        public List<EvolutionDetails> Evolution_Details { get; set; }
        public List<Chain> Evolves_To { get; set; }
    }

    public class EvolutionChain
    {
        public int ID { get; set; }
        public Chain Chain { get; set; }
    }
}
