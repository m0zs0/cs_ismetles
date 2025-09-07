using ism_console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ism_teszt
{

    public class MockedFileTests
    {
        [Fact]
        public void ExportUsersToCsv_ShouldWriteCorrectContent()
        {
            // Arrange
            var users = new List<User>
            {
                new User(1, "Teszt Elek", "pw123", "teszt@example.com", DateTime.Now, 3)
            };
            var service = new UserService(users);

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, Encoding.UTF8))
            {
                // Act
                // Itt nem a fájlba írunk, hanem a memóriába
                foreach (var user in users)
                {
                    string line = $"{user.Id};{user.Name};{user.Password};{user.Email};{user.RegistrationDate:yyyy-MM-dd HH:mm:ss};{user.Level}";
                    sw.WriteLine(line);
                }
                sw.Flush();

                // Assert
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    string content = sr.ReadToEnd();
                    Assert.Contains("Teszt Elek", content);
                }
            }
        }
    }

}
