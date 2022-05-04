using HamsterWars_DataAccess.Data;
using HamsterWars_DataAccess.Models;
using HamsterWars_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace HamsterWars_Repositories.Repositories 
{
    public class HamsterRepository : IHamsterRepository
    {
        private readonly DataContext _context;
        public HamsterRepository(DataContext context)
        {
            _context = context;
            
        }

        public List<Hamster> Hamsters { get; set; } = new List<Hamster>();
        public List<PercentModel> PercentWin { get; set; } = new List<PercentModel>();
        public List<PercentModel> PercentLoss { get; set; } = new List<PercentModel>();

        public async Task AddHamster(Hamster hamster)
        {
            _context.Add(hamster);
            await _context.SaveChangesAsync();
            
        }

        public async Task<Hamster> GetOneHamster(int id)
        {
            var hamster = await _context.Hamsters.FindAsync(id);
            if (hamster == null)
            {
                throw new Exception("No hamster here!");
            }
            return hamster;
        }

        public async Task LoadHamsters()
        {
            try
            {
                Hamsters = await _context.Hamsters.ToListAsync();
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        public async Task LoadTopFive()
        {
            PercentWin = await (from h in _context.Hamsters
                              .Where(w => w.Wins >= 3)
                              .OrderByDescending(h => ((double)h.Wins / (double)h.Games))
                              .ThenByDescending(h => h.Wins)
                              select new PercentModel   
                              {
                                  WinPercentRate = Math.Round(((double)h.Wins / (double)h.Games) * 100, 2),
                                  Name = h.Name,
                                  ImgName = h.ImgName

                              }).Take(5).ToListAsync();

        
        
        }

        public async Task LoadBottomFive()
        {
           PercentLoss = await (from h in _context.Hamsters
                   .Where(l => l.Losses >= 3)
                   .OrderByDescending(h => ((double)h.Losses / (double)h.Games))
                   .ThenByDescending(h => h.Losses)
                   select new PercentModel
                   {
                       LossPercentRate = Math.Round(((double)h.Losses / (double)h.Games) * 100, 2),
                       Name = h.Name,
                       ImgName = h.ImgName

                   }).Take(5).ToListAsync();
        }

        public async Task DeleteHamster(int id)
        {
            var join = await (from h in _context.Hamsters

                              join hg in _context.Hamsters_Games on h.Id equals hg.HamsterId
                              join g in _context.Games on hg.GameId equals g.Id
                              where h.Id == id

                              select new 
                              { 
                                  hamster = h,
                                  game = g

                              }).ToListAsync();

            if (join != null)
            {
                foreach (var item in join)
                {
                    _context.Remove(item.game);
                    _context.Hamsters.Remove(item.hamster);
                }
                await _context.SaveChangesAsync();
                
            }
            var dbHamster = _context.Hamsters.Find(id);

            if (dbHamster != null)
            {
                _context.Hamsters.Remove(dbHamster);
                await _context.SaveChangesAsync();
            }
                
            
        }

    }
}
