using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlCon;
        SqlDataAdapter sqlDA;
        DataTable sqlDT;
        DataRowView sqlRow;

        OleDbConnection odbCon;
        OleDbDataAdapter odbDA;
        DataTable odbDT;
        DataRowView odbRow;

        public MainWindow()
        {
            InitializeComponent(); ViewBases();
        }

        public void ViewBases()
        {
            #region InitBases

            var conSqlStr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true
            };
            sqlCon = new SqlConnection(conSqlStr.ConnectionString);
            sqlDA = new SqlDataAdapter();
            sqlDT = new DataTable();

            //var conOdbStr = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=C:\Users\admin\source\repos\C#\HomeWork\16.6_HomeWork_WPFapp shop base usses database\ItemsDB.accdb";

            var conOdbStr = new OleDbConnectionStringBuilder
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = @"C:\Users\admin\source\repos\C#\HomeWork\16.6_HomeWork_WPFapp shop base usses database\ItemsDB.accdb",
                PersistSecurityInfo = true
            };
            odbCon = new OleDbConnection(conOdbStr.ConnectionString);
            odbDA = new OleDbDataAdapter();
            odbDT = new DataTable();

            #endregion

            #region Select

            var sqlSelect = @"SELECT * FROM ClientsInfo Order By ClientsInfo.id";
            sqlDA.SelectCommand = new SqlCommand(sqlSelect, sqlCon);

            var odbSelect = @"SELECT * FROM Items Order By Items.ID";
            odbDA.SelectCommand = new OleDbCommand(odbSelect, odbCon);

            #endregion

            #region FillBases

            sqlDA.Fill(sqlDT);
            clientsGridView.DataContext = sqlDT.DefaultView;

            odbDA.Fill(odbDT);
            itemsGridView.DataContext = odbDT.DefaultView;

            #endregion

        }

        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            sqlRow = (DataRowView)clientsGridView.SelectedItem;
            sqlRow.BeginEdit();

            odbRow = (DataRowView)itemsGridView.SelectedItem;
            odbRow.BeginEdit();
        }

        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (sqlRow == null) return;
            sqlRow.EndEdit();
            sqlDA.Update(sqlDT);

            if (odbRow == null) return;
            odbRow.EndEdit();
            odbDA.Update(odbDT);
        }
    }
}
