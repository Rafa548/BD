using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form7 : Form
    {
        private SqlConnection cn;
        private int currentEventL;
        private bool addevent;
        public Event save;
        private string loc_id;
        public Form7(Location loc)
        {
            InitializeComponent();
            loc_id = loc.Location_ID;
            string loc_name = loc.L_Name;

            loadCustomersToolStripMenuItem_Click(null, null);

            string id = "ID";
            string name = "Event";
            string supplier = "Supplier";
            string organizer = "Organizer";
            string sponsor = "Sponsor";

            string formattedString = string.Format("{0,-5} {1,-30} {2,-25} {3,-25} {4,-15}", id, name , supplier, organizer, sponsor);

            label2.Text = loc_name + " [" + loc_id + "]";
            label2.Font = new Font("Courier New", 24, FontStyle.Bold);

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

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT\r\n\tE.Event_ID,\r\n\tEventName,\r\n\tN_tickets_vip,\r\n\tN_tickets_public,\r\n\tPrice_vip,\r\n\tPrice_public,\r\n\tEventLocation_ID,\r\n\tL_Name,\r\n    O.Name AS organizer_name,\r\n    S.Name AS supplier_name,\r\n    SP.Name AS sponsor_name\r\nFROM\r\n    Event E\r\n    LEFT JOIN Location L ON E.EventLocation_ID = L.Location_ID\r\n    LEFT JOIN Organizer ORG ON E.Event_ID = ORG.Event_ID\r\n    LEFT JOIN Company O ON ORG.O_ID = O.ID\r\n    LEFT JOIN Supplier SUP ON E.Event_ID = SUP.Event_ID\r\n    LEFT JOIN Company S ON SUP.S_ID = S.ID\r\n    LEFT JOIN Sponsor SPONS ON E.Event_ID = SPONS.Event_ID\r\n    LEFT JOIN Company SP ON SPONS.SP_ID = SP.ID Where E.EventLocation_ID = @ID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", loc_id);
            cmd.Connection = cn;
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();


            while (reader.Read())
            {
                Event_L C = new Event_L();
                C.EventID = reader["Event_ID"].ToString();
                C.EventName = reader["EventName"].ToString();
                C.N_tickets_vip = reader["N_tickets_vip"].ToString();
                C.N_tickets_public = reader["N_tickets_public"].ToString();
                C.Price_public = reader["Price_public"].ToString();
                C.Price_vip = reader["Price_vip"].ToString();
                C.Supplier_Name = reader["supplier_name"].ToString();
                C.Sponsor_Name = reader["organizer_name"].ToString();
                C.Organizer_Name = reader["sponsor_name"].ToString();
                listBox1.Items.Add(C);
            }
            cn.Close();


            currentEventL = 0;
            ShowEventL();
        }
        public void ShowEventL()
        {
            if (listBox1.Items.Count == 0 | currentEventL < 0)
                return;
            Event_L C = new Event_L();
            C = (Event_L)listBox1.Items[currentEventL];
            txtID.Text = C.EventID.ToString();
            txtName.Text = C.EventName.ToString();
            txtNTP.Text = C.N_tickets_public.ToString();
            txtNTVip.Text = C.N_tickets_vip.ToString();
            txtPricePublic.Text = C.Price_public.ToString();
            txtPriceVIP.Text = C.Price_vip.ToString();
        }
        public void LockControls()
        {
            txtID.ReadOnly = true;
            txtName.ReadOnly = true;
            txtNTP.ReadOnly = true;
            txtNTVip.ReadOnly = true;
            txtPricePublic.ReadOnly = true;
            txtPriceVIP.ReadOnly = true;
        }

        public void UnlockControls()
        {
            txtID.ReadOnly = false;
            txtName.ReadOnly = false;
            txtNTP.ReadOnly = false;
            txtNTVip.ReadOnly = false;
            txtPricePublic.ReadOnly = false;
            txtPriceVIP.ReadOnly = false;
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
            txtName.Text = "";
            txtNTP.Text = "";
            txtNTVip.Text = "";
            txtPricePublic.Text = "";
            txtPriceVIP.Text = "";
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

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentEventL = listBox1.SelectedIndex;
                ShowEventL();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();

            form6.TopLevel = false;
            form6.FormBorderStyle = FormBorderStyle.None;
            form6.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form6);
            form6.Show();
        }

        private void btn_DTLS_Click(object sender, EventArgs e)
        {
            currentEventL = listBox1.SelectedIndex;
            if (currentEventL < 0)
            {
                MessageBox.Show("Please select a Event to edit");
                return;
            }



            Event_L ev = new Event_L();
            //Implementar no form3 deve ser igual ao 2 com o objeto Event_L
            ev = (Event_L)listBox1.Items[currentEventL];

            Form3 form3 = new Form3(ev);

            form3.TopLevel = false;
            form3.FormBorderStyle = FormBorderStyle.None;
            form3.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form3);
            form3.Show();
        }

        private void bttnAdd_Click(object sender, EventArgs e)
        {
            //add event
            addevent = true;
            ClearFields();
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            //cancel event
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                ShowEventL();
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
            //edit event
            currentEventL = listBox1.SelectedIndex;
            if (currentEventL < 0)
            {
                MessageBox.Show("Please select a Sale to edit");
                return;
            }
            addevent = false;
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            //ok event
            try
            {
                SaveEventL();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtName.Text);
            listBox1.SelectedIndex = idx;
            loadCustomersToolStripMenuItem_Click(null, null);
            ShowButtons();
        }

        private bool SaveEventL()
        {
            Event_L e = new Event_L();
            try
            {
                e.EventID = txtID.Text;
                e.EventName = txtName.Text;
                e.N_tickets_vip = txtNTVip.Text;
                e.N_tickets_public = txtNTP.Text;
                e.Price_public = txtPricePublic.Text;
                e.Price_vip = txtPriceVIP.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (addevent)
            {
                SubmitEventL(e);
                listBox1.Items.Add(e);
            }
            else
            {
                UpdateEventL(e);
                listBox1.Items[currentEventL] = e;
            }
            return true;
        }

        private void SubmitEventL(Event_L e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT Event (Event_ID, EventName, N_tickets_vip, N_tickets_public,EventLocation_ID,Price_vip,Price_public) VAlUES (@EventID,@EventName,@N_tickets_public,@N_tickets_public,@Location_ID,@Price_vip,@Price_public) ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", e.EventID);
            cmd.Parameters.AddWithValue("@EventName", e.EventName);
            cmd.Parameters.AddWithValue("@N_tickets_vip", e.N_tickets_vip);
            cmd.Parameters.AddWithValue("@N_tickets_public", e.N_tickets_public);
            cmd.Parameters.AddWithValue("@Location_ID", loc_id);
            cmd.Parameters.AddWithValue("@Price_vip", e.Price_vip);
            cmd.Parameters.AddWithValue("@Price_public", e.Price_public);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update event in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        private void UpdateEventL(Event_L e)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateEventL @EventID,@EventName,@NTV,@NTP,@Location_ID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", e.EventID);
            cmd.Parameters.AddWithValue("@EventName", e.EventName);
            cmd.Parameters.AddWithValue("@NTV", e.N_tickets_vip);
            cmd.Parameters.AddWithValue("@NTP", e.N_tickets_public);
            cmd.Parameters.AddWithValue("@Location_ID", loc_id);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update event in database. \n ERROR MESSAGE: \n" + ex.Message);
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

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveEventL((Event_L)listBox1.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentEventL == listBox1.Items.Count)
                    currentEventL = listBox1.Items.Count - 1;
                if (currentEventL == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more event");
                }
                else
                {
                    ShowEventL();
                }
            }
        }
        private void RemoveEventL(Event_L e)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteEvent @EventID ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", e.EventID);
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
    }
}
