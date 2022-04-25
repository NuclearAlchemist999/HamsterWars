using HamsterWars_DataAccess.Models;


namespace HamsterWars_Repositories.IRepositories 
{
    public interface IHamsterRepository 
    {
        List<Hamster> Hamsters { get; set; }
        Task LoadHamsters();
        Task<Hamster> GetOneHamster(int id);
        Task AddHamster(Hamster hamster);
        Task UpdateHamster(Hamster hamster, int id);
        Task DeleteHamster(int id);
    }
}
