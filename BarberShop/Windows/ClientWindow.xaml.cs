using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static BarberShop.Classes.AppData;

namespace BarberShop.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        List<EF.Client> listClient = new List<EF.Client>();
        List<string> ListSort = new List<string>()
        {
            "По умолчанию",
            "По фамилии",
            "По имени",
            "По телефону",
            "По специализации"
        };
        public ClientWindow()
        {
            InitializeComponent();
            cbSort.ItemsSource = ListSort;
            cbSort.SelectedIndex = 0;
            dgClient.ItemsSource = context.Client.ToList();
        }

        private void Filter()
        {
            listClient = context.Client.ToList();
            listClient = listClient.
                Where(i => i.LName.Contains(tbSearch.Text)
                || i.FName.Contains(tbSearch.Text)
                || i.Phone.Contains(tbSearch.Text)).ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0:
                    listClient.OrderBy(i => i.IdClient).ToList();
                    break;

                case 1:
                    listClient.OrderBy(i => i.LName).ToList();
                    break;

                case 2:
                    listClient.OrderBy(i => i.FName).ToList();
                    break;

                case 3:
                    listClient.OrderBy(i => i.Phone).ToList();
                    break;
                case 4:
                    listClient.OrderBy(i => i.Phone).ToList();
                    break;

                default:
                    listClient.OrderBy(i => i.IdGender).ToList();
                    break;
            }
            if (listClient.Count == 0)
            {
                MessageBox.Show("Записей нет");
            }
            dgClient.ItemsSource = listClient;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void dgClient_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var resClick = MessageBox.Show("Вы уыеренны?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                try
                {
                    if (resClick == MessageBoxResult.Yes)
                    {
                        EF.Client userDel = new EF.Client();

                        if (!(dgClient.SelectedItem is EF.Client))
                        {
                            MessageBox.Show("Запись не выбрана");
                            return;
                        }

                        userDel = (dgClient.SelectedItem as EF.Client);

                        context.Client.Remove(userDel);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
