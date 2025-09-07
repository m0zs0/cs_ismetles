using ism_console;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ism_wpf
{

    public partial class MainWindow : Window
    {
        private ObservableCollection<User> users;
        private UserService userService;

        public MainWindow()
        {
            InitializeComponent();
            users = new ObservableCollection<User>();
            userService = new UserService(users.ToList());
            dgUsers.ItemsSource = users;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var newUser = userService.CreateUser("Új Név", "pw", "email@example.com", DateTime.Now, 1);
            users.Add(newUser);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (dgUsers.SelectedItem is User selectedUser)
            {
                var result = MessageBox.Show(
                    $"Biztosan törölni szeretnéd a(z) {selectedUser.Name} felhasználót?",
                    "Törlés megerősítése",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    if (userService.DeleteUserById(selectedUser.Id))
                    {
                        users.Remove(selectedUser);
                    }
                }
            }

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string path = Config.CsvFullPath;
            char separator = Config.CsvSeparator;
            userService.SaveUsersToCsv(path, separator);
            MessageBox.Show("Mentés sikeres.");
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string path = Config.CsvFullPath;
            char separator = Config.CsvSeparator;
            users.Clear();
            userService.LoadUsersFromCsv(path, separator);
            foreach (var user in userService.GetAllUsers())
            {
                users.Add(user);
            }
        }
    }

}