using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projeto
{
    public partial class Form4 : Form
    {
        private SqlConnection cn;
        private int currentClient;
        private bool addClient;
        public Client save;
        public Form4()
        {
            InitializeComponent();
            loadCustomersToolStripMenuItem_Click(null, null);
            string Ssn = "Ssn";
            string name = "Name";
            string age = "Age";
            cumbox.Items.Add("0-18");
            cumbox.Items.Add("18-30");
            cumbox.Items.Add("30-50");
            cumbox.Items.Add("50+");

           


            string formattedString = string.Format("{0,-10} {1,-25} {2,-5}" ,Ssn, name,age);
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

        private void loadCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT *  FROM Client"
                , cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();


            while (reader.Read())
            {
                Client C = new Client();
                C.Ssn = reader["Ssn"].ToString();
                C.Name = reader["C_Name"].ToString();
                C.Age= reader["Age"].ToString();
                listBox1.Items.Add(C);
            }
            cn.Close();


            currentClient = 0;
            ShowClient();
        }

        private void SearchC(object sender, EventArgs e,string agemin,string agemax)
        {
            if (!verifySGBDConnection())
                return;

            string query = "SELECT Ssn,C_Name,Age FROM Client  ";

            List<string> filters = new List<string>();

            // Apply filters based on textbox values
            if (!string.IsNullOrWhiteSpace(txtfiltername.Text))
                filters.Add($" C_Name LIKE '%{txtfiltername.Text}%'");
            if (!string.IsNullOrWhiteSpace(agemin))
                filters.Add($" Age >= '{agemin}'");
            if(agemax!=null)
                filters.Add($" Age < '{agemax}'");

            // Append the WHERE clause if filters exist
            if (filters.Count > 0)
                query += "WHERE " + string.Join("And", filters);

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Client C = new Client();
                C.Ssn = reader["Ssn"].ToString();
                C.Name = reader["C_Name"].ToString();
                C.Age = reader["Age"].ToString();
                listBox1.Items.Add(C);
            }

            cn.Close();
            currentClient = 0;
            ShowClient();
        }

        public void ShowClient()
        {
            if (listBox1.Items.Count == 0 | currentClient < 0)
                return;
            Client cl = new Client();
            cl = (Client)listBox1.Items[currentClient];
            txtSsn.Text = cl.Ssn;
            txtCompany.Text = cl.Name;
            txtAge.Text = cl.Age;

        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentClient = listBox1.SelectedIndex;
                ShowClient();
            }
        }
        public void LockControls()
        {
            txtSsn.ReadOnly = true;
            txtCompany.ReadOnly = true;
            txtAge.ReadOnly = true;
        }

        public void UnlockControls()
        {
            txtSsn.ReadOnly = false;
            txtCompany.ReadOnly = false;
            txtAge.ReadOnly = false;
        }

        public void UnlockControls2()
        {
            txtSsn.ReadOnly = true;
            txtCompany.ReadOnly = false;
            txtAge.ReadOnly = false;
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
            txtSsn.Text = "";
            txtCompany.Text = "";
            txtAge.Text = "";
        }

        public void ClearSearchFields()
        {
            txtfiltername.Text = "";
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

        private void txtSsn_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btn_DTLS_Click(object sender, EventArgs e)
        {
            currentClient = listBox1.SelectedIndex;
            if (currentClient < 0)
            {
                MessageBox.Show("Please select a client");
                return;
            }

            Client client= new Client();
            client = (Client)listBox1.Items[currentClient];


            Form5 form5 = new Form5(client);

            form5.TopLevel = false;
            form5.FormBorderStyle = FormBorderStyle.None;
            form5.Dock = DockStyle.Fill;
            panel2.Controls.Clear(); // Remove any existing controls from the panel
            panel2.Controls.Add(form5);
            form5.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();

            form6.TopLevel = false;
            form6.FormBorderStyle = FormBorderStyle.None;
            form6.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form6);
            form6.Show();

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
            //add client 
            addClient = true;
            ClearFields();
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            //cancel client
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                ShowClient();
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
            //edit client
            currentClient = listBox1.SelectedIndex;
            if (currentClient < 0)
            {
                MessageBox.Show("Please select a Client to edit");
                return;
            }
            addClient = false;
            HideButtons2();
            listBox1.Enabled = false;
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            //ok client
            try
            {
                SaveClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtCompany.Text);
            listBox1.SelectedIndex = idx;
            ShowButtons();
        }


        private bool SaveClient()
        {
            Client c = new Client();
            try
            {
                c.Ssn = txtSsn.Text;
                c.Name = txtCompany.Text;
                c.Age = txtAge.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (addClient)
            {
                SubmitClient(c);
                listBox1.Items.Add(c);
            }
            else
            {
                UpdateClient(c);
                listBox1.Items[currentClient] = c;
            }
            return true;
        }

        private void SubmitClient(Client c)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Client (Ssn,C_Name,Age) " +
                              "VALUES (@Ssn, @C_Name, @Age)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Ssn", c.Ssn);
            cmd.Parameters.AddWithValue("@C_Name", c.Name);
            cmd.Parameters.AddWithValue("@Age", c.Age);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update client in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void UpdateClient(Client c)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateClient @Ssn,@C_Name,@Age";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Ssn", c.Ssn);
            cmd.Parameters.AddWithValue("@C_Name", c.Name);
            cmd.Parameters.AddWithValue("@Age", c.Age);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update client in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update OK");
                else
                    MessageBox.Show("Update NOT OK");

                cn.Close();
            }
        }

        private void RemoveClient(string ssn)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteClient @Ssn";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Ssn", ssn);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete client in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveClient(((Client)listBox1.SelectedItem).Ssn);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentClient == listBox1.Items.Count)
                    currentClient = listBox1.Items.Count - 1;
                if (currentClient == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more clients");
                }
                else
                {
                    ShowClient();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchC(null, null,null,null);
            ClearSearchFields();
        }

        private void comboage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = cumbox.SelectedItem.ToString();

            switch (selectedOption)
            {
                case "0-18":
                    SearchC(null, null,"0","18");
                    break;
                case "18-30":
                    SearchC(null, null, "18", "30");
                    break;
                case "30-50":
                    SearchC(null, null, "30", "50");
                    break;
                case "50+":
                    SearchC(null, null, "50", null);
                    break;
            }
        }
    }
}
