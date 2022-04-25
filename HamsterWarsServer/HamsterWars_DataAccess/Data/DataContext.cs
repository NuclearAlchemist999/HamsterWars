using HamsterWars_DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterWars_DataAccess.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Hamster> Hamsters { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<HamsterGame> Hamsters_Games { get; set; }

    }
}
