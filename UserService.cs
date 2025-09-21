using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ism_console
{
    public class UserService
    {
        // Controller metódusok
        
        private List<User> users;

        public UserService(List<User> users)
        {
            this.users = users;
        }

        public List<User> GetAllUsers() => users;

        // osztályszintű metódusok

        /// <summary>
        /// Egy CSV sorból létrehoz egy <see cref="User"/> példányt.
        /// A sor formátuma: név, jelszó, email, jogosultsági szint (szám).
        /// A mezőket vessző választja el, és a felesleges szóközök eltávolításra kerülnek.
        /// </summary>
        /// <param name="csvLine">A CSV sor, amely egy felhasználó adatait tartalmazza.</param>
        /// <param name="separator">A CSV sorain belül alkalmazott elválasztó jel.</param>
        /// <returns>A létrehozott <see cref="User"/> példány.</returns>
        /// <exception cref="FormatException">Ha a sor nem tartalmaz pontosan 4 mezőt.</exception>
        /// <exception cref="ArgumentException">Ha valamelyik mező érvénytelen (pl. üres név, hibás email).</exception>
        /// <exception cref="SystemException">Ha a jogosultsági szint nem konvertálható egész számmá.</exception>
        public static User ParseFromCsv(string csvLine, char separator)
        {
            var parts = csvLine.Split(separator);

            if (parts.Length != 6)
                throw new FormatException("A CSV sor nem megfelelő formátumú.");

            int id = int.Parse(parts[0].Trim());
            string name = parts[1].Trim();
            string password = parts[2].Trim();
            string email = parts[3].Trim();
            DateTime registrationDate = DateTime.Parse(parts[4].Trim());
            int level = int.Parse(parts[5].Trim());

            return new User(id, name, password, email, registrationDate, level);
        }


        ///<summary>
        /// Beolvassa a felhasználókat egy CSV fájlból, és visszaadja őket egy listában.
        /// A fájl minden sora egy felhasználót reprezentál: név, jelszó, email, jogosultsági szint.
        /// Kommentelt sorokat (# jellel kezdődő) és üres sorokat kihagy.
        /// Hibás sorokat figyelmeztetéssel kihagy.
        /// </summary>
        /// <param name="path">A CSV fájl elérési útja.</param>
        /// <param name="separator">A CSV sorain belül alkalmazott elválasztó jel.</param>
        /// <exception cref="IOException">Ha a fájl nem olvasható vagy nem létezik.</exception>
        /// <exception cref="FormatException">Ha egy sor nem megfelelő formátumú.</exception>
        public void LoadUsersFromCsv(string path, char separator)
        {
            users.Clear();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Üres vagy kommentelt sor kihagyása
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                            continue;

                        try
                        {
                            User user = ParseFromCsv(line, separator);
                            users.Add(user);
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Hibás sor kihagyva: {line}\nHiba: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"Hibás sor kihagyva: {line}\nHiba: {ex.Message}");
                        }
                    }
                }
            }
            catch (IOException ioEx)
            {
                //Console.WriteLine("Fájlbeolvasási hiba: " + ioEx.Message);
                System.Diagnostics.Debug.WriteLine("Hiba történt a fájl olvasása során: " + ioEx.Message);
            }


        }


        /// <summary>
        /// Kiírja a megadott <see cref="User"/> példányokat egy CSV fájlba.
        /// A fájl minden sora egy felhasználót reprezentál: név, jelszó, email, jogosultsági szint.
        /// A fájl felülíródik, ha már létezik.
        /// </summary>
        /// <param name="path">A CSV fájl elérési útja.</param>
        /// <param name="separator">A CSV sorain belül alkalmazott elválasztó jel.</param>
        /// <exception cref="IOException">Ha a fájl nem írható vagy nem elérhető.</exception>
        public void SaveUsersToCsv(string path, char separator)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    foreach (var user in users)
                    {
                        string line = $"{user.Id}{separator}{user.Name}{separator}{user.Password}{separator}{user.Email}{separator}{user.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss")}{separator}{user.Level}";
                        sw.WriteLine(line);
                        //System.Diagnostics.Debug.WriteLine(line);
                    }
                }
            }
            catch (IOException ex)
            {
                //Console.WriteLine("Hiba történt a fájl írása során: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Hiba történt a fájl írása során: " + ex.Message);
            }
        }




        public User CreateUser(string name, string password, string email, DateTime registrationDate, int level)
        {
            int newId = User.UserCount + 1;
            var user = new User(newId, name, password, email, registrationDate, level);
            users.Add(user);
            return user;
        }

        public User GetUserByIndex(int index)
        {
            return users.FirstOrDefault(u => u.Id == index);
        }

        public bool UpdateUserName(int index, string newName)
        {
            var user = users.FirstOrDefault(u => u.Id == index);
            if (user != null)
            {
                user.Name = newName;
                return true;
            }
            return false;
        }

        public bool DeleteUserById(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                return true;
            }
            return false;
        }


    }
}

