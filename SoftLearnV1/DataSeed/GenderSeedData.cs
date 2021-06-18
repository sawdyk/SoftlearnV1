using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class GenderSeedData
    {
        public static void SeedGenderTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender
                {
                    Id = 1,
                    GenderName = "Male"
                },
                new Gender
                {
                    Id = 2,
                    GenderName = "Female"
                }
            );
        }
    }
}
