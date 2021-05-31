using Microsoft.EntityFrameworkCore;
using MVCSample.Models;
using System.Collections.Generic;
using System.Linq;

namespace MVCSample.Context
{
    public class MVCContext : DbContext
    {
        public DbSet<FundModel> FundModels { get; set; }

        public MVCContext(DbContextOptions options) : base(options)
        {
        }

    }
}
