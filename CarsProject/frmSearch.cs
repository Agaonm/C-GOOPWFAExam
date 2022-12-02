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
        //Set up Database Connection
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\CarsDatabase.accdb");
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
            cboField.Items.Add("DateRegistered");
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
            if (string.IsNullOrEmpty(cboField.Text) || string.IsNullOrEmpty(cboOperator.Text) || string.IsNullOrEmpty(tbValue.Text))
            {
                MessageBox.Show("Fill in Boxes");
            }
            else
            {
                // -- Working on it -- 
                string sql = "SELECT * FROM tblCars WHERE " + cboField.Text + " " + cboOperator.Text + " '" + tbValue.Text + "'";

                try
                {
                    //Setup Adapter to load data into a dataTable
                    adapter = new OleDbDataAdapter(sql, conn);
                    //Clear Previous Search
                    dt.Rows.Clear();
                    //Fill DataTable 
                    adapter.Fill(dt);
                    //Add DataTable to Grid View
                    dataGridView1.DataSource = dt;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }        
    }
}
