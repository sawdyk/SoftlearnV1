using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class FacilitatorTypeSeedData
    {
        public static void seedFacilitatorType(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacilitatorType>().HasData(
                new FacilitatorType
                {
                    Id = 1,
                    FacilitatorTypeName = "External"
                },
                new FacilitatorType
                {
                    Id = 2,
                    FacilitatorTypeName = "Internal"
                }
            );
        }
    }
}
