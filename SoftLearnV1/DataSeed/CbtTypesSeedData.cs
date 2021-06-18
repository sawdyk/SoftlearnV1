using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class CbtTypesSeedData
    {
        public static void SeedCbtTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CbtTypes>().HasData(
                new CbtTypes
                {
                    Id = 1,
                    TypeName = "Practice"
                },
                new CbtTypes
                {
                    Id = 2,
                    TypeName = "Schedule"
                }
            );
        }
    }
}
