using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class ActiveInActiveStatusSeedData
    {
        public static void SeedActiveInActiveStatus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActiveInActiveStatus>().HasData(
                new ActiveInActiveStatus
                {
                    Id = 1,
                    StatusName = "Active"
                },
                new ActiveInActiveStatus
                {
                    Id = 2,
                    StatusName = "InActive"
                }
            );
        }
    }
}
