using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class ScoreCategorySeedData
    {
        public static void seedScoreCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScoreCategory>().HasData(
                new ScoreCategory
                {
                    Id = 1,
                    CategoryName = "Exam"
                },
                new ScoreCategory
                {
                    Id = 2,
                    CategoryName = "CA"
                },
                new ScoreCategory
                {
                    Id = 3,
                    CategoryName = "Behavioural"
                },
                new ScoreCategory
                {
                    Id = 4,
                    CategoryName = "Extra Curricular"
                }
            );
        }
    }
}
