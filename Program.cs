using System;
using System.Collections.Generic;
using System.IO;

namespace ism_console
{
    class Program
    {
        // UI metódusok: View 
        
        public static List<User> users = new List<User>();
        public static UserService userService = new UserService(users);


        public static string path = Config.CsvFullPath;
        public static char separator = Config.CsvSeparator;

        

        static void CreateUser(UserService service)
        {
            try
            {
                Console.Write("Név: ");
                string name = Console.ReadLine();

                Console.Write("Jelszó: ");
                string password = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Regisztráció dátuma (yyyy-MM-dd): ");
                DateTime regDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Hozzáférési szint (1-5): ");
                int level = int.Parse(Console.ReadLine());

                User newUser = service.CreateUser(name, password, email, DateTime.Now, level);
                Console.WriteLine("Létrehozva: " + newUser);
            }
            catch (Exception ex)    
            {
                Console.WriteLine("Hiba: " + ex.Message);
            }
        }

        static void ReadUser(UserService service)
        {
            Console.Write("Kérem az id-t: ");
            int id= int.Parse(Console.ReadLine());

            var user = service.GetUserByIndex(id);
            Console.WriteLine(user != null ? user.ToString() : "Nincs ilyen indexű user.");
        }

        static void UpdateUser(UserService service)
        {
            Console.Write("Módosítandó user Id-ja: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Új név: ");
            string newName = Console.ReadLine();

            bool updated = service.UpdateUserName(id, newName);
            Console.WriteLine(updated ? "Név frissítve." : "Nincs ilyen id-jű user.");
        }


        static void DeleteUser(UserService service)
        {
            Console.Write("Törlendő user Id-ja: ");
            int id = int.Parse(Console.ReadLine());
            
            bool deleted = service.DeleteUserById(id);
            Console.WriteLine(deleted ? "User törölve." : "Nincs ilyen id-jű user.");
        }


        static void ListAllUsers(UserService service)
        {
            var users = service.GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("Nincs egyetlen felhasználó sem.");
                return;
            }

            Console.WriteLine("Id;Name;Password;Email;RegistrationDate;AccessLevel");
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }

        static void SaveAllUsers(UserService service)
        {
            try
            {
                service.SaveUsersToCsv(path, separator);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }






        static void ShowMenu()
        {
            Console.WriteLine("\n--- USER MENÜ ---");
            Console.WriteLine("1: Új felhasználó létrehozása");
            Console.WriteLine("2: Felhasználó keresése index alapján");
            Console.WriteLine("3: Felhasználó nevének frissítése index alapján");
            Console.WriteLine("4: Felhasználó törlése ID alapján");
            Console.WriteLine("5: Felhasználók listázása");
            Console.WriteLine("6: Felhasználók fájlbaírása");
            Console.WriteLine("0: Kilépés");
        }


        static void Main()
        {

            Console.WriteLine("Üdv ebben az OOP programban!");

            // egy user felvétele
            /*string csv = "1;Zsolt;secure123;zsolt@example.com;2020-01-01;5";
            User user = User.ParseFromCsv(csv, separator);
            users.Add(user);*/

            // user-ek beolvasása
            userService.LoadUsersFromCsv(path, separator);
           

            while (true)
            {
                ShowMenu();
                Console.Write("Választás: ");
                //string choice = Console.ReadLine();
                string choice = Console.ReadKey().KeyChar.ToString();

                switch (choice)
                {
                    case "1": CreateUser(userService); break;
                    case "2": ReadUser(userService); break;
                    case "3": UpdateUser(userService); break;
                    case "4": DeleteUser(userService); break;
                    case "5": ListAllUsers(userService); break;
                    case "6": SaveAllUsers(userService); break;
                    case "0": return;
                    default: Console.WriteLine("Érvénytelen választás."); break;
                }
            }
        }
    }
}


