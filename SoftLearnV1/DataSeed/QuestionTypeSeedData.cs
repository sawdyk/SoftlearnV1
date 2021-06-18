using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class QuestionTypeSeedData
    {
        public static void SeedQuestionTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuestionTypes>().HasData(
                new QuestionTypes
                {
                    Id = 1,
                    QuestionTypeName = "Multiple Choice"
                },
               new QuestionTypes
               {
                   Id = 2,
                   QuestionTypeName = "Fill in the Gap"
               },
               new QuestionTypes
               {
                   Id = 3,
                   QuestionTypeName = "True or False"
               }
            );
        }
    }
}
