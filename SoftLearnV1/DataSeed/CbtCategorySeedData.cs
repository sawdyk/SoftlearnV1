using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class CbtCategorySeedData
    {
        public static void SeedCbtCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CbtCategory>().HasData(
                new CbtCategory
                {
                    Id = 1,
                    CategoryName = "Exam"
                },
                new CbtCategory
                {
                    Id = 2,
                    CategoryName = "CA"
                }
            );
        }
    }
}
