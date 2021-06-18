using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class CourseCategorySeedData
    {
        public static void SeedCourseCategoryRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseCategory>().HasData(
                new CourseCategory
                {
                    Id = 1,
                    CourseCategoryName = "Design",
                    CategoryImageUrl = "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/angularjs.jpg.jpg",
                },
                new CourseCategory
                {
                    Id = 2,
                    CourseCategoryName = "Networking",
                    CategoryImageUrl = "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/reactjs.jpg.jpg",

                },
                new CourseCategory
                {
                    Id = 3,
                    CourseCategoryName = "Painting",
                    CategoryImageUrl = "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/wordpress.jpg.jpg",

                },
                new CourseCategory
                {
                    Id = 4,
                    CourseCategoryName = "Advertising",
                    CategoryImageUrl = "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/photoshop.jpg.jpg",

                },
                new CourseCategory
                {
                    Id = 5,
                    CourseCategoryName = "Marketing",
                    CategoryImageUrl = "https://res.cloudinary.com/mywebsite/image/upload/v1603445237/softlearn/course_category_images/marketing_ekmzwf.jpg",

                }
            );
        }
    }
}
