using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _16._6_HomeWork_WPFapp_shop_base_usses_database
{
    public partial class ClientAddWindow : Window
    {
        private ClientAddWindow() { InitializeComponent(); }
        public ClientAddWindow(DataRow row) : this()
        {
            cancelBtn.Click += delegate { this.DialogResult = false; };
            okBtn.Click += delegate
            {
                row["Surname"] = txtClientSurname.Text;
                row["Name"] = txtClientName.Text;
                row["Patronymic"] = txtClientPatr.Text;
                row["Phonenumber"] = txtClientPhone.Text;
                row["Email"] = txtClientEmail.Text;
                this.DialogResult = !false;
            };
        }
    }
}
