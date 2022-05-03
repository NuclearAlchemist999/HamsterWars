﻿using HamsterWars_DataAccess.Data;
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
        public List<PercentModel> Percents { get; set; } = new List<PercentModel>();

        public async Task AddHamster(Hamster hamster)
        {
            _context.Add(hamster);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteHamster(int id)
        {
            var dbHamster = await _context.Hamsters.FindAsync(id);

            if (dbHamster == null)
            {
                throw new Exception("No hamster here!");
            }
            _context.Hamsters.Remove(dbHamster);
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
            Percents = await (from h in _context.Hamsters
                              .Where(g => g.Wins >= 5)
                              .OrderByDescending(h => ((double)h.Wins / (double)h.Games))
                              select new PercentModel   
                              {
                                  WinPercentRate = Math.Round(((double)h.Wins / (double)h.Games) * 100, 2),
                                  Name = h.Name,
                                  ImgName = h.ImgName
                              })
                             .Take(5).ToListAsync();

        
        
        }

    }
}
