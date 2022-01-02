using Backend.DAL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;


namespace Backend.Api.Tests.Api
{
    public class ServerFixture : IDisposable
    {
        public TestServer Instance { get; set; }

        public ServerFixture()
        {
            Instance = new TestServer(new WebHostBuilder()
                 .ConfigureServices(services => services.AddSingleton(TestData()))
                .UseStartup<TestStartup>());
        }
        public void Dispose()
        {
            if (Instance != null)
            {
                Instance.Dispose();
            }
        }

        private DataSet TestData()
        {
            var testData = new DataSet
            {
                Comments = new List<CommentEntity>(),
                Threads = new List<ThreadEntity>(),
                Users = new List<UserEntity>(),
                Movies = new List<MovieEntity>()
            };

            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxNmRhMGVjOC1jMGM3LTRmZGItYTQ0Yi1jYjE1OWE3Y2E4ZTciLCJpYXQiOiIxMC83LzIwMjAgMzozNzoxMyBQTSIsInN1YiI6InVzZXIxQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlcjEiLCJleHAiOjE2MDIwODg2MzMsImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiVXNlciJdfQ.Pocnt0ZXmfnVPx2f7zIm0OAYXiDIO7w8N-w_bKP6Kl0
            var user1 = new UserEntity { Email = "user@levi9.com", UserName = "User", Password = "b512d97e7cbf97c273e4db073bbb547aa65a84589227f8f3d9e4a72b9372a24d", Role = Role.User };

            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI0NmZjYmYzMi03ZjljLTQwODUtYTM1NC1mNWM5MWQ3NDdlM2YiLCJpYXQiOiIxMC83LzIwMjAgMzo1MzozOSBQTSIsInN1YiI6ImFkbWluQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiQWRtaW4iLCJleHAiOjE2MDk2NDk2MTksImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiQWRtaW4iXX0.PEwTtquuArmWVhNfpWCUOkx4xKxwPycEB5bgWvgr7Kg
            var user2 = new UserEntity { Email = "admin@levi9.com", UserName = "Admin", Password = "c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f", Role = Role.Admin };

            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIwOTg1M2QxYy1lMzE4LTRjZTgtODA4My01ODUyMTM4OTUxYWQiLCJpYXQiOiIxMC83LzIwMjAgMzo0Njo1MSBQTSIsInN1YiI6InVzZXIxQGxldmk5LmNvbSIsInVzZXJuYW1lIjoiVXNlcjEiLCJleHAiOjE2MDk4NjUyMTEsImlzcyI6Ikxldmk5IEJhY2tlbmQiLCJhdWQiOiJsZXZpOVVzZXJzIiwicm9sZXMiOlsiVXNlciJdfQ.vMg4G7sYIq1jgYUYc9ekWhDfJxEX2XlALCHcLLvGwJA
            var user3 = new UserEntity { Email = "user1@levi9.com", UserName = "User1", Password = "27a534a25cf745b6c985eb782079a6fe8641b00003dada14f392a2d01b9c790a", Role = Role.User };

            //Threads
            testData.Threads.Add(new ThreadEntity { Id = 1, Title = "Title 1", Content = "Content 1", Author = user1 });
            testData.Threads.Add(new ThreadEntity { Id = 2, Title = "Title 2", Content = "Content 2", Author = user1 });
            testData.Threads.Add(new ThreadEntity { Id = 3, Title = "Title 3", Content = "Content 3", Author = user1 });
            testData.Threads.Add(new ThreadEntity { Id = 4, Title = "Title 4", Content = "Content 4", Author = user1 });
            testData.Threads.Add(new ThreadEntity { Id = 5, Title = "Title 5", Content = "Content 5", Author = user1 });

            //Comments  
            testData.Comments.Add(new CommentEntity { Id = 1, Content = "Content 1", Author = user1 });
            testData.Comments.Add(new CommentEntity { Id = 2, Content = "Content 2", Author = user1 });
            testData.Comments.Add(new CommentEntity { Id = 3, Content = "Content 3", Author = user1 });
            testData.Comments.Add(new CommentEntity { Id = 4, Content = "Content 4", Author = user1 });
            testData.Comments.Add(new CommentEntity { Id = 5, Content = "Content 5", Author = user1 });


            // Users
            testData.Users.Add(user1);
            testData.Users.Add(user2);
            testData.Users.Add(user3);

            testData.Movies.Add(new MovieEntity { Name = "Movie One", ImageUrl = "Movie One Image" });
            testData.Movies.Add(new MovieEntity { Name = "Movie Two", ImageUrl = "Movie Two Image" });
            return testData;
        }
    }
}
