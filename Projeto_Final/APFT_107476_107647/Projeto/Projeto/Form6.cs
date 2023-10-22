using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form6 : Form
    {
        private SqlConnection cn;
        private int currentLocation;
        private bool adding;
        public Location save;
        public Form6()
        {
            InitializeComponent();
            loadLocationsToolStripMenuItem_Click(null, null);
            string Location_ID = "L_ID";
            string L_Name = "L_Name";
            string L_Address = "L_Address";
            string L_City = "L_City";


            string formattedString = string.Format("{0,-5} {1,-40} {2,-30} {3,-20}", Location_ID, L_Name,L_Address,L_City);
            label1.Text = formattedString;
            label1.Font = new Font("Courier New", 8, FontStyle.Bold);
            listBox1.Font = new Font("Courier New", 8, FontStyle.Bold);
        }
        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=tcp:mednat.ieeta.pt\\SQLSERVER,8101;initial catalog=p5g9; uid = p5g9; password = 8ZUDcbzvua$");
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }
        private void loadLocationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Location"
                , cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();


            while (reader.Read())
            {
                Location loc = new Location();
                loc.Location_ID = reader["Location_ID"].ToString();
                loc.L_Name = reader["L_Name"].ToString();
                loc.L_Address = reader["L_Address"].ToString();
                loc.L_City = reader["L_City"].ToString() ;
                listBox1.Items.Add(loc);
            }
            cn.Close();


            currentLocation = 0;
            ShowLocation();
        }

        private void SearchL(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            string query = "SELECT Location_ID,L_Name,L_Address,L_City FROM Location  ";

            List<string> filters = new List<string>();

            // Apply filters based on textbox values
            if (!string.IsNullOrWhiteSpace(txtname.Text))
                filters.Add($" L_Name LIKE '%{txtname.Text}%'");
            if (!string.IsNullOrWhiteSpace(txtcity.Text))
                filters.Add($" L_City LIKE '%{txtcity.Text}%'");

            // Append the WHERE clause if filters exist
            if (filters.Count > 0)
                query += "WHERE " + string.Join("And", filters);

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Location loc = new Location();
                loc.Location_ID = reader["Location_ID"].ToString();
                loc.L_Name = reader["L_Name"].ToString();
                loc.L_Address = reader["L_Address"].ToString();
                loc.L_City = reader["L_City"].ToString();
                listBox1.Items.Add(loc);
            }

            cn.Close();
            currentLocation = 0;
            ShowLocation();
        }

        public void ShowLocation()
        {
            if (listBox1.Items.Count == 0 | currentLocation < 0)
                return;
            Location l = new Location();
            l = (Location)listBox1.Items[currentLocation];
            txtLocationID.Text = l.Location_ID;
            txtLName.Text = l.L_Name;
            txtAddress.Text = l.L_Address;
            txtLCity.Text = l.L_City;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentLocation = listBox1.SelectedIndex;
                ShowLocation();
            }
        }
        public void LockControls()
        {
            txtLocationID.ReadOnly = true;
            txtLName.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtLCity.ReadOnly = true;
        }

        public void UnlockControls()
        {
            txtLCity.ReadOnly = false;
            txtLName.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtLocationID.ReadOnly = false;
        }

        public void UnlockControls2()
        {
            txtLCity.ReadOnly = false;
            txtLName.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtLocationID.ReadOnly = true;
        }

        public void ShowButtons()
        {
            LockControls();
            bttnAdd.Visible = true;
            bttnDelete.Visible = true;
            bttnEdit.Visible = true;
            bttnOK.Visible = false;
            bttnCancel.Visible = false;
            btn_DTLS.Visible = true;
        }

        public void ClearFields()
        {
            txtAddress.Text = "";
            txtLCity.Text = "";
            txtLName.Text = "";
            txtLocationID.Text = "";
        }

        public void ClearSearchFields()
        {
            txtname.Text = "";
            txtcity.Text = "";
        }
        public void HideButtons()
        {
            UnlockControls();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
            btn_DTLS.Visible = false;
        }

        public void HideButtons2()
        {
            UnlockControls2();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
            btn_DTLS.Visible = false;
        }

        private void btn_DTLS_Click(object sender, EventArgs e)
        {

            currentLocation = listBox1.SelectedIndex;
            if (currentLocation < 0)
            {
                MessageBox.Show("Please select a Location");
                return;
            }

            Location location = new Location();
            location = (Location)listBox1.Items[currentLocation];


            Form7 form7 = new Form7(location);
            form7.TopLevel = false;
            form7.FormBorderStyle = FormBorderStyle.None;
            form7.Dock = DockStyle.Fill;
            panel2.Controls.Clear(); // Remove any existing controls from the panel
            panel2.Controls.Add(form7);
            form7.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            form4.TopLevel = false;
            form4.FormBorderStyle = FormBorderStyle.None;
            form4.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form4);
            form4.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btn_2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            form1.TopLevel = false;
            form1.FormBorderStyle = FormBorderStyle.None;
            form1.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form1);
            form1.Show();
        }

        private void bttnAdd_Click(object sender, EventArgs e)
        {
            adding = true;
            ClearFields();
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                ShowLocation();
            }
            else
            {
                ClearFields();
                LockControls();
            }
            ShowButtons();
        }

        private void bttnEdit_Click(object sender, EventArgs e)
        {
            currentLocation = listBox1.SelectedIndex;
            if (currentLocation < 0)
            {
                MessageBox.Show("Please select a Sale to edit");
                return;
            }
            adding = false;
            HideButtons2();
            listBox1.Enabled = false;
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLocation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtLocationID.Text);
            listBox1.SelectedIndex = idx;
            ShowButtons();
            loadLocationsToolStripMenuItem_Click(null, null);
        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveLocation(((Location)listBox1.SelectedItem).Location_ID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentLocation == listBox1.Items.Count)
                    currentLocation = listBox1.Items.Count - 1;
                if (currentLocation == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more Sales");
                }
                else
                {
                    ShowLocation();
                }
            }
        }

        private void UpdateLocation(Location lc)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateLocation @Location_ID,@L_Name,@L_Address,@L_City;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Location_ID", lc.Location_ID);
            cmd.Parameters.AddWithValue("@L_Name", lc.L_Name);
            cmd.Parameters.AddWithValue("@L_Address", lc.L_Address);
            cmd.Parameters.AddWithValue("@L_City", lc.L_City);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update sale in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void RemoveLocation(string L_ID)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec EliminateLocal @L_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@L_ID", L_ID);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete sale from the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private bool SaveLocation()
        {
            Location lc = new Location();
            try
            {
                lc.Location_ID = txtLocationID.Text;
                lc.L_Address = txtAddress.Text;
                lc.L_Name = txtLName.Text;
                lc.L_City = txtLCity.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (adding)
            {
                SubmitLocation(lc);
                listBox1.Items.Add(lc);
            }
            else
            {
                UpdateLocation(lc);
                listBox1.Items[currentLocation] = lc;
            }
            return true;
        }

        private void SubmitLocation(Location lc)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Location " +
                              "VALUES (@L_ID, @L_Name, @L_Addr, @L_City)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@L_ID", lc.Location_ID);
            cmd.Parameters.AddWithValue("@L_Name", lc.L_Name);
            cmd.Parameters.AddWithValue("@L_Addr", lc.L_Address);
            cmd.Parameters.AddWithValue("@L_City", lc.L_City);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update sale in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchL(null, null);
            ClearSearchFields();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
