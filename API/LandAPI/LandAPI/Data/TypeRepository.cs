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
                new Models.Type 
                { 
                    ID = 1, 
                    Name = "water",
					Damage_Relations = new DamageRelations
					{
						Double_Damage_From = ["grass"],
						Double_Damage_To = ["fire"],
						Half_Damage_From = ["fire"],
						Half_Damage_To = ["grass", "water"],
						None_Damage_From = [],
						None_Damage_To = []
					}
				},
                new Models.Type 
                { 
                    ID = 2, 
                    Name = "fire",
					Damage_Relations = new DamageRelations
					{
						Double_Damage_From = ["water"],
						Double_Damage_To = ["grass"],
						Half_Damage_From = ["grass"],
						Half_Damage_To = ["water", "fire"],
						None_Damage_From = [],
						None_Damage_To = []
					}
				},
                new Models.Type 
                { 
                    ID = 3, 
                    Name = "grass",
					Damage_Relations = new DamageRelations
					{
						Double_Damage_From = ["fire"],
						Double_Damage_To = ["grass"],
						Half_Damage_From = ["grass"],
						Half_Damage_To = ["fire", "grass"],
						None_Damage_From = [],
						None_Damage_To = []
					}
				},
                new Models.Type 
                { 
                    ID = 4, 
                    Name = "light",
					Damage_Relations = new DamageRelations
					{
						Double_Damage_From = ["dark"],
						Double_Damage_To = ["dark"],
						Half_Damage_From = [],
						Half_Damage_To = [],
						None_Damage_From = [],
						None_Damage_To = []
					}
				},
                new Models.Type 
                { 
                    ID = 5, 
                    Name = "dark",
					Damage_Relations = new DamageRelations
					{
						Double_Damage_From = ["light"],
						Double_Damage_To = ["light"],
						Half_Damage_From = [],
						Half_Damage_To = [],
						None_Damage_From = [],
						None_Damage_To = []
					}
				}
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
