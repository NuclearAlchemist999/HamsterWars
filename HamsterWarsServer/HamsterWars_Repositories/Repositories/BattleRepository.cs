﻿using HamsterWars_DataAccess.Data;
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

        public async Task AddFighter(int hamsterId, int gameId)
        {
            var fighter = new HamsterGame
            {
                HamsterId = hamsterId,
                GameId = gameId
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
                                  ImgName = h.ImgName


                              }).ToListAsync();




        }
    }
}
