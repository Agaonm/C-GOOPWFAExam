using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
using System.Diagnostics;

namespace CarsProject
{
    public partial class frmCars : Form
    {
        //Set up Database COnnection
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Agaonm\Downloads\CarsDatabase.accdb");
        // Set up Database Adapter
        OleDbDataAdapter adapter;
        //Set up Data Table 
        DataTable dt = new DataTable();
        //Set up Command for Running SQL Queries
        OleDbCommand cmd;
        //Start Database at row 0
        int pos = 0;

        public frmCars()
        {
            InitializeComponent();
        }

        private void frmCars_Load(object sender, EventArgs e)
        {
            //Fill Form with Database Entries on load
            string sql = "SELECT * FROM tblCars";
            adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(dt);
            showData(pos);
        }


        public void showData(int index)
        {
            //Set textboxs from Data Table
            tbRegNo.Text = dt.Rows[index]["VehicleRegNo"].ToString();
            tbMake.Text = dt.Rows[index]["Make"].ToString();
            tbEngine.Text = dt.Rows[index]["EngineSize"].ToString();
            //Convert Date and time to just Date
            tbDate.Text = Convert.ToDateTime(dt.Rows[index]["DateRegistered"]).ToString("dd/MM/yyyy");
            //Concert Number to Currency
            tbRent.Text = "€" + Convert.ToDouble(dt.Rows[index]["RentalPerDay"]).ToString("#,##,0.00");
           
            //Check Available set checkbox to True
            if (dt.Rows[index]["Available"].ToString() == "True"){
                cbAvailable.Checked = true;
            }
            //Set Check box to False 
            else
            {
                cbAvailable.Checked = false;
            }

            //Set the Bottom textbox to total number of entries
            tbTotalEntries.Text = pos+1 + " of " + dt.Rows.Count;
        }
        //First Button - Shows first Entry
        private void button1_Click(object sender, EventArgs e)
        {
            pos = 0;
            showData(pos);
        }

        //Previous Button, shows previous, Current - 1
        private void button2_Click(object sender, EventArgs e)
        {
            pos--;
            if(pos >= 0)
            {
                showData(pos);
            }
            else
            {
                MessageBox.Show("END");
            }
        }

        //Next Buton, Shows Next, Current + 1
        private void button3_Click(object sender, EventArgs e)
        {
            pos++;
            if(pos < dt.Rows.Count)
            {
                showData(pos);
            }
            else
            {
                MessageBox.Show("END");
                pos = dt.Rows.Count - 1;
            }
        }

        //Last Button, Shows Last 
        private void button4_Click(object sender, EventArgs e)
        {
            pos = dt.Rows.Count - 1;
            showData(pos);
        }

        //Exit the Program
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Update Entry
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Setup SQL Query
            string sql = "UPDATE tblCars SET [Make] = ?, [EngineSize] = ?, [DateRegistered] = ?, [RentalPerDay] = ? WHERE [VehicleRegNo] = ? ";
            //Start command
            cmd = new OleDbCommand(sql, conn);
            //Open Connection to Database
            conn.Open();

            //Sets paramters in [] to the variable next to it IN ORDER,
            //e.g [EngineSize] = @EngineSize and tbEngine.Text = 2nd ?
            //Must have parameters in the same order you put them in Query
            cmd.Parameters.AddWithValue("@Make", tbMake.Text);
            cmd.Parameters.AddWithValue("@EngineSize", tbEngine.Text);
            cmd.Parameters.AddWithValue("@DateRegistered", tbDate.Text);
            cmd.Parameters.AddWithValue("@RentalPerDay", tbRent.Text);
            //cmd.Parameters.AddWithValue("@Available", cbAvailable.Text); ####HAVENT GOT THIS WORKING YET####
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbRegNo.Text);

            //Run the Query
            cmd.ExecuteNonQuery();

            //Update the Program 
            updateScreen();
            //Close the Datbase connection
            conn.Close();
        }

        //Add Entry
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Set up SQL Query
            string sql = "INSERT INTO tblCars ([VehicleRegNo],[Make],[EngineSize],[DateRegistered],[RentalPerDay]) VALUES (?,?,?,?,?)";
            //Start Command
            cmd = new OleDbCommand(sql, conn);
            //Open Database Connection
            conn.Open();

            //Set parameters for Query, See Above for Explination
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbRegNo.Text);
            cmd.Parameters.AddWithValue("@Make", tbMake.Text);
            cmd.Parameters.AddWithValue("@EngineSize", tbEngine.Text);
            cmd.Parameters.AddWithValue("@DateRegistered", tbDate.Text);
            cmd.Parameters.AddWithValue("@RentalPerDay", tbRent.Text);
            //cmd.Parameters.AddWithValue("@Available", cbAvailable.Text); ####Not working yet####
            
            //Run the Query
            cmd.ExecuteNonQuery();
            //Close connection
            conn.Close();
            //Update the program
            updateScreen();
        }
        
        //Delete an entry
        private void btnDlt_Click(object sender, EventArgs e)
        {
            //Set up SQL Query
            string sql = "DELETE FROM tblCars WHERE [VehicleRegNo] = ?";
            //Start Command
            cmd = new OleDbCommand(sql, conn);
            //Open Database Connection
            conn.Open();

            //Set Parameter
            cmd.Parameters.AddWithValue("@VehicleRegNo", tbRegNo.Text);
            //Run Query
            cmd.ExecuteNonQuery();
            //Close Connection
            conn.Close();
            //Update Program
            updateScreen();
            //Goes back one - Not Needed!!
            button4.PerformClick();
        }

        private void updateScreen()
        {
            dt.Reset();
            adapter.Fill(dt);
            this.Refresh();
        }

        //Open the Search Form
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Create search form
            frmSearch frmSearch = new frmSearch();
            //Show search Form
            frmSearch.Show();
        }
    }
}
