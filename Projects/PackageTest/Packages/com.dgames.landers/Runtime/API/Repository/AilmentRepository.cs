using dgames.http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Landers.API
{
    public class AilmentRepository : BaseRepository<Ailment>
    {
        public static AilmentRepository Instance { get; set; }

        public override async Task<bool> Initialize()
        {
            string request = Path.Combine(LanderConst.Url, "api/v1/ailment");
            AsyncOperationWeb<Ailment[]> asyncOp = WebService.AsyncRequestJson<Ailment[]>(request);

            await asyncOp.AwaitCompletion();

            if (!asyncOp.IsError)
            {
                modelList = asyncOp.Result;
                return true;
            }

            return false;
        }

        #region Internal methods

        /// <summary>
        /// Retrieves an Ailment by its name.
        /// </summary>
        /// <param name="name">The name of the ailment.</param>
        /// <returns>An <see cref="Ailment"/> object corresponding to the given name.</returns>
        public Ailment GetByName(string name) => modelList.FirstOrDefault(x => x.name == name);

        /// <summary>
        /// Retrieves an Ailment by its ID.
        /// </summary>
        /// <param name="id">The ID of the ailment.</param>
        /// <returns>An <see cref="Ailment"/> object corresponding to the given ID.</returns>
        public Ailment GetById(int id) => modelList.FirstOrDefault(x => x.id == id);

        /// <summary>
        /// Retrieves the ID of an Ailment by its name.
        /// </summary>
        /// <param name="name">The name of the ailment.</param>
        /// <returns>The ID of the Ailment.</returns>
        public int GetIdByName(string name) => modelList.FirstOrDefault(x => x.name == name)?.id ?? -1;

        /// <summary>
        /// Retrieves the name of an Ailment by its ID.
        /// </summary>
        /// <param name="id">The ID of the ailment.</param>
        /// <returns>The name of the Ailment.</returns>
        public string GetNameById(int id) => modelList.FirstOrDefault(x => x.id == id)?.name;

        /// <summary>
        /// Checks if an Ailment exists in the repository by its name.
        /// </summary>
        /// <param name="name">The name of the ailment.</param>
        /// <returns><c>true</c> if the ailment exists; otherwise, <c>false</c>.</returns>
        public bool IsExist(string name) => modelList.Any(x => x.name.Equals(name, StringComparison.OrdinalIgnoreCase));

        #endregion
    }
}
