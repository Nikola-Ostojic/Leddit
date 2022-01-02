using Backend.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.DAL.Helpers
{
    public static class Seed
    {
        public static void SeedDB(this ModelBuilder builder)
        {
            builder.Entity<UserEntity>(options =>
            {
                options.HasData(
                    new UserEntity
                    {
                        Id = 1,
                        UserName = "User",
                        Email = "user@levi9.com",
                        // Password: User
                        Password = "b512d97e7cbf97c273e4db073bbb547aa65a84589227f8f3d9e4a72b9372a24d",
                        Created = DateTime.Now,
                        Role = Role.User
                    },
                    new UserEntity
                    {
                        Id = 2,
                        UserName = "Admin",
                        Email = "admin@levi9.com",
                        // Password: Admin
                        Password = "c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f",
                        Created = DateTime.Now,
                        Role = Role.Admin
                    }
                    );
            });

            builder.Entity<MovieEntity>(options =>
            {
                options.HasData(
                   new MovieEntity
                   {
                       Id = 1,
                       Name = "The Martian",
                       ImageUrl = "https://gortoncenter.org/wp-content/uploads/2018/05/The-Martian.jpg",
                       Created = DateTime.Now
                   },
                    new MovieEntity
                    {
                        Id = 2,
                        Name = "Kingsman: The Golden Circle",
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjQ3OTgzMzY4NF5BMl5BanBnXkFtZTgwOTc4OTQyMzI@._V1_.jpg",
                        Created = DateTime.Now
                    },
                    new MovieEntity
                    {
                        Id = 3,
                        Name = "Battlestar Galactica Razor",
                        ImageUrl = "https://www.denofgeek.com/wp-content/uploads/2019/09/battlestar-galactica-reboot-1.jpeg",
                        Created = DateTime.Now
                    },
                    new MovieEntity
                    {
                        Id = 4,
                        Name = "The Dictator",
                        ImageUrl = "https://www.un.org/sites/un2.un.org/files/styles/large-article-image-style-16-9/public/field/image/dictator_quad-1024x768.jpg",
                        Created = DateTime.Now
                    },
                    new MovieEntity
                    {
                        Id = 5,
                        Name = "Iron Man",
                        ImageUrl = "https://i.pinimg.com/originals/a8/63/be/a863beaf54137860699352f35e6c5052.jpg",
                        Created = DateTime.Now
                    },
                    new MovieEntity
                    {
                        Id = 6,
                        Name = "Guardians of the Galaxy",
                        ImageUrl = "https://icdn2.digitaltrends.com/image/digitaltrends/guardians-of-the-galaxy-vol-2-2.jpg",
                        Created = DateTime.Now
                    },
                    new MovieEntity
                    {
                        Id = 7,
                        Name = "How to train your dragon",
                        ImageUrl = "https://m.media-amazon.com/images/M/MV5BMjA5NDQyMjc2NF5BMl5BanBnXkFtZTcwMjg5ODcyMw@@._V1_.jpg",
                        Created = DateTime.Now
                    }
                );
            });
        }
    }
}
