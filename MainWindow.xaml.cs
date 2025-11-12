using System.ComponentModel;
using System.IO;
using System.Windows;

namespace PacientApp1
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            MainFrame.Navigate(new LoginPage());

            UpdateSystemInfo();

            MainFrame.Navigated += MainFrame_Navigated;
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            UpdateSystemInfo();
        }

        private void UpdateSystemInfo()
        {
            try
            {
                int userCount = Directory.GetFiles(".", "D_*.json").Length;
                int patientCount = Directory.GetFiles(".", "P_*.json").Length;
                SystemInfoText.Text = $"Пользователи: {userCount} | Пациенты: {patientCount}";

                if (App.CurrentDoctor != null)
                {
                    StatusText.Text = $"Врач: {App.CurrentDoctor.LastName} {App.CurrentDoctor.Name}";
                }
                else
                {
                    StatusText.Text = "Готов к работе";
                }
            }
            catch (Exception ex)
            {
                SystemInfoText.Text = "Ошибка загрузки данных";
                StatusText.Text = "Ошибка";
            }
        }

        private void ChangeThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ToggleTheme();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Окно приема пациентов v1.0\nРазработано для управления медицинскими записями.",
                          "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}