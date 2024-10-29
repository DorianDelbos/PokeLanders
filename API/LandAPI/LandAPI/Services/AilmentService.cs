using LandAPI.Data;
using LandAPI.Models;

namespace LandAPI.Services
{
	public class AilmentService
	{
		private readonly AilmentRepository _ailmentRepository;

		public AilmentService(AilmentRepository ailmentRepository)
		{
			_ailmentRepository = ailmentRepository;
		}

		public List<Ailment> GetAllAilements()
			=> _ailmentRepository.Ailment;

		public Ailment GetAilementById(int id)
			=> _ailmentRepository.Ailment.FirstOrDefault(p => p.ID == id);
	}
}
