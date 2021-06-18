using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SchoolTypeSeedData
    {
        public static void SeedSchoolTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolType>().HasData(
                new SchoolType
                {
                    Id = 1,
                    SchoolTypeName = "Nursery"
                },
                new SchoolType
                {
                    Id = 2,
                    SchoolTypeName = "Primary"
                },
                new SchoolType
                {
                    Id = 3,
                    SchoolTypeName = "Secondary"
                }
            );
        }
    }
}
