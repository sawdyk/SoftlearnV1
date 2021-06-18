using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class CourseTypeSeedData
    {
        public static void seedCourseType(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseType>().HasData(
                new CourseType
                {
                    Id = 1,
                    CourseTypeName = "Paid"
                },
                new CourseType
                {
                    Id = 2,
                    CourseTypeName = "Free"
                }
            );
        }
    }
}
