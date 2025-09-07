using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ism_console
{

    public class User
    {
        // osztályszintű mező
        public static int UserCount = 0;

        // példányszintű mezők
        private string name;
        private string password;
        private string email;
        private int level;
        private DateTime registrationDate;

        // konstruktor
        public User(int id, string name, string password, string email, DateTime registrationDate, int level)
        {
            Id = id;
            Name = name;
            Password = password;
            Email = email;
            this.level = level;
            RegistrationDate = registrationDate;

            UserCount++;
        }

        public int Id { get; set; }

        // tulajdonságok (ellenőrzéssel)
        public string Name
        {
            get => name;
            //set => name = string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A név nem lehet üres vagy szóköz.");
                name = value;
            }
        }


        public string Password
        {
            get => password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A jelszó mező nem lehet üres.");
                password = value;
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (!value.Contains("@"))
                    throw new ArgumentException("Érvénytelen e-mail cím.");
                email = value;
            }
        }

        /*public DateTime RegistrationDate
        {
            get => registrationDate;
        }*/
        //public DateTime RegistrationDate => registrationDate;

        public DateTime RegistrationDate
        {
            get => registrationDate;
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("A regisztráció dátuma nem lehet a jövőben.");
                }

                if (value < new DateTime(2000, 1, 1))
                {
                    throw new ArgumentException("A regisztráció dátuma nem lehet 2000 előtt.");
                }

                registrationDate = value;
            }

        }



        public int Level
        {
            get => level;
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentException("A szerepkör szintjének 0 és 10 között kell lennie.");
                level = value;
            }
        }


        // ToString felüldefiniálás
        public override string ToString()
        {
            return $"{Id};{Name};{Password};{Email};{RegistrationDate:yyyy-MM-dd HH:mm:ss};{Level}";
            /*return $"User:\n" +
                   $"- Id: {Id}\n" +
                   $"- Név: {Name}\n" +
                   $"- Email: {Email}\n" +
                   $"- Registered: {RegistrationDate:yyyy-MM-dd HH:mm}\n" +
                   $"- Level: {Level}\n" /*+
                   $"- Összes felhasználó: {UserCount}";*/
        }
    }
}
