using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SystemRolesSeedData
    {
        public static void SeedSystemRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemRoles>().HasData(
                new SystemRoles
                {
                    Id = 1,
                    RoleName = "Super Administrator"
                },
                new SystemRoles
                {
                    Id = 2,
                    RoleName = "Administrator"
                },
                new SystemRoles
                {
                    Id = 3,
                    RoleName = "Content Creator"
                }
            );
        }
    }

}
