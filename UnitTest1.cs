using ism_console;

namespace ism_teszt
{
    public class UnitTest1
    {
        [Fact]
        public void CreateUser_ShouldAddUserToList()
        {
            // Arrange
            var users = new List<User>();
            var service = new UserService(users);

            // Act
            var user = service.CreateUser("Teszt Elek", "jelszo123", "teszt@example.com", DateTime.Now, 3);

            // Assert
            Assert.Single(users);
            Assert.Equal("Teszt Elek", user.Name);
            Assert.Equal("jelszo123", user.Password);
            Assert.Equal("teszt@example.com", user.Email);
            Assert.Equal(3, user.Level);
        }

        [Fact]
        public void GetUserByIndex_ShouldReturnCorrectUser()
        {
            // Arrange
            var user = new User(1, "Anna", "pw", "anna@example.com", DateTime.Now, 2);
            var users = new List<User> { user };
            var service = new UserService(users);

            // Act
            var result = service.GetUserByIndex(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Anna", result.Name);
        }

        [Fact]
        public void UpdateUserName_ShouldUpdateName_WhenUserExists()
        {
            // Arrange
            var user = new User(1, "Régi Név", "pw", "email@example.com", DateTime.Now, 1);
            var users = new List<User> { user };
            var service = new UserService(users);

            // Act
            bool updated = service.UpdateUserName(1, "Új Név");

            // Assert
            Assert.True(updated);
            Assert.Equal("Új Név", user.Name);
        }

        [Fact]
        public void UpdateUserName_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var users = new List<User>();
            var service = new UserService(users);

            // Act
            bool updated = service.UpdateUserName(99, "Valaki");

            //Assert
            Assert.False(updated);
        }

        [Fact]
        public void DeleteUserById_ShouldRemoveUser_WhenExists()
        {
            // Arrange
            var user = new User(1, "Törlendõ", "pw", "email@example.com", DateTime.Now, 1);
            var users = new List<User> { user };
            var service = new UserService(users);

            // Act
            bool deleted = service.DeleteUserById(1);

            // Assert
            Assert.True(deleted);
            Assert.Empty(users);
        }

        [Fact]
        public void DeleteUserById_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var users = new List<User>();
            var service = new UserService(users);

            // Act
            bool deleted = service.DeleteUserById(42);

            // Assert
            Assert.False(deleted);
        }

        [Fact]
        public void ParseFromCsv_ShouldReturnValidUser()
        {
            // Arrange
            string csv = "1;Béla;pw123;bela@example.com;2023-01-01 12:00:00;4";
            
            // Act
            var user = UserService.ParseFromCsv(csv, ';');

            // Assert
            Assert.Equal(1, user.Id);
            Assert.Equal("Béla", user.Name);
            Assert.Equal("pw123", user.Password);
            Assert.Equal("bela@example.com", user.Email);
            Assert.Equal(4, user.Level);
        }

        [Fact]
        public void ParseFromCsv_ShouldThrowFormatException_WhenWrongFieldCount()
        {
            // Arrange
            string csv = "1;Béla;pw123;bela@example.com;2023-01-01 12:00:00";

            // Act & Assert
            Assert.Throws<FormatException>(() => UserService.ParseFromCsv(csv, ';'));
        }

        [Fact]
        public void ParseFromCsv_ShouldThrowArgumentException_WhenInvalidEmail()
        {
            // Arrange
            string csv = "1;Béla;pw123;hibasemail;2023-01-01 12:00:00;4";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => UserService.ParseFromCsv(csv, ';'));
        }

        [Fact]
        public void LoadUsersFromCsv_ShouldHandleMissingFileGracefully()
        {
            var users = new List<User>();
            var service = new UserService(users);

            string nonExistentPath = Path.Combine(Path.GetTempPath(), "missing_file.csv");

            service.LoadUsersFromCsv(nonExistentPath, ';');

            Assert.Empty(service.GetAllUsers());
        }

        [Fact]
        public void ParseFromCsv_ShouldThrowArgumentException_WhenRegistrationDateIsInFuture()
        {
            string futureDate = DateTime.Now.AddDays(10).ToString("yyyy-MM-dd HH:mm:ss");
            string csv = $"1;Name;pw;email@example.com;{futureDate};3";

            Assert.Throws<ArgumentException>(() =>
            {
                UserService.ParseFromCsv(csv, ';');
            });
        }
    }
}