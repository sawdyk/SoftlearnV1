using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class ReportCardCommentConfigSeedData
    {
        public static void SeedReportCardConfig(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportCardCommentConfig>().HasData(
                new ReportCardCommentConfig
                {
                    Id = 1,
                    CommentBy = "Examiner"
                },
                new ReportCardCommentConfig
                {
                    Id = 2,
                    CommentBy = "Class Teacher"
                },
                new ReportCardCommentConfig
                {
                    Id = 3,
                    CommentBy = "Head Teacher"
                },
                 new ReportCardCommentConfig
                 {
                     Id = 4,
                     CommentBy = "Principal"
                 }
            );
        }
    }
}
