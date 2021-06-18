using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class StatusSeedData
    {
        public static void SeedStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    Id = 1,
                    StatusName = "Approved"
                },
                new Status
                {
                    Id = 2,
                    StatusName = "Pending"
                },
                new Status
                {
                    Id = 3,
                    StatusName = "Declined"
                }
            );
        }
    }
}
