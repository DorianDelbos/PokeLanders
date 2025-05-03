namespace LandAPI.API
{
    public class NatureService
    {
        private readonly NatureRepository _natureRepository;

        public NatureService(NatureRepository natureRepository)
        {
            _natureRepository = natureRepository;
        }

        public List<Nature> GetAllNatures()
            => _natureRepository.Nature;

        public Nature? GetNatureById(int id)
            => _natureRepository.Nature.FirstOrDefault(p => p.ID == id);
    }
}
