using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class CourseLevelTypesSeedData
    {
        public static void SeedCourseLevelTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseLevelTypes>().HasData(
                new CourseLevelTypes
                {
                    Id = 1,
                    LevelTypeName = "Primary/Elementary"
                },
                new CourseLevelTypes
                {
                    Id = 2,
                    LevelTypeName = "Secondary/Higher"
                },
                new CourseLevelTypes
                {
                    Id = 3,
                    LevelTypeName = "University/Adult"
                },
                new CourseLevelTypes
                {
                    Id = 4,
                    LevelTypeName = "All"
                }
            );
        }
    }
}
