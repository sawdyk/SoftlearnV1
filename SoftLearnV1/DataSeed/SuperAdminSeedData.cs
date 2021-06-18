using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.DataSeed
{
    public static class SuperAdminSeedData
    {
        public static void SeedSuperAdmin(this ModelBuilder modelBuilder)
        {
            // any guid
            const string superAdminId = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            modelBuilder.Entity<SystemUsers>().HasData(

                new SystemUsers
                {
                    Id = new Guid(superAdminId),
                    DateCreated = DateTime.Now,
                    Email = "superadmin@superadmin.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "superadmin@superadmin.com",
                    PasswordHash = HashPassword().passwordHash.Trim(),
                    Salt = HashPassword().salt.Trim(),
                    LastUpdatedDate = DateTime.Now,
                    IsActive = true
                }
            );
            modelBuilder.Entity<SystemUserRoles>().HasData(
                new SystemUserRoles
                {
                    DateCreated = DateTime.Now,
                    Id = 1,
                    RoleId = 1,
                    UserId = new Guid(superAdminId)
                }
            );
        }
        private static PasswordClass HashPassword()
        {
            PasswordClass newObject = new PasswordClass();
            var paswordHasher = new PasswordHasher();
            //the salt
            string salt = paswordHasher.getSalt();
            //Hash the password and salt
            string passwordHash = paswordHasher.hashedPassword("Password", salt);
            newObject.passwordHash = passwordHash;
            newObject.salt = salt;
            
            return newObject;
        }
    }
    public class PasswordClass
    {
        public string salt { get; set; }
        public string passwordHash { get; set; }
    }
}
