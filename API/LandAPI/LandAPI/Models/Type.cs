namespace LandAPI.Models
{
	public class DamageRelations
	{
		public String[] Double_Damage_From { get; set; }
		public String[] Double_Damage_To { get; set; }
		public String[] Half_Damage_From { get; set; }
		public String[] Half_Damage_To { get; set; }
		public String[] None_Damage_From { get; set; }
		public String[] None_Damage_To { get; set; }
	}

	public class Type
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public DamageRelations Damage_Relations { get; set; }
	}
}
