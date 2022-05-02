using HamsterWars_DataAccess.Data;
using HamsterWars_DataAccess.Models;
using HamsterWars_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace HamsterWars_Repositories.Repositories 
{
    public class BattleRepository : IBattleRepository
    {
        private readonly DataContext _context;
        public BattleRepository(DataContext context)
        {
            _context = context;
        }

        public List<Hamster> Hamsters { get; set; } = new List<Hamster>();
        public List<JoinModel> Fighters { get; set; } = new List<JoinModel>();



        public async Task GetTwoHamsters()
        {
            try
            {
                Hamsters = await _context.Hamsters.OrderBy(h => Guid.NewGuid()).Take(2).ToListAsync();
            }
            catch
            {
                throw new ArgumentException();
            }

        }

        public async Task UpdateWins(int id)
        {
            var dbHamster = await _context.Hamsters.FindAsync(id);

            if (dbHamster == null)
            {
                throw new Exception("No hamster here!");
            }

            dbHamster.Wins++;
            dbHamster.Games++;


            await _context.SaveChangesAsync();

        }

        public async Task UpdateLosses(int id)
        {
            var dbHamster = await _context.Hamsters.FindAsync(id);

            if (dbHamster == null)
            {
                throw new Exception("No hamster here!");
            }

            dbHamster.Losses++;
            dbHamster.Games++;


            await _context.SaveChangesAsync();

        }

        public async Task<int> AddGame()
        {
            var game = new Game
            {

            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game.Id;

        }

        public async Task AddFighter(int hamsterId, int gameId, string winStat)
        {
            var fighter = new HamsterGame
            {
                HamsterId = hamsterId,
                GameId = gameId,
                WinStatus = winStat
            };

            _context.Add(fighter);
            await _context.SaveChangesAsync();
        }

        public async Task GetFighters(int id)
        {
            Fighters = await (from h in _context.Hamsters
                              join hg in _context.Hamsters_Games on h.Id equals hg.HamsterId
                              join g in _context.Games on hg.GameId equals g.Id
                              where g.Id == id
                              select new JoinModel
                              {
                                  HamsterName = h.Name,
                                  Wins = h.Wins,
                                  Losses = h.Losses,
                                  Games = h.Games,
                                  HamsterId = h.Id,
                                  ImgName = h.ImgName,
                                  WinStatus = hg.WinStatus

                              }).ToListAsync();

        }

        public async Task BattleWinner(int id)
        {
            
            var r = (from hh in _context.Hamsters
                     join hgg in _context.Hamsters_Games on hh.Id equals hgg.HamsterId
                     join gg in _context.Games on hgg.GameId equals gg.Id
                     where hgg.HamsterId == id && hgg.WinStatus == "Winner"
                     select gg.Id);

            Fighters = await (from g in _context.Games
            join hg in _context.Hamsters_Games on g.Id equals hg.GameId
            join h in _context.Hamsters on hg.HamsterId equals h.Id
                              
            where r.Contains(g.Id)
            select new JoinModel
            {
                GameId = g.Id,
                HamsterName = h.Name,
                WinStatus = hg.WinStatus
                                  

            }).Distinct().ToListAsync();

        }

        public async Task BattleHistory()
        {
            Fighters = await (from h in _context.Hamsters
                        join hg in _context.Hamsters_Games on h.Id equals hg.HamsterId
                        join g in _context.Games on hg.GameId equals g.Id
                        select new JoinModel
                        {
                            GameId = hg.GameId,
                            HamsterName = h.Name,
                            WinStatus = hg.WinStatus


                        }).ToListAsync();
        }

        public async Task DeleteGame(int id)
        {
            var dbGame = await _context.Games.FindAsync(id);

            if (dbGame != null)
            {
                _context.Games.Remove(dbGame);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteFighters(int id)
        {
            var dbGame = await _context.Hamsters_Games.Where(g => g.GameId == id).ToListAsync();

            if (dbGame != null)
            {
                foreach (var game in dbGame)
                {
                    _context.Hamsters_Games.Remove(game);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
