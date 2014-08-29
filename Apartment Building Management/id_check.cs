using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Apartment_Building_Management
{
    public partial class id_check : Form
    {
        public id_check()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\databases\new_version\_abmDB.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection conn = new SqlConnection(connectionString);
            string insertStr = "insert into date_check values (@ID, @date)";
            SqlCommand command = new SqlCommand(insertStr, conn);

            SqlParameter id = new SqlParameter();
            id.ParameterName = "@ID";
            id.SqlDbType = SqlDbType.TinyInt;
            id.Value = textBox1.Text;

            SqlParameter _date = new SqlParameter();
            _date.ParameterName = "@date";
            _date.SqlDbType = SqlDbType.Date;
            _date.Value = dateTimePicker1.Value;

            command.Parameters.Add(id);
            command.Parameters.Add(_date);

            conn.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void id_check_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";
        } 
    }
}
