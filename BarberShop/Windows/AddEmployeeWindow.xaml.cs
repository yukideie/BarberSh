using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        EF.Employee editEmployee = new EF.Employee();
        private string pathPhoto = null;
        bool isEdit = true;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            cbSpecialization.ItemsSource = context.Specialization.ToList();
            cbSpecialization.DisplayMemberPath = "NameOfSpecialization";
            cbSpecialization.SelectedIndex = 0;

            isEdit = false;
        }

        public AddEmployeeWindow(EF.Employee employee)
        {
            InitializeComponent();

            cbSpecialization.ItemsSource = context.Specialization.ToList();
            cbSpecialization.DisplayMemberPath = "NameOfSpecialization";
            cbSpecialization.SelectedIndex = Convert.ToInt32(employee.IdSpecialization - 1);

            tbFName.Text = employee.FName;
            tbLName.Text = employee.LName;
            tbPhone.Text = employee.Phone;
            tbLogin.Text = employee.Login;
            tbPassword1.Text = employee.Password;

            editEmployee = employee;
            isEdit = true;

            if (employee.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(employee.Photo)) 
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.EndInit();
                    photoUser.Source = bitmapImage;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFName.Text))
            {
                MessageBox.Show("Поле Имя не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbLName.Text))
            {
                MessageBox.Show("Поле Фамилия не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                MessageBox.Show("Поле Телефон не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                MessageBox.Show("Поле Логин не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbPassword1.Text))
            {
                MessageBox.Show("Поле Пароль не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return;
            }
            if (tbRePassword.Text != tbPassword1.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK);
                return;
            }

            var resClick = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            try
            {
                if (resClick == MessageBoxResult.Yes)
                {
                    if (isEdit)
                    {
                        
                        editEmployee.FName = tbFName.Text;
                        editEmployee.LName = tbLName.Text;
                        editEmployee.IdSpecialization = cbSpecialization.SelectedIndex + 1;
                        editEmployee.Phone = tbPhone.Text;
                        editEmployee.Login = tbLogin.Text;
                        editEmployee.Password = tbPassword1.Text;

                        if (pathPhoto != null)
                        {
                            editEmployee.Photo = File.ReadAllBytes(pathPhoto);
                        }

                        context.SaveChanges();

                        MessageBox.Show("Пользователь успешно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();

                    }
                    else
                    {
                        EF.Employee addEmployee = new EF.Employee();
                        addEmployee.FName = tbFName.Text;
                        addEmployee.LName = tbLName.Text;
                        addEmployee.IdSpecialization = cbSpecialization.SelectedIndex + 1;
                        addEmployee.Phone = tbPhone.Text;
                        addEmployee.Login = tbLogin.Text;
                        addEmployee.Password = tbPassword1.Text;

                        if (pathPhoto != null)
                        {
                            editEmployee.Photo = File.ReadAllBytes(pathPhoto);
                        }

                        context.Employee.Add(addEmployee);
                        context.SaveChanges();

                        MessageBox.Show("Пользователь успешно добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeWindow = new EmployeeWindow();
            employeeWindow.ShowDialog();
            this.Close();
            
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                photoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                pathPhoto = openFile.FileName;
            }
        }
    }
}
