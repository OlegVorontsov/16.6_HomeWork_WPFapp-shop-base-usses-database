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
        SqlConnection clientCon;
        SqlDataAdapter clientDA;
        DataTable clientDT;
        DataRowView clientRow;

        SqlConnection itemCon;
        SqlDataAdapter itemDA;
        DataTable itemDT;
        DataRowView itemRow;

        static string emailSelected;

        public MainWindow()
        {
            InitializeComponent(); ViewBases();
        }

        public void ViewItems()
        {
            var conStr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true
            };

            string sqlExpression = "SELECT * FROM ItemsInfo WHERE Email = @Param";

            SqlCommand command = new SqlCommand(sqlExpression, itemCon);
            command.Parameters.Add("@Param", SqlDbType.NVarChar, 30, emailSelected);

            itemDA.Fill(itemDT);
            itemsGridView.DataContext = itemDT.DefaultView;

        }

        public void ViewBases()
        {
            #region InitBases

            var conStr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true
            };
            clientCon = new SqlConnection(conStr.ConnectionString);
            clientDA = new SqlDataAdapter();
            clientDT = new DataTable();

            itemCon = new SqlConnection(conStr.ConnectionString);
            itemDA = new SqlDataAdapter();
            itemDT = new DataTable();

            #endregion

            #region Select

            var clientSelect = @"SELECT * FROM ClientsInfo Order By ClientsInfo.id";
            clientDA.SelectCommand = new SqlCommand(clientSelect, clientCon);

            var itemSelect = @"SELECT * FROM ItemsInfo Order By ItemsInfo.ID";
            itemDA.SelectCommand = new SqlCommand(itemSelect, itemCon);

            #endregion

            #region Insert

            var sql = @"insert into ClientsInfo (id, surname, name, patronymic, phonenumber, email) 
                                            values (@surname, @name, @patronymic, @phonenumber, @email); 
                     set @id = @@identity;";

            clientDA.InsertCommand = new SqlCommand(sql, clientCon);

            clientDA.InsertCommand.Parameters.Add("@id", SqlDbType.Int, 4, "id").Direction = ParameterDirection.Output;
            clientDA.InsertCommand.Parameters.Add("@surname", SqlDbType.NVarChar, 20, "surname");
            clientDA.InsertCommand.Parameters.Add("@name", SqlDbType.NVarChar, 10, "name");
            clientDA.InsertCommand.Parameters.Add("@patronymic", SqlDbType.NVarChar, 20, "patronymic");
            clientDA.InsertCommand.Parameters.Add("@phonenumber", SqlDbType.Float, 10, "phonenumber");
            clientDA.InsertCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "email");

            #endregion

            #region Update

            var sqlUpdate = @"UPDATE ClientsInfo SET 
                           surname = @surname,
                           name = @name, 
                           patronymic = @patronymic,
                           phonenumber = @phonenumber,
                           email = @email
                           WHERE id = @id";

            clientDA.UpdateCommand = new SqlCommand(sqlUpdate, clientCon);
            clientDA.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 0, "id").SourceVersion = DataRowVersion.Original;
            clientDA.UpdateCommand.Parameters.Add("@surname", SqlDbType.NVarChar, 20, "surname");
            clientDA.UpdateCommand.Parameters.Add("@name", SqlDbType.NVarChar, 10, "name");
            clientDA.UpdateCommand.Parameters.Add("@patronymic", SqlDbType.NVarChar, 20, "patronymic");
            clientDA.UpdateCommand.Parameters.Add("@phonenumber", SqlDbType.Float, 10, "phonenumber");
            clientDA.UpdateCommand.Parameters.Add("@email", SqlDbType.NVarChar, 30, "email");

            #endregion

            #region FillBases

            clientDA.Fill(clientDT);
            clientsGridView.DataContext = clientDT.DefaultView;

            #endregion

        }

        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            clientRow = (DataRowView)clientsGridView.SelectedItem;
            clientRow.BeginEdit();

            itemRow = (DataRowView)itemsGridView.SelectedItem;
            itemRow.BeginEdit();
        }

        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (clientRow == null) return;
            clientRow.EndEdit();
            clientDA.Update(clientDT);

            if (itemRow == null) return;
            itemRow.EndEdit();
            itemDA.Update(itemDT);
        }

        private void clientsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object[] temp;
            clientRow = (DataRowView)clientsGridView.SelectedItem;
            temp = clientRow.Row.ItemArray;
            emailSelected = temp[5].ToString();
            ViewItems();
        }
    }
}
