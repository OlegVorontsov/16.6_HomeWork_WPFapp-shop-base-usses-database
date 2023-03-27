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

        public MainWindow()
        {
            InitializeComponent(); ViewBases();
        }

        public void ViewBases()
        {
            #region InitBases

            var conClientStr = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true
            };
            clientCon = new SqlConnection(conClientStr.ConnectionString);
            clientDA = new SqlDataAdapter();
            clientDT = new DataTable();

            #endregion

            #region Select

            var clientSelect = @"SELECT * FROM ClientsInfo Order By ClientsInfo.id";
            clientDA.SelectCommand = new SqlCommand(clientSelect, clientCon);

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
        }

        private void GVCurrentCellChanged(object sender, EventArgs e)
        {
            if (clientRow == null) return;
            clientRow.EndEdit();
            clientDA.Update(clientDT);
        }
    }
}
