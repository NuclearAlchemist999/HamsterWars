using HamsterWars_DataAccess.Models;


namespace HamsterWars_Repositories.IRepositories
{
    public interface IBattleRepository
    {
        List<Hamster> Hamsters { get; set; }
        List<JoinModel> Fighters { get; set; }
        Task GetTwoHamsters();
        Task UpdateWins(int id);
        Task UpdateLosses(int id);
        Task<int> AddGame();
        Task AddFighter(int hamsterId, int gameId);
        Task GetFighters(int id);
    }
}
