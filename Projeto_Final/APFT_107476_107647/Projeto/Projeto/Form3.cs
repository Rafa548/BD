using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto
{
    public partial class Form3 : Form
    {

        private SqlConnection cn;
        private int currentSale;
        private int currentSchedule;
        private int currentAD;
        private Boolean adding;
        private Boolean addingSch;
        private Boolean addingAD;
        private string ev_id;
        public Form3(Event_L ev)
        {
            InitializeComponent();

            ev_id = ev.EventID;
            loadSalesToolStripMenuItem_Click(null, null);
            loadScheduleToolStripMenuItem_Click(null, null);
            loadADToolStripMenuItem_Click(null, null);


            label15.Text = ev.EventName + " [" + ev_id + "]";
            label15.Font = new Font("Courier New", 24, FontStyle.Bold);

            string id = "ID";
            string name = "Client_SSN";
            string location = "N_Tickets";
            string supplier = "Type";

            string ScheduleId = "S_ID";
            string ActivityId = "A_ID";
            string Name = "A_Name";
            string Day = "Day";
            string Hour = "Hour";

            string AD_ID = "AD_ID";
            string D = "Duration";
            string C = "Cost";

            string formattedString = string.Format("{0,-5} {1,-15} {2,-15} {3}", id, name, location, supplier);
            string formattedString2 = string.Format($"{ScheduleId,-5} {ActivityId,-5} {Name,-25} {Day,-5} {Hour}");
            string formattedString3 = string.Format($"{AD_ID,-5} {D,-10} {C}");

            listBox1.Font = new Font("Courier New", 8, FontStyle.Bold);
            listBox2.Font = new Font("Courier New", 8, FontStyle.Bold);
            listBox3.Font = new Font("Courier New", 8, FontStyle.Bold);

            label12.Font = new Font("Courier New", 8, FontStyle.Bold);
            label12.Text = formattedString;
            label14.Font = new Font("Courier New", 8, FontStyle.Bold);
            label14.Text = formattedString2;
            label4.Font = new Font("Courier New", 8, FontStyle.Bold);
            label4.Text = formattedString3;
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

        private void loadSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT Sale_ID,Event_ID,N_tickets,Ssn,T_type " + "FROM Sales Where Event_ID = @ID ;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ev_id);
            cmd.Connection = cn;
            SqlDataReader reader = cmd.ExecuteReader();
            listBox2.Items.Clear();

            while (reader.Read())
            {
                Sales sale = new Sales();
                sale.SaleID = reader["Sale_ID"].ToString();
                sale.EventID = reader["Event_ID"].ToString();
                sale.N_tickets = reader["N_tickets"].ToString();
                sale.Ssn = reader["Ssn"].ToString();
                sale.T_type = reader["T_type"].ToString();

                listBox2.Items.Add(sale);
            }

            cn.Close();

            currentSale = 0;
        }

        private void loadScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT S.Schedule_ID AS S_ID,S.Event_ID as E_ID, A.Activity_ID AS act_ID, A.Name AS Activity_name, S.Day, S.Hour, A.Objective, A.Cost " +
                                             "FROM Schedule AS S " +
                                             "JOIN Activity AS A ON S.Activity_Id = A.Activity_ID Where S.Event_ID = @ID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ev_id);
            cmd.Connection = cn;
            SqlDataReader reader = cmd.ExecuteReader();
            listBox3.Items.Clear();

            while (reader.Read())
            {
                Schedule schedule = new Schedule();
                schedule.ScheduleId = reader["S_ID"].ToString();
                schedule.ActivityId = reader["act_ID"].ToString();
                schedule.Name = reader["Activity_name"].ToString();
                schedule.Day = reader["Day"].ToString();
                schedule.Hour = reader["Hour"].ToString();
                schedule.Objective = reader["Objective"].ToString();
                schedule.Cost = reader["Cost"].ToString();

                listBox3.Items.Add(schedule);
            }

            cn.Close();

            currentSchedule = 0;
        }

        private void loadADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * FROM AD join Event on Event.Event_ID = AD.Event_ID Where Event.Event_ID = @ID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ID", ev_id);
            cmd.Connection = cn;
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Ad ad = new Ad();
                ad.EventID = reader["Event_Id"].ToString();
                ad.Duration = reader["Duration"].ToString();
                ad.AdID = reader["AD_ID"].ToString();
                ad.Cost = reader["Cost"].ToString();

                listBox1.Items.Add(ad);
            }

            cn.Close();

            currentAD = 0;
        }


        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentAD = listBox1.SelectedIndex;
                ShowAD();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                currentSale = listBox2.SelectedIndex;
                ShowSale();
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                currentSchedule = listBox3.SelectedIndex;
                ShowSchedule();
            }
        }


        public void ShowSale()
        {
            if (listBox2.Items.Count == 0 | currentSale < 0)
                return;
            Sales sale = (Sales)listBox2.Items[currentSale];
            txtSaleID.Text = sale.SaleID;
            txtSale_SSN.Text = sale.Ssn;
            txtSale_nt.Text = sale.N_tickets;
            txtSaleType.Text = sale.T_type;
        }

        public void ShowSchedule()
        {
            if (listBox3.Items.Count == 0 | currentSchedule < 0)
                return;
            Schedule sc = (Schedule)listBox3.Items[currentSchedule];
            textA_ID.Text = sc.ActivityId;
            textA_Name.Text = sc.Name;
            textS_ID.Text = sc.ScheduleId;
            text_Day.Text = sc.Day;
            text_Hour.Text = sc.Hour;
            text_Obj.Text = sc.Objective;
            text_Cost.Text = sc.Cost;
        }

        public void ShowAD()
        {
            if (listBox1.Items.Count == 0 | currentAD < 0)
                return;
            Ad ad = (Ad)listBox1.Items[currentAD];
            txtAD_ID.Text = ad.AdID;
            txtAD_D.Text = ad.Duration;
            txtAD_C.Text = ad.Cost;
        }


        private void bttnAdd_Click(object sender, EventArgs e)
        {
            adding = true;
            ClearFields();
            HideButtons();
            listBox2.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        //add
        {
            addingSch = true;
            ClearFields2();
            HideButtons2();
            listBox3.Enabled = false;
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            listBox2.Enabled = true;
            if (listBox2.Items.Count > 0)
            {
                ShowSale();
            }
            else
            {
                ClearFields();
                LockControls();
            }
            ShowButtons();
        }

        private void button3_Click(object sender, EventArgs e)
        //cancel
        {
            listBox3.Enabled = true;
            if (listBox3.Items.Count > 0)
            {
                ShowSchedule();
            }
            else
            {
                ClearFields2();
                LockControls2();
            }
            ShowButtons2();
        }

        private void bttnEdit_Click(object sender, EventArgs e)
        {
            currentSale = listBox2.SelectedIndex;
            if (currentSale < 0)
            {
                MessageBox.Show("Please select a Sale to edit");
                return;
            }
            adding = false;
            HideButtons_1();
            listBox2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        //edit
        {
            currentSchedule = listBox3.SelectedIndex;
            if (currentSchedule < 0)
            {
                MessageBox.Show("Please select a Sale to edit");
                return;
            }
            addingSch = false;
            HideButtons2_2();
            listBox3.Enabled = false;
        }


        private void bttnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            listBox2.Enabled = true;
            int idx = listBox2.FindString(txtSaleID.Text);
            listBox2.SelectedIndex = idx;
            ShowButtons();
            loadSalesToolStripMenuItem_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ok
            try
            {
                SaveSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox3.Enabled = true;
            int idx = listBox1.FindString(textS_ID.Text);
            listBox3.SelectedIndex = idx;
            loadScheduleToolStripMenuItem_Click(null, null);
            ShowButtons2();
        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                try
                {
                    RemoveSales(((Sales)listBox2.SelectedItem).SaleID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                if (currentSale == listBox2.Items.Count)
                    currentSale = listBox2.Items.Count - 1;
                if (currentSale == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more Sales");
                }
                else
                {
                    ShowSale();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        //delete
        {
            if (listBox3.SelectedIndex > -1)
            {
                try
                {
                    RemoveSchedule(((Schedule)listBox3.SelectedItem).ScheduleId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                if (currentSchedule == listBox3.Items.Count)
                    currentSchedule = listBox3.Items.Count - 1;
                if (currentSchedule == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more Schedules");
                }
                else
                {
                    ShowSchedule();
                }
            }
        }


        private void UpdateSales(Sales sale)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateSales @SaleID,@EventId,@N_tickets,@Ssn,@T_type;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SaleID", sale.SaleID);
            cmd.Parameters.AddWithValue("@EventId", ev_id);
            cmd.Parameters.AddWithValue("@N_tickets", sale.N_tickets);
            cmd.Parameters.AddWithValue("@Ssn", sale.Ssn);
            cmd.Parameters.AddWithValue("@T_type", sale.T_type);
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


        private void UpdateSchedule(Schedule sch)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateSchedule @EventId,@Day,@A_id,@Hour,@ScheduleID;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Day", sch.Day);
            cmd.Parameters.AddWithValue("@EventId", ev_id);
            cmd.Parameters.AddWithValue("@A_id", sch.ActivityId);
            cmd.Parameters.AddWithValue("@Hour", sch.Hour);
            cmd.Parameters.AddWithValue("@ScheduleID", sch.ScheduleId);
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

        private void UpdateAd(Ad ad)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC UpdateAd @Duration,@Ad_Id,@Cost,@Event_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Duration", ad.Duration);
            cmd.Parameters.AddWithValue("@Ad_ID", ad.AdID);
            cmd.Parameters.AddWithValue("@Cost", ad.Cost);
            cmd.Parameters.AddWithValue("@Event_ID", ev_id);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update ad in the database.\nERROR MESSAGE:\n" + ex.Message);
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

        private void RemoveSales(string SaleID)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteSale @SaleID,@Event_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SaleID", SaleID);
            cmd.Parameters.AddWithValue("Event_ID", ev_id);
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

        private void RemoveAd(string AdID)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteAd @AdID,@E_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@AdID", AdID);
            cmd.Parameters.AddWithValue("@E_ID", ev_id);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete ad from the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void RemoveSchedule(string SCHID)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Exec dbo.DeleteSchedule @SCHID,@E_ID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SCHID", SCHID);
            cmd.Parameters.AddWithValue("@E_ID", ev_id);
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

        private bool SaveSales()
        {
            Sales sale = new Sales();
            try
            {
                sale.SaleID = txtSaleID.Text;
                sale.N_tickets = txtSale_nt.Text;
                sale.Ssn = txtSale_SSN.Text;
                sale.T_type = txtSaleType.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (adding)
            {
                SubmitSales(sale);
                listBox2.Items.Add(sale);
            }
            else
            {
                UpdateSales(sale);
                listBox2.Items[currentSale] = sale;
            }
            return true;
        }

        private void SaveAd()
        {
            Ad ad = new Ad();
            try
            {
                ad.AdID = txtAD_ID.Text;
                ad.Duration = txtAD_D.Text;
                ad.Cost = txtAD_C.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (addingAD)
            {
                SubmitAd(ad);
                listBox1.Items.Add(ad);
            }
            else
            {
                UpdateAd(ad);
                listBox1.Items[currentAD] = ad;
            }
        }

        private bool SaveSchedule()
        {
            Schedule sch = new Schedule();
            try
            {
                sch.ActivityId = textA_ID.Text;
                sch.Name = textA_Name.Text;
                sch.ScheduleId = textS_ID.Text;
                sch.Day = text_Day.Text;
                sch.Hour = text_Hour.Text;
                sch.Objective = text_Obj.Text;
                sch.Cost = text_Cost.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (addingSch)
            {
                SubmitSchedule(sch);
                listBox3.Items.Add(sch);
            }
            else
            {
                UpdateSchedule(sch);
                listBox3.Items[currentSchedule] = sch;
            }
            return true;
        }

        private void SubmitSales(Sales sale)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Sales (Sale_ID, Event_ID, N_tickets, Ssn, T_type) " +
                              "VALUES (@SaleID, @EventID, @NTickets, @Ssn, @TType)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SaleID", sale.SaleID);
            cmd.Parameters.AddWithValue("@EventID", ev_id);
            cmd.Parameters.AddWithValue("@NTickets", sale.N_tickets);
            cmd.Parameters.AddWithValue("@Ssn", sale.Ssn);
            cmd.Parameters.AddWithValue("@TType", sale.T_type);
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


        private void SubmitSchedule(Schedule sch)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Schedule (Event_ID,Day,Activity_Id,Hour,Schedule_ID) " +
                              "VALUES (@SaleID, @EventID, @NTickets, @Ssn, @TType)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@SaleID", ev_id);
            cmd.Parameters.AddWithValue("@EventID", sch.Day);
            cmd.Parameters.AddWithValue("@NTickets", sch.ActivityId);
            cmd.Parameters.AddWithValue("@Ssn", sch.Hour);
            cmd.Parameters.AddWithValue("@TType", sch.ScheduleId);
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

        public void SubmitAd(Ad ad)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Ad (Event_ID,Duration,Ad_ID,Cost) " +
                              "VALUES (@EventID,@Duration,@AdID,@Cost)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EventID", ev_id);
            cmd.Parameters.AddWithValue("@Duration", ad.Duration);
            cmd.Parameters.AddWithValue("@AdID", ad.AdID);
            cmd.Parameters.AddWithValue("@Cost", ad.Cost);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update ad in the database.\nERROR MESSAGE:\n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }



        public void LockControls2()
        {
            textA_ID.ReadOnly = true;
            textA_Name.ReadOnly = true;
            textS_ID.ReadOnly = true;
            text_Day.ReadOnly = true;
            text_Hour.ReadOnly = true;
            text_Obj.ReadOnly = true;
            text_Cost.ReadOnly = true;
        }

        public void UnlockControls2()
        {

            textA_ID.ReadOnly = false;
            textA_Name.ReadOnly = true;
            textS_ID.ReadOnly = false;
            text_Day.ReadOnly = false;
            text_Hour.ReadOnly = false;
            text_Obj.ReadOnly = true;
            text_Cost.ReadOnly = true;
        }

        public void ShowButtons2()
        {
            LockControls2();
            btn_add2.Visible = true;
            btn_delete2.Visible = true;
            btn_edit2.Visible = true;
            btn_ok2.Visible = false;
            btn_cancel2.Visible = false;
        }

        public void ClearFields2()
        {
            textA_ID.Text = "";
            textA_Name.Text = "";
            textS_ID.Text = "";
            text_Day.Text = "";
            text_Hour.Text = "";
            text_Obj.Text = "";
            text_Cost.Text = "";
        }

        public void HideButtons2()
        {
            UnlockControls2();
            btn_add2.Visible = false;
            btn_delete2.Visible = false;
            btn_edit2.Visible = false;
            btn_ok2.Visible = true;
            btn_cancel2.Visible = true;
        }

        public void HideButtons2_2()
        {
            UnlockControls2_2();
            btn_add2.Visible = false;
            btn_delete2.Visible = false;
            btn_edit2.Visible = false;
            btn_ok2.Visible = true;
            btn_cancel2.Visible = true;
        }

        public void UnlockControls2_2()
        {

            textA_ID.ReadOnly = false;
            textA_Name.ReadOnly = true;
            textS_ID.ReadOnly = true;
            text_Day.ReadOnly = false;
            text_Hour.ReadOnly = false;
            text_Obj.ReadOnly = true;
            text_Cost.ReadOnly = true;
        }

        public void LockControls()
        {
            txtSaleID.ReadOnly = true;
            txtSale_SSN.ReadOnly = true;
            txtSale_nt.ReadOnly = true;
            txtSaleType.ReadOnly = true;
        }

        public void UnlockControls()
        {
            txtSaleID.ReadOnly = false;
            txtSale_SSN.ReadOnly = false;
            txtSale_nt.ReadOnly = false;
            txtSaleType.ReadOnly = false;
        }

        public void ShowButtons()
        {
            LockControls();
            bttnAdd.Visible = true;
            bttnDelete.Visible = true;
            bttnEdit.Visible = true;
            bttnOK.Visible = false;
            bttnCancel.Visible = false;
        }

        public void ClearFields()
        {
            txtSaleID.Text = "";
            txtSale_SSN.Text = "";
            txtSale_nt.Text = "";
            txtSaleType.Text = "";
        }

        public void HideButtons()
        {
            UnlockControls();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
        }
        public void HideButtons_1()
        {
            UnlockControls_1();
            bttnAdd.Visible = false;
            bttnDelete.Visible = false;
            bttnEdit.Visible = false;
            bttnOK.Visible = true;
            bttnCancel.Visible = true;
        }

        public void UnlockControls_1()
        {
            txtSaleID.ReadOnly = true;
            txtSale_SSN.ReadOnly = true;
            txtSale_nt.ReadOnly = false;
            txtSaleType.ReadOnly = false;
        }

        public void LockControlsAD()
        {
            txtAD_ID.ReadOnly = true;
            txtAD_D.ReadOnly = true;
            txtAD_C.ReadOnly = true;
        }

        public void UnlockControlsAD()
        {
            txtAD_ID.ReadOnly = false;
            txtAD_D.ReadOnly = false;
            txtAD_C.ReadOnly = false;
        }

        public void ShowButtonsAD()
        {
            LockControlsAD();
            AD_ADD.Visible = true;
            AD_Delete.Visible = true;
            AD_EDIT.Visible = true;
            AD_OK.Visible = false;
            AD_CANCEL.Visible = false;
        }

        public void ClearFieldsAD()
        {
            txtAD_ID.Text = "";
            txtAD_D.Text = "";
            txtAD_C.Text = "";
        }

        public void HideButtonsAD()
        {
            UnlockControlsAD();
            AD_ADD.Visible = false;
            AD_Delete.Visible = false;
            AD_EDIT.Visible = false;
            AD_OK.Visible = true;
            AD_CANCEL.Visible = true;
        }

        public void UnlockControlsAD_1()
        {
            txtAD_ID.ReadOnly = true;
            txtAD_D.ReadOnly = false;
            txtAD_C.ReadOnly = false;
        }

        public void HideButtonsAD_1()
        {
            UnlockControlsAD_1();
            AD_ADD.Visible = false;
            AD_Delete.Visible = false;
            AD_EDIT.Visible = false;
            AD_OK.Visible = true;
            AD_CANCEL.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            form1.TopLevel = false;
            form1.FormBorderStyle = FormBorderStyle.None;
            form1.Dock = DockStyle.Fill;
            panel12.Controls.Clear();
            panel12.Controls.Add(form1);
            form1.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AD_Delete_Click(object sender, EventArgs e)
        {
            //delete ad
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveAd(((Ad)listBox1.SelectedItem).AdID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentAD == listBox1.Items.Count)
                    currentAD = listBox3.Items.Count - 1;
                if (currentAD == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more ads");
                }
                else
                {
                    ShowAD();
                }
            }
        }

        private void AD_ADD_Click(object sender, EventArgs e)
        {
            //add ad
            addingAD = true;
            ClearFieldsAD();
            HideButtonsAD();
            listBox1.Enabled = false;
        }

        private void AD_CANCEL_Click(object sender, EventArgs e)
        {
            //cancel ad
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                ShowAD();
            }
            else
            {
                ClearFieldsAD();
                LockControlsAD();
            }
            ShowButtonsAD();
        }

        private void AD_EDIT_Click(object sender, EventArgs e)
        {
            //edit ad
            currentAD = listBox1.SelectedIndex;
            if (currentAD < 0)
            {
                MessageBox.Show("Please select a Ad to edit");
                return;
            }
            addingAD = false;
            HideButtonsAD_1();
            listBox1.Enabled = false;
        }

        private void AD_OK_Click(object sender, EventArgs e)
        {
            //ok ad
            try
            {
                SaveAd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(txtAD_ID.Text);
            listBox1.SelectedIndex = idx;
            ShowButtonsAD();


        }
    }
}
