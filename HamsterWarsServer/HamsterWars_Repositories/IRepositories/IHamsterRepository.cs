using HamsterWars_DataAccess.Models;


namespace HamsterWars_Repositories.IRepositories 
{
    public interface IHamsterRepository 
    {
        List<Hamster> Hamsters { get; set; }
        List<PercentModel> Percents { get; set; }
        Task LoadHamsters();
        Task<Hamster> GetOneHamster(int id);
        Task AddHamster(Hamster hamster);
        Task DeleteHamster(int id);
        Task LoadTopFive();
    }
}
