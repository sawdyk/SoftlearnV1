using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class ScoreStatusSeedData
    {
        public static void SeedScoreStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreStatus>().HasData(
                new ScoreStatus
                {
                    Id = 1,
                    ScoreStatusName = "Passed"
                },
                new ScoreStatus
                {
                    Id = 2,
                    ScoreStatusName = "Failed"
                },
                new ScoreStatus
                {
                    Id = 3,
                    ScoreStatusName = "Pending"
                }
            );
        }
    }
}
