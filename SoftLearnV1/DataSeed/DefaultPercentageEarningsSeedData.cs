using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class DefaultPercentageEarningsSeedData
    {
        public static void SeedDefaultPercentageEarnings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DefaultPercentageEarningsPerCourse>().HasData(
                new DefaultPercentageEarningsPerCourse
                {
                    Id = 1,
                    Percentage = 10
                }              
            );
        }
    }
}
