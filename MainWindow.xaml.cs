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
        SqlConnection sqlCon;
        SqlDataAdapter sqlDA;
        DataTable sqlDT;
        DataRowView sqlRow;

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

            #endregion

            #region Select

            var sqlSelect = @"SELECT * FROM ClientsInfo Order By ClientsInfo.id";
            sqlDA.SelectCommand = new SqlCommand(sqlSelect, sqlCon);

            #endregion

            #region FillBases

            sqlDA.Fill(sqlDT);
            clientsGridView.DataContext = sqlDT.DefaultView;

            #endregion

        }

        private void GVCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            sqlRow = (DataRowView)clientsGridView.SelectedItem;
            sqlRow.BeginEdit();
        }

        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (sqlRow == null) return;
            sqlRow.EndEdit();
            sqlDA.Update(sqlDT);
        }
    }
}
