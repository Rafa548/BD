using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Projeto
{
    public partial class Form5 : Form
    {
        private SqlConnection cn;
        private int currentSale;
        private bool addSale;
        public Sales save;
        private string cn_ssn;
        public Form5(Client cn)
        {
            InitializeComponent();
            cn_ssn = cn.Ssn;
            loadCustomersToolStripMenuItem_Click(null, null);
            string SaleID = "S_ID";
            string EventID = "E_ID";
            string N_tickets = "N_Tickets";
            string T_Type = "Type";
            string e_name = "EventName";


            label5.Text = cn.Name + " [" + cn_ssn + "]";
            label5.Font = new Font("Courier New", 24, FontStyle.Bold);

            string formattedString = string.Format("{0,-5} {1,-5} {2,-20} {3,-10} {4,-10}", SaleID, EventID, e_name, N_tickets, T_Type);
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
            cmd.CommandText = "SELECT Sale_ID,Event.Event_ID,EventName,N_tickets,Ssn,T_type " + "FROM Sales Join EVENT on EVENT.Event_ID = Sales.Event_ID Where SSN = @ID ;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", cn_ssn);
            cmd.Connection = cn;
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Sales_Client sale = new Sales_Client();
                sale.SaleID = reader["Sale_ID"].ToString();
                sale.EventID = reader["Event_ID"].ToString();
                sale.E_Name = reader["EventName"].ToString();
                sale.N_tickets = reader["N_tickets"].ToString();
                sale.Ssn = reader["Ssn"].ToString();
                sale.T_type = reader["T_type"].ToString();

                listBox1.Items.Add(sale);
            }

            cn.Close();

            currentSale = 0;
            ShowSales();
        }
        public void ShowSales()
        {
            if (listBox1.Items.Count == 0 | currentSale < 0)
                return;
            Sales_Client sl = new Sales_Client();
            sl = (Sales_Client)listBox1.Items[currentSale];
            txtEventID.Text = sl.EventID;
            txtSaleID.Text = sl.SaleID;
            txtNTickets.Text = sl.N_tickets;
            textTType.Text = sl.T_type;
            textE_name.Text = sl.E_Name;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentSale = listBox1.SelectedIndex;
                ShowSales();
            }
        }
        public void LockControls()
        {

            txtEventID.ReadOnly = true;
            txtSaleID.ReadOnly = true;
            txtNTickets.ReadOnly = true;
            textTType.ReadOnly = true;
            textE_name.ReadOnly = true;
        }

        public void UnlockControls()
        {
            txtEventID.ReadOnly = true;
            txtSaleID.ReadOnly = true;
            txtNTickets.ReadOnly = false;
            textTType.ReadOnly = false;
            textE_name.ReadOnly = true;
        }

        public void UnlockControlsAdd()
        {
            txtEventID.ReadOnly = false;
            txtSaleID.ReadOnly = false;
            txtNTickets.ReadOnly = false;
            textTType.ReadOnly = false;
            textE_name.ReadOnly = true;
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
            txtEventID.Text = "";
            txtSaleID.Text = "";
            txtNTickets.Text = "";
            textTType.Text = "";
            textE_name.Text = "";
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
            UnlockControlsAdd();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
            btn_DTLS.Visible = false;
        }


        private void textTType_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_DTLS_Click(object sender, EventArgs e)
        {
            //currentSale = listBox1.SelectedIndex;
            //if (currentSale <= 0)
            //{
            //    MessageBox.Show("Please select a client");
            //    return;
            //

            Event ev = new Event();
            ev.EventID = txtEventID.Text;
            ev.EventName = textE_name.Text;

            Form2 form2 = new Form2(ev);
            form2.TopLevel = false;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Dock = DockStyle.Fill;
            panel2.Controls.Clear(); // Remove any existing controls from the panel
            panel2.Controls.Add(form2);
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            form4.TopLevel = false;
            form4.FormBorderStyle = FormBorderStyle.None;
            form4.Dock = DockStyle.Fill;
            panel2.Controls.Clear();
            panel2.Controls.Add(form4);
            form4.Show();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void bttnAdd_Click(object sender, EventArgs e)
        {
            //add sale
            addSale = true;
            ClearFields();
            HideButtons2();
            listBox1.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            //cancel sale
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                ShowSales();
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
            //edit sale
            currentSale = listBox1.SelectedIndex;
            if (currentSale < 0)
            {
                MessageBox.Show("Please select a Sale to edit");
                return;
            }
            addSale = false;
            HideButtons();
            listBox1.Enabled = false;
        }

        private void bttnOK_Click(object sender, EventArgs e)
        {
            //ok sale
            try
            {
                SaveSale();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtEventID.Text);
            listBox1.SelectedIndex = idx;
            loadCustomersToolStripMenuItem_Click(null, null);
            ShowButtons();
        }


        private bool SaveSale()
        {
            Sales_Client sc = new Sales_Client();
            try
            {
                sc.SaleID = txtSaleID.Text;
                sc.EventID = txtEventID.Text;
                sc.E_Name = textE_name.Text;
                sc.N_tickets = txtNTickets.Text;
                sc.T_type = textTType.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (addSale)
            {
                SubmitSale(sc);
                listBox1.Items.Add(sc);
            }
            else
            {
                UpdateSale(sc);
                listBox1.Items[currentSale] = sc;
            }
            return true;
        }

        private void SubmitSale(Sales_Client sc)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Sales VALUES (@SaleID, @EventID, @NTickets, @Ssn, @TType)";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Ssn", cn_ssn);
            cmd.Parameters.AddWithValue("@SaleID", sc.SaleID);
            cmd.Parameters.AddWithValue("@EventID", sc.EventID);
            cmd.Parameters.AddWithValue("@NTickets", sc.N_tickets);
            cmd.Parameters.AddWithValue("@TType", sc.T_type);
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
        private void UpdateSale(Sales_Client sc)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateSalesC @SaleID,@EventID,@NTickets,@TType,@Ssn";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SaleID", sc.SaleID);
            cmd.Parameters.AddWithValue("@EventID", sc.EventID);
            cmd.Parameters.AddWithValue("@NTickets", sc.N_tickets);
            cmd.Parameters.AddWithValue("@TType", sc.T_type);
            cmd.Parameters.AddWithValue("@Ssn", cn_ssn);
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
                    RemoveSale(((Sales_Client)listBox1.SelectedItem));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentSale == listBox1.Items.Count)
                    currentSale = listBox1.Items.Count - 1;
                if (currentSale == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more clients");
                }
                else
                {
                    ShowSales();
                }
            }
        }
        private void RemoveSale(Sales_Client s)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteSale @Sale_ID,@Event_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Sale_ID", s.SaleID);
            cmd.Parameters.AddWithValue("@Event_ID", s.EventID);
            cmd.Parameters.AddWithValue("@Ssn", cn_ssn);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete sale in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void textE_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
