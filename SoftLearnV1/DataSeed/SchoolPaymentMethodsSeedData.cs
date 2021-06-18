using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SchoolPaymentMethodsSeedData
    {
        public static void SeedSchoolPaymentMethods(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolPaymentMethods>().HasData(
                new SchoolPaymentMethods
                {
                    Id = 1,
                    MethodName = "Bank Deposit"
                },
                new SchoolPaymentMethods
                {
                    Id = 2,
                    MethodName = "Online Transfer"
                },
                new SchoolPaymentMethods
                {
                    Id = 3,
                    MethodName = "Card Payment"
                }
            );
        }
    }
}
