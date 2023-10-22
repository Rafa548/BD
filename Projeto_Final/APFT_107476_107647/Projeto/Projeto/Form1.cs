using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        private int currentEvent;
        private bool adding;

        public Form1()
        {
            InitializeComponent();
            loadEventsToolStripMenuItem_Click(null, null);

            string id = "ID";
            string name = "Event";
            string location = "Location";
            string supplier = "Supplier";
            string organizer = "Organizer";
            string sponsor = "Sponsor";

            string formattedString = string.Format("{0,-5} {1,-30} {2,-30} {3,-25} {4,-25} {5,-15}", id, name, location, supplier, organizer, sponsor);
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


        private void loadEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT\r\n\tE.Event_ID,\r\n\tEventName,\r\n\tN_tickets_vip,\r\n\tN_tickets_public,\r\n\tPrice_vip,\r\n\tPrice_public,\r\n\tEventLocation_ID,\r\n\tL_Name,\r\n    O.Name AS organizer_name,\r\n    S.Name AS supplier_name,\r\n    SP.Name AS sponsor_name\r\nFROM\r\n    Event E\r\n    LEFT JOIN Location L ON E.EventLocation_ID = L.Location_ID\r\n    LEFT JOIN Organizer ORG ON E.Event_ID = ORG.Event_ID\r\n    LEFT JOIN Company O ON ORG.O_ID = O.ID\r\n    LEFT JOIN Supplier SUP ON E.Event_ID = SUP.Event_ID\r\n    LEFT JOIN Company S ON SUP.S_ID = S.ID\r\n    LEFT JOIN Sponsor SPONS ON E.Event_ID = SPONS.Event_ID\r\n    LEFT JOIN Company SP ON SPONS.SP_ID = SP.ID;"
                , cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();


            while (reader.Read())
            {
                Event C = new Event();
                C.EventID = reader["Event_ID"].ToString();
                C.EventName = reader["EventName"].ToString();
                C.N_tickets_vip = reader["N_tickets_vip"].ToString();
                C.N_tickets_public = reader["N_tickets_public"].ToString();
                C.Price_vip = reader["Price_vip"].ToString();
                C.Price_public = reader["Price_public"].ToString();
                C.Location_ID = reader["EventLocation_ID"].ToString();
                C.Location_Name = reader["L_Name"].ToString();
                C.Supplier_Name = reader["supplier_name"].ToString();
                C.Sponsor_Name = reader["organizer_name"].ToString();
                C.Organizer_Name = reader["sponsor_name"].ToString();
                listBox1.Items.Add(C);
            }
            cn.Close();


            currentEvent = 0;
            ShowEvent();
        }

        private void SearchE(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            string query = "SELECT E.Event_ID, EventName, N_tickets_vip, N_tickets_public, Price_vip, Price_public, EventLocation_ID, L_Name, " +
                   "O.Name AS organizer_name, S.Name AS supplier_name, SP.Name AS sponsor_name " +
                   "FROM Event E " +
                   "LEFT JOIN Location L ON E.EventLocation_ID = L.Location_ID " +
                   "LEFT JOIN Organizer ORG ON E.Event_ID = ORG.Event_ID " +
                   "LEFT JOIN Company O ON ORG.O_ID = O.ID " +
                   "LEFT JOIN Supplier SUP ON E.Event_ID = SUP.Event_ID " +
                   "LEFT JOIN Company S ON SUP.S_ID = S.ID " +
                   "LEFT JOIN Sponsor SPONS ON E.Event_ID = SPONS.Event_ID " +
                   "LEFT JOIN Company SP ON SPONS.SP_ID = SP.ID ";

            List<string> filters = new List<string>();

            // Apply filters based on textbox values
            if (!string.IsNullOrWhiteSpace(txtLocationName.Text))
                filters.Add($" L_Name LIKE '%{txtLocationName.Text}%'");
            if (!string.IsNullOrWhiteSpace(txtEventName.Text))
                filters.Add($" EventName LIKE '%{txtEventName.Text}%'");
            if (!string.IsNullOrWhiteSpace(txtCName.Text))
                filters.Add($" (O.Name LIKE '%{txtCName.Text}%' OR S.Name = '%{txtCName.Text}%' OR SP.Name = '%{txtCName.Text}%')"); 

            // Append the WHERE clause if filters exist
            if (filters.Count > 0)
                query += "WHERE " + string.Join("And", filters);

            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Event C = new Event();
                C.EventID = reader["Event_ID"].ToString();
                C.EventName = reader["EventName"].ToString();
                C.N_tickets_vip = reader["N_tickets_vip"].ToString();
                C.N_tickets_public = reader["N_tickets_public"].ToString();
                C.Price_vip = reader["Price_vip"].ToString();
                C.Price_public = reader["Price_public"].ToString();
                C.Location_ID = reader["EventLocation_ID"].ToString();
                C.Location_Name = reader["L_Name"].ToString();
                C.Supplier_Name = reader["supplier_name"].ToString();
                C.Sponsor_Name = reader["organizer_name"].ToString();
                C.Organizer_Name = reader["sponsor_name"].ToString();
                listBox1.Items.Add(C);
            }

            cn.Close();
            currentEvent = 0;
            ShowEvent();
        }

        public void ShowEvent()
        {
            if (listBox1.Items.Count == 0 | currentEvent < 0)
                return;
            Event ev = new Event();
            ev = (Event)listBox1.Items[currentEvent];
            txtID.Text       =  ev.EventID            ;
            txtCompany.Text  =  ev.EventName          ;
            txtContact.Text  =  ev.Location_Name      ;
            txtCity.Text     =  ev.N_tickets_vip      ;
            txtState.Text    =  ev.Price_vip          ;
            TXTL_ID.Text      =  ev.Location_ID        ;
            txtTel.Text      =  ev.N_tickets_public   ;
            txtFax.Text      =  ev.Price_public       ;
           

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentEvent = listBox1.SelectedIndex;
                ShowEvent();
            }
        }

        public void LockControls()
        {
            txtID.ReadOnly = true;
            txtCompany.ReadOnly = true;
            txtContact.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtState.ReadOnly = true;
            TXTL_ID.ReadOnly = true;
            txtTel.ReadOnly = true;
            txtFax.ReadOnly = true;
            
        }

        public void UnlockControls()
        {
            txtID.ReadOnly = false;
            txtCompany.ReadOnly = false;
            txtContact.ReadOnly = true;
            txtCity.ReadOnly = false;
            txtState.ReadOnly = false;
            TXTL_ID.ReadOnly = false;
            txtTel.ReadOnly = false;
            txtFax.ReadOnly = false;
            
        }

        public void UnlockControls2()
        {
            txtID.ReadOnly = true;
            txtCompany.ReadOnly = false;
            txtContact.ReadOnly = true;
            txtCity.ReadOnly = false;
            txtState.ReadOnly = false;
            TXTL_ID.ReadOnly = false;
            txtTel.ReadOnly = false;
            txtFax.ReadOnly = false;

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
            txtID.Text = "";
            txtCompany.Text = "";
            txtContact.Text = "";
            txtCity.Text = "";
            TXTL_ID.Text = "";
            txtState.Text = "";
            txtTel.Text = "";
            txtFax.Text = "";
            
        }

        public void ClearSearchFields()
        {
            txtEventName.Text = "";
            txtLocationName.Text = "";
            txtCName.Text = "";
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
        private void SubmitEvent(Event C)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT Event " + "VALUES (@EventID, @EventName, @NTV, @NTP, @PV, @PP, @L_ID);";
                
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", C.EventID);
            cmd.Parameters.AddWithValue("@EventName", C.EventName);
            cmd.Parameters.AddWithValue("@NTV", C.N_tickets_vip);
            cmd.Parameters.AddWithValue("@NTP", C.N_tickets_public);
            cmd.Parameters.AddWithValue("@PV", C.Price_vip);
            cmd.Parameters.AddWithValue("@PP", C.Price_public);
            cmd.Parameters.AddWithValue("@L_ID", C.Location_ID);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update contact in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
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
                currentEvent = listBox1.SelectedIndex;
                if (currentEvent < 0)
                    currentEvent = 0;
                ShowEvent();
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
            currentEvent = listBox1.SelectedIndex;
            if (currentEvent < 0)
            {
                MessageBox.Show("Please select a contact to edit");
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
                SaveEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtID.Text);
            listBox1.SelectedIndex = idx;
            loadEventsToolStripMenuItem_Click(null, null);
            ShowButtons();
        }

        private void btn_DTLS_Click(object sender, EventArgs e)
        {
            currentEvent = listBox1.SelectedIndex;
            if (currentEvent < 0)
            {
                MessageBox.Show("Please select a Event to edit");
                return;
            }
            Event ev = new Event();
            ev = (Event)listBox1.Items[currentEvent];

            Form2 form2 = new Form2(ev);
            
            form2.TopLevel = false;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Dock = DockStyle.Fill;
            panel2.Controls.Clear(); 
            panel2.Controls.Add(form2);
            form2.Show();

        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveEvent(((Event)listBox1.SelectedItem).EventID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentEvent == listBox1.Items.Count)
                    currentEvent = listBox1.Items.Count - 1;
                if (currentEvent == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more Events");
                }
                else
                {
                    ShowEvent();
                }
            }
        }

        private void UpdateEvent(Event C)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateEvent @EventID,@EventName,@NTV,@NTP,@PV,@PP,@L_ID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", C.EventID);
            cmd.Parameters.AddWithValue("@EventName", C.EventName);
            cmd.Parameters.AddWithValue("@NTV", C.N_tickets_vip);
            cmd.Parameters.AddWithValue("@NTP", C.N_tickets_public);
            cmd.Parameters.AddWithValue("@PV", C.Price_vip);
            cmd.Parameters.AddWithValue("@PP", C.Price_public);
            cmd.Parameters.AddWithValue("@L_ID", C.Location_ID);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update contact in database. \n ERROR MESSAGE: \n" + ex.Message);
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


        private void RemoveEvent(string EventID)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteEvent @EventID ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", EventID);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete Event in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private bool SaveEvent()
        {
            Event e = new Event();
            try
            {
                e.EventID = txtID.Text;
                e.EventName = txtCompany.Text;
                e.Location_Name = txtContact.Text;
                e.N_tickets_vip = txtCity.Text;
                e.Price_vip = txtState.Text;
                e.Location_ID = TXTL_ID.Text;
                e.N_tickets_public = txtTel.Text;
                e.Price_public = txtFax.Text;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (adding)
            {
                SubmitEvent(e);
                listBox1.Items.Add(e);
            }
            else
            {
                UpdateEvent(e);
                listBox1.Items[currentEvent] = e;
            }
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //procurar
            SearchE(null, null);
            ClearSearchFields();
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

        }
    }
}

