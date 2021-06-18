using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class ClassOrAlumniSeedData
    {
        public static void SeedClassOrAlumni(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassAlumni>().HasData(
                new ClassAlumni
                {
                    Id = 1,
                    Category = "Alumni"
                },
                new ClassAlumni
                {
                    Id = 2,
                    Category = "Class"
                }
            );
        }
    }
}
