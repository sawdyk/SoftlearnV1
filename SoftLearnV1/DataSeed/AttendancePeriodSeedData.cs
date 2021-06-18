using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class AttendancePeriodSeedData
    {
        public static void SeedAttendancePeriod(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendancePeriod>().HasData(
                new AttendancePeriod
                {
                    Id = 1,
                    AttendancePeriodName = "Morning"
                },
                new AttendancePeriod
                {
                    Id = 2,
                    AttendancePeriodName = "Afternoon"
                },
                new AttendancePeriod
                {
                    Id = 3,
                    AttendancePeriodName = "Both"
                }
            );
        }
    }
}
