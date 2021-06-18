using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class AppTypesSeedData
    {
        public static void SeedAppTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTypes>().HasData(
                new AppTypes
                {
                    Id = 1,
                    AppName = "CourseApp"
                },
                new AppTypes
                {
                    Id = 2,
                    AppName = "SchoolApp"
                }
            );
        }
    }
}
