using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SchoolSubTypesSeedData
    {
        public static void SeedSchoolSubTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolSubTypes>().HasData(
                new SchoolSubTypes
                {
                    Id = 1,
                    SchoolTypeId = 3,
                    SubTypeName = "Junior"
                },
                new SchoolSubTypes
                {
                    Id = 2,
                    SchoolTypeId = 3,
                    SubTypeName = "Senior"
                },
                new SchoolSubTypes
                {
                    Id = 3,
                    SchoolTypeId = 2,
                    SubTypeName = "Primary"
                },
                new SchoolSubTypes
                {
                    Id = 4,
                    SchoolTypeId = 1,
                    SubTypeName = "Nursery"
                }

            );
        }
    }
}
