namespace Landers.API
{
    [System.Serializable]
    public class DamageRelations
    {
        public string[] double_damage_from;
        public string[] double_damage_to;
        public string[] half_damage_from;
        public string[] half_damage_to;
        public string[] none_damage_from;
        public string[] none_damage_to;
    }

    [System.Serializable]
    public class Type : IBaseModel
    {
        public int id;
        public string name;
        public string color;
        public DamageRelations damage_relations;
        public string sprite;
    }
}
