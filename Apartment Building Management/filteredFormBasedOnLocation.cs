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
    public partial class filteredFormBasedOnLocation : UnfilteredForm
    {
        private string selectedID;

        public string SelectedID
        {
            get { return selectedID; }
            set { selectedID = value; }
        }

        public filteredFormBasedOnLocation()
        {
            InitializeComponent();
            areaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            addressComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void filteredFormBasedOnLocation_Load(object sender, EventArgs e)
        {

        }

        public void fillTheComboBox(string queryString, ComboBox comboBox)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);

            using (connection)
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[0]);
                    }
                }
            }
        }

        public void fillTheComboBox(string queryString, ComboBox comboBox, string filterItem)
        {
            addressComboBox.ResetText();
            addressComboBox.Items.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = queryString;
                command.Connection = connection;

                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@parameter";
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Value = filterItem;

                command.Parameters.Add(parameter);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader[0]);
                    }
                }
            }
        }

        public virtual void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            addressComboBox.Enabled = true;
            areaChanged();
        }

        public void areaChanged()
        {
            whileEditingControlsStatus(false);
            whileNotEditingControlsStatus(true);
            EditBtn.Hide();
            UnfilteredDataGridView.Hide();
            string queryString = "select bAddress from buildings where bArea = @parameter order by bAddress";
            fillTheComboBox(queryString, addressComboBox, areaComboBox.SelectedItem.ToString());
        }

        public virtual void addressComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queryString = "select buildingID from buildings where bArea = @Area and bAddress = @Address";
            string areaParameter = areaComboBox.SelectedItem.ToString();
            string addressParameter = addressComboBox.SelectedItem.ToString();
            SelectedID = RetrieveIdBasedOnLocation(areaParameter, addressParameter, queryString);
            adapterInitialization();
        }

        public virtual void adapterInitialization()
        {
            UnfilteredDataGridView.Show();
            whileNotEditingControlsStatus(true);

            /*string queryString = "select buildingID from buildings where bArea = @Area and bAddress = @Address";
            string areaParameter = areaComboBox.SelectedItem.ToString();
            string addressParameter = addressComboBox.SelectedItem.ToString();
            selectedID = RetrieveIdBasedOnLocation(areaParameter, addressParameter, queryString);*/

            string selectQuery = "select buildingID as 'Building ID', owner as Owner, apartmentID as 'Apartment ID', generalProportion as 'General Proportion (‰)', " +
            "elevatorProportion as 'Elevator Proportion (‰)' from Apartments where buildingID = @buildingID order by apartmentID";

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = selectQuery;
            selectCommand.Connection = connection;

            SqlParameter parameter1 = new SqlParameter();
            parameter1.ParameterName = "@buildingID";
            parameter1.SqlDbType = SqlDbType.VarChar;
            parameter1.SourceColumn = "buildingID";
            parameter1.Value = selectedID;

            selectCommand.Parameters.Add(parameter1);

            Adapter = new SqlDataAdapter();
            Adapter.SelectCommand = selectCommand;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(Adapter);

            GetData();

            UnfilteredDataGridView.Columns[0].Visible = false;
            whileEditingControlsStatus(false);

            whileNotEditingControlsStatus(true);
        }

        public string RetrieveIdBasedOnLocation(string areaParameter, string addressParameter, string queryString)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = queryString;

                SqlParameter param_area = new SqlParameter();
                param_area.ParameterName = "@Area";
                param_area.SqlDbType = SqlDbType.NVarChar;
                param_area.Value = areaParameter;

                SqlParameter param_address = new SqlParameter();
                param_address.ParameterName = "@Address";
                param_address.SqlDbType = SqlDbType.NVarChar;
                param_address.Value = addressParameter;

                command.Parameters.Add(param_area);
                command.Parameters.Add(param_address);

                connection.Open();
                return command.ExecuteScalar().ToString();
            }
        }

        public override void saveBtn_Click(object sender, EventArgs e)
        {
            string[] columnAutomaticValues = { selectedID };
            save(columnAutomaticValues);
        }

        public void save(string[] columnAutomaticValues)
        {
            string message = "Are you sure you want to update the database with changes?";
            string caption = "Update confirmation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = (DataTable)BindingSource1.DataSource;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row.RowState.ToString() != "Deleted")
                        {
                            for (int i = 0; i < columnAutomaticValues.Length; i++)
                            {
                                if (row[i].ToString().Length == 0)
                                {
                                    row[i] = columnAutomaticValues[i];
                                }
                            }
                        }
                    }

                    int r = Adapter.Update(dt);

                    whileEditingControlsStatus(false);
                    MessageBox.Show("Saved! " + r + " row(s) affected.");
                    //GetData(adapter.SelectCommand);
                    whileNotEditingControlsStatus(true);
                }
                catch (SqlException sqlEx)
                {
                    switch (sqlEx.Number)
                    {
                        case 2627:
                            {
                                MessageBox.Show(sqlEx.Message);
                                break;
                            }
                        case 515:
                            {
                                MessageBox.Show(sqlEx.Message);
                                break;
                            }
                        case 8152:
                            {
                                MessageBox.Show(sqlEx.Message);
                                break;
                            }
                        default:
                            {
                                MessageBox.Show(sqlEx.Message);
                                break;
                            }
                    }
                }
            }
            else if (result == DialogResult.No)
            {
                try
                {
                    MessageBox.Show("Not Saved!");
                    GetData();
                    whileEditingControlsStatus(false);
                    whileNotEditingControlsStatus(true);
                }
                catch
                {
                    MessageBox.Show("Not Saved!");
                    MessageBox.Show("Reloading from database failed");
                }
            }
        }
    }
}
