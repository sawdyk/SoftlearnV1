using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SchoolRolesSeedData
    {
        public static void SeedSchoolRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolRoles>().HasData(
                new SchoolRoles
                {
                    Id = 1,
                    RoleName = "Super Administrator"
                },
                new SchoolRoles
                {
                     Id = 2,
                     RoleName = "Administrator"
                },
                new SchoolRoles
                {
                    Id = 3,
                    RoleName = "Class Teacher"
                },
                new SchoolRoles
                {
                    Id = 4,
                    RoleName = "Subject Teacher"
                },
                new SchoolRoles
                {
                    Id = 5,
                    RoleName = "Finance Officer"
                },
                new SchoolRoles
                {
                    Id = 6,
                    RoleName = "Principal"
                },
                new SchoolRoles
                {
                    Id = 7,
                    RoleName = "Vice Principal"
                }
            );
        }
    }
}
