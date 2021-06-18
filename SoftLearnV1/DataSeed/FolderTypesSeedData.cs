using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class FolderTypesSeedData
    {
        public static void SeedFolderTypes(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FolderTypes>().HasData(

                new FolderTypes { Id = 1, AppId = 1, FolderName = "CourseCategoryImages" },
                new FolderTypes { Id = 2, AppId = 1, FolderName = "CourseImages" },
                new FolderTypes { Id = 3, AppId = 1, FolderName = "Documents" },
                new FolderTypes { Id = 4, AppId = 1, FolderName = "ProfilePictures" },
                new FolderTypes { Id = 5, AppId = 1, FolderName = "Videos" },

                new FolderTypes { Id = 6, AppId = 2, FolderName = "Assignments" },
                new FolderTypes { Id = 7, AppId = 2, FolderName = "LessonNotes" },
                new FolderTypes { Id = 8, AppId = 2, FolderName = "SchoolLogos" },
                new FolderTypes { Id = 9, AppId = 2, FolderName = "Signatures" },
                new FolderTypes { Id = 10, AppId = 2, FolderName = "StudentPassports"},
                new FolderTypes { Id = 11, AppId = 2, FolderName = "SubjectNotes" }

            );
        }
    }
}
