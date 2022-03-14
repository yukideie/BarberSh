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
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        List<EF.Employee> listEmployee = new List<EF.Employee>();
        List<string> ListSort = new List<string>()
        {
            "По умолчанию",
            "По фамилии",
            "По имени",
            "По телефону",
            "По специализации"
        };
        public EmployeeWindow()
        {
            InitializeComponent();
            cbSort.ItemsSource = ListSort;
            cbSort.SelectedIndex = 0;
            Filter();
            
        }
        private void Filter() 
        {
            listEmployee = context.Employee.ToList();
            listEmployee = listEmployee.
                Where(i => i.LName.Contains(tbSearch.Text)
                || i.FName.Contains(tbSearch.Text)
                || i.Phone.Contains(tbSearch.Text)).ToList();

            switch (cbSort.SelectedIndex)
            {
                case 0: listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;

                case 1:
                    listEmployee.OrderBy(i => i.LName).ToList();
                    break;

                case 2:
                    listEmployee.OrderBy(i => i.FName).ToList();
                    break;

                case 3:
                    listEmployee.OrderBy(i => i.Phone).ToList();
                    break;
                case 4:
                    listEmployee.OrderBy(i => i.IdSpecialization).ToList();
                    break;

                default:
                    listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;
            }
            if (listEmployee.Count == 0)
            {
                MessageBox.Show("Записей нет");
            }
            lvEmployee.ItemsSource = listEmployee;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            this.Hide();
            addEmployeeWindow.ShowDialog();
            this.Show();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void lvEmployee_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var resClick = MessageBox.Show("Вы уверенны?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                try
                {
                    if (resClick == MessageBoxResult.Yes)
                    {
                        EF.Employee userDel = new EF.Employee();

                        if (!(lvEmployee.SelectedItem is EF.Employee))
                        {
                            MessageBox.Show("Запись не выбрана");
                            return;
                        }

                        userDel = (lvEmployee.SelectedItem as EF.Employee);

                        context.Employee.Remove(userDel);
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                MessageBox.Show($"Пользователь успешно удален", "Успех", MessageBoxButton.OK);
            }
            Filter();
        }
    }
}
