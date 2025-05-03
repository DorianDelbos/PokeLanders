using dgames.http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landers.API
{
    public class TypeRepository : BaseRepository<Type>
    {
        public static TypeRepository Instance { get; set; }
        private Dictionary<(string, string), float> typeEfficiency = null;

        public override async Task<bool> Initialize()
        {
            string request = Path.Combine(LanderConst.Url, "api/v1/type");
            AsyncOperationWeb<Type[]> asyncOp = WebService.AsyncRequestJson<Type[]>(request);

            await asyncOp.AwaitCompletion();

            if (!asyncOp.IsError)
            {
                modelList = asyncOp.Result;
                InitializeEfficiencyDictionary();
                return true;
            }

            return false;
        }

        private void InitializeEfficiencyDictionary()
        {
            typeEfficiency = new Dictionary<(string, string), float>();

            foreach (var type in modelList)
            {
                string typeFrom = type.name;

                foreach (var typeTo in type.damage_relations.double_damage_to)
                    typeEfficiency.TryAdd((typeFrom.ToLower(), typeTo.ToLower()), 2.0f);

                foreach (var typeTo in type.damage_relations.half_damage_to)
                    typeEfficiency.TryAdd((typeFrom.ToLower(), typeTo.ToLower()), 0.5f);
            }
        }

        #region Internal methods

        public Type GetByName(string name) => modelList.First(x => x.name == name);
        public Type GetById(int id) => modelList.First(x => x.id == id);
        public int GetIdByName(string name) => modelList.First(x => x.name == name).id;
        public string GetNameById(int id) => modelList.First(x => x.id == id).name;
        public bool IsExist(string name) => modelList.Any(x => x.name.Equals(name));
        public float GetEfficiency(string attackerType, string defenderType)
        {
            attackerType = attackerType.ToLower();
            defenderType = defenderType.ToLower();

            if (!IsExist(attackerType))
                throw new Exception($"{attackerType} is not an existing type !");

            if (!IsExist(defenderType))
                throw new Exception($"{defenderType} is not an existing type !");

            if (typeEfficiency.TryGetValue((attackerType, defenderType), out float efficiency))
                return efficiency;

            return 1.0f; // Default value
        }

        #endregion
    }
}
