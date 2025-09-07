using ism_console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ism_teszt
{
    public class IntegrationTests : IDisposable
    {
        private readonly string tempFilePath;
        private readonly char separator = ';';
        private List<User> users;
        private UserService service;

        public IntegrationTests()
        {
            tempFilePath = Path.Combine(Path.GetTempPath(), $"users_test_{Guid.NewGuid()}.csv");
            users = new List<User>();
            service = new UserService(users);
        }

        [Fact]
        public void CreateUser_IntegrationTest()
        {
            var user = service.CreateUser("Teszt Elek", "pw123", "teszt@example.com", DateTime.Now, 3);
            Assert.Single(users);
            Assert.Equal("Teszt Elek", user.Name);
        }

        [Fact]
        public void UpdateUserName_IntegrationTest()
        {
            var user = service.CreateUser("Régi Név", "pw", "email@example.com", DateTime.Now, 2);
            bool updated = service.UpdateUserName(user.Id, "Új Név");
            Assert.True(updated);
            Assert.Equal("Új Név", user.Name);
        }

        [Fact]
        public void DeleteUserById_IntegrationTest()
        {
            var user = service.CreateUser("Törlendő", "pw", "del@example.com", DateTime.Now, 1);
            bool deleted = service.DeleteUserById(user.Id);
            Assert.True(deleted);
            Assert.Empty(users);
        }

        [Fact]
        public void ExportUsersToCsv_IntegrationTest()
        {
            var user = service.CreateUser("Exportált", "pw", "exp@example.com", DateTime.Now, 4);
            service.SaveUsersToCsv(tempFilePath, separator);
            Assert.True(File.Exists(tempFilePath));
            string[] lines = File.ReadAllLines(tempFilePath);
            Assert.Single(lines);
            Assert.Contains("Exportált", lines[0]);
        }

        [Fact]
        public void LoadUsersFromCsv_IntegrationTest()
        {
            string csvLine = $"1{separator}Beolvasott{separator}pw{separator}read@example.com{separator}{DateTime.Now:yyyy-MM-dd HH:mm:ss}{separator}5";
            File.WriteAllText(tempFilePath, csvLine);
            service.LoadUsersFromCsv(tempFilePath, separator);
            Assert.Single(service.GetAllUsers());
            Assert.Equal("Beolvasott", service.GetAllUsers()[0].Name);
        }

        public void Dispose()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
}
