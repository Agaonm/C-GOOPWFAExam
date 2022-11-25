using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarsProject
{
    public partial class frmSearch : Form
    {
        //!!!!!!MUST CHANGE THIS TO YOUR FILE PATH Set File Path to Database!!!
        const string FilePath = "C:\\Users\\Agaonm\\Downloads\\CarsDatabase.accdb";

        //Set up Database Connection
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+FilePath);
        //Set up Database Adapter
        OleDbDataAdapter adapter;
        //Set up Data Table
        DataTable dt = new DataTable();


        public frmSearch()
        {
            InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            //Add Fields to Combo Boxes
            cboField.Items.Add("Make");
            cboField.Items.Add("EngineSize");
            cboField.Items.Add("RentalPerDay");
            cboField.Items.Add("Available");

            cboOperator.Items.Add("=");
            cboOperator.Items.Add("<");
            cboOperator.Items.Add(">");
            cboOperator.Items.Add("<=");
            cboOperator.Items.Add(">=");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Close Search Form
            this.Close();
        }
       
        private void btnRun_Click(object sender, EventArgs e)
        {
            //string sql="SELECT * FROM tblCars WHERE Make = 'HondaS'";
            string sql = "SELECT * FROM tblCars WHERE " + cboField.Text + " " + cboOperator.Text + " '" + tbValue.Text + "'";       

            //Setup Adapter to load data into a dataTable
            adapter = new OleDbDataAdapter(sql, conn); 
            //Clear Previous Search
            dt.Rows.Clear();
            //Fill DataTable 
            adapter.Fill(dt);
            //Add DataTable to Grid View
            dataGridView1.DataSource = dt;
        }        
    }
}
