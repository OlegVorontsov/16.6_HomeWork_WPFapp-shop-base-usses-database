using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _16._6_HomeWork_WPFapp_shop_base_usses_database
{
    public partial class MainWindow : Window
    {
        SqlConnection Con;

        SqlDataAdapter clientDA;
        DataTable clientDT;
        DataRowView clientRow;

        SqlDataAdapter itemDA;
        DataTable itemDT;
        DataRowView itemRow;

        static string emailSelected;
        bool clientDeleting = false;

        public MainWindow()
        {
            InitializeComponent(); ConnectBases();
        }

        public void ConnectBases()
        {
            #region InitTools

            var conStr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true
            };
            Con = new SqlConnection(conStr.ConnectionString);
            clientDA = new SqlDataAdapter();
            clientDT = new DataTable();

            itemDA = new SqlDataAdapter();
            itemDT = new DataTable();

            #endregion

            #region SelectClients

            var clientSelect = @"SELECT * FROM ClientsInfo Order By ClientsInfo.id";
            clientDA.SelectCommand = new SqlCommand(clientSelect, Con);

            #endregion

            #region InsertIntoBases

            var sqlInsertClients = @"insert into ClientsInfo (surname, name, patronymic, phonenumber, email) 
                                                     values (@surname, @name, @patronymic, @phonenumber, @email); 
                     set @id = @@identity;";

            clientDA.InsertCommand = new SqlCommand(sqlInsertClients, Con);

            clientDA.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            clientDA.InsertCommand.Parameters.Add("@surname", SqlDbType.NVarChar, 20, "surname");
            clientDA.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 10, "name");
            clientDA.InsertCommand.Parameters.Add("@patronymic", SqlDbType.NVarChar, 20, "patronymic");
            clientDA.InsertCommand.Parameters.Add("@phonenumber", SqlDbType.Float, 10, "phonenumber");
            clientDA.InsertCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "email");

            var sqlInsertItems = @"insert into ItemsInfo (Email, Code, Title) 
                                                  values (@email, @code, @title); 
                     set @ID = @@identity;";

            itemDA.InsertCommand = new SqlCommand(sqlInsertItems, Con);

            itemDA.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "ID").Direction = ParameterDirection.Output;
            itemDA.InsertCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "Email");
            itemDA.InsertCommand.Parameters.Add("@code", SqlDbType.Int, 10, "Code");
            itemDA.InsertCommand.Parameters.Add("@title", SqlDbType.NVarChar, 20, "Title");

            #endregion

            #region UpdateBases

            var sqlClientsUpdate = @"UPDATE ClientsInfo SET 
                           surname = @surname,
                           name = @name, 
                           patronymic = @patronymic,
                           phonenumber = @phonenumber,
                           email = @email
                           WHERE id = @id";

            clientDA.UpdateCommand = new SqlCommand(sqlClientsUpdate, Con);
            clientDA.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id").SourceVersion = DataRowVersion.Original;
            clientDA.UpdateCommand.Parameters.Add("@surname", SqlDbType.NVarChar, 20, "surname");
            clientDA.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 10, "name");
            clientDA.UpdateCommand.Parameters.Add("@patronymic", SqlDbType.NVarChar, 20, "patronymic");
            clientDA.UpdateCommand.Parameters.Add("@phonenumber", SqlDbType.Float, 10, "phonenumber");
            clientDA.UpdateCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "email");

            var sqlItemsUpdate = @"UPDATE ItemsInfo SET 
                           Email = @email,
                           Code = @code, 
                           Title = @title
                           WHERE ID = @id";

            itemDA.UpdateCommand = new SqlCommand(sqlItemsUpdate, Con);
            itemDA.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 0, "ID").SourceVersion = DataRowVersion.Original;
            itemDA.UpdateCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "Email");
            itemDA.UpdateCommand.Parameters.Add("@code", SqlDbType.NVarChar, 10, "Code");
            itemDA.UpdateCommand.Parameters.Add("@title", SqlDbType.NVarChar, 20, "Title");

            #endregion

            #region FillClients

            clientDA.Fill(clientDT);
            clientsGridView.DataContext = clientDT.DefaultView;

            #endregion

            #region delete

            var sqlDeleteClient = "DELETE FROM ClientsInfo WHERE id = @id";

            clientDA.DeleteCommand = new SqlCommand(sqlDeleteClient, Con);
            clientDA.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id");

            var sqlDeleteItem = "DELETE FROM ItemsInfo WHERE ID = @id";

            itemDA.DeleteCommand = new SqlCommand(sqlDeleteItem, Con);
            itemDA.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 4, "ID");

            #endregion
        }

        private void GVCellEditEndingClient(object sender, DataGridCellEditEndingEventArgs e)
        {
            clientRow = (DataRowView)clientsGridView.SelectedItem;
            clientRow.BeginEdit();
        }

        private void GVCurrentCellChangedClient(object sender, EventArgs e)
        {
            if (clientRow == null) return;
            clientRow.EndEdit();
            clientDA.Update(clientDT);
        }

        private void GVCellEditEndingItem(object sender, DataGridCellEditEndingEventArgs e)
        {
            itemRow = (DataRowView)itemsGridView.SelectedItem;
            itemRow.BeginEdit();
        }

        private void GVCurrentCellChangedItem(object sender, EventArgs e)
        {
            if (itemRow == null) return;
            itemRow.EndEdit();
            itemDA.Update(itemDT);
        }
        //выбор клиента
        private void clientsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!clientDeleting)
            {
                object[] temp;
                clientRow = (DataRowView)clientsGridView.SelectedItem;
                temp = clientRow.Row.ItemArray;
                emailSelected = temp[5].ToString();
                ViewItems();
            }
        }
        //вывод товаров выбранного клиента
        public void ViewItems()
        {
            itemDT.Clear();
            string sqlExpression = "SELECT * FROM ItemsInfo WHERE Email = " + $"'{emailSelected}'";
            itemDA.SelectCommand = new SqlCommand(sqlExpression, Con);
            itemDA.Fill(itemDT);
            itemsGridView.DataContext = itemDT.DefaultView;
        }
        //добавление клиента через контекстное меню
        private void MenuItemAddClientClick(object sender, RoutedEventArgs e)
        {
            DataRow r = clientDT.NewRow();
            ClientAddWindow add = new ClientAddWindow(r);
            add.ShowDialog();

            if (add.DialogResult.Value)
            {
                clientDT.Rows.Add(r);
                clientDA.Update(clientDT);
            }
        }
        //удаление клиета через контекстное меню
        private void MenuItemDeleteClientClick(object sender, RoutedEventArgs e)
        {
            clientRow = (DataRowView)clientsGridView.SelectedItem;
            clientDeleting = true;
            clientRow.Row.Delete();
            clientDA.Update(clientDT);
            clientDeleting = false;
        }
        //добавление товара через контекстное меню
        private void MenuItemAddItemClick(object sender, RoutedEventArgs e)
        {
            DataRow r = itemDT.NewRow();
            ItemAddWindow add = new ItemAddWindow(r);
            add.ShowDialog();

            if (add.DialogResult.Value)
            {
                itemDT.Rows.Add(r);
                itemDA.Update(itemDT);
            }
        }
        //удаление товара через контекстное меню
        private void MenuItemDeleteItemClick(object sender, RoutedEventArgs e)
        {
            itemRow = (DataRowView)itemsGridView.SelectedItem;
            itemRow.Row.Delete();
            itemDA.Update(itemDT);
        }


    }
}
