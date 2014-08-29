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
    public partial class filteredFormBasedOnLocationAndTime : filteredFormBasedOnLocation
    {
        private string[] name;
        private SqlDbType[] type;
        private string[] column;
        private string[] value;
        private string selectQuery;
        private int commandParameters;

        public string[] _Name
        {
            get { return name; }
            set { name = value; }
        }

        public SqlDbType[] Type
        {
            get { return type; }
            set { type = value; }
        }

        public string[] Column
        {
            get { return column; }
            set { column = value; }
        }

        public ComboBox MonthComboBox
        {
            get { return monthComboBox; }
            set { monthComboBox = value; }
        }

        public ComboBox YearComboBox
        {
            get { return yearComboBox; }
            set { yearComboBox = value; }
        }

        public filteredFormBasedOnLocationAndTime(string selectQuery, int commandParameters)
        {
            InitializeComponent();

            name = new string[commandParameters];
            type = new SqlDbType[commandParameters];
            column = new string[commandParameters];
            value = new string[commandParameters];
            this.selectQuery = selectQuery;
            this.commandParameters = commandParameters;

            string[] monthNames = { "Ιανουάριος", "Φεβρουάριος", "Μάρτιος", "Απρίλιος", "Μάιος", "Ιούνιος", "Ιούλιος", "Αύγουστος", "Σεπτέμβριος", "Οκτώβριος",
                                  "Νοέμβριος", "Δεκέμβριος" };
            monthComboBox.Items.AddRange(monthNames);

            for (int i = 2010; i < 2050; i++)
            {
                yearComboBox.Items.Add(i);
            }

            monthComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            yearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void filteredFormBasedOnLocationAndTime_Load(object sender, EventArgs e)
        {

        }

        public override void areaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            addressComboBox.Enabled = true;
            areaChanged();
            monthComboBox.SelectedIndex = -1;
            yearComboBox.SelectedIndex = -1;
        }

        public override void addressComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            monthComboBox.SelectedIndex = -1;
            yearComboBox.SelectedIndex = -1;
        }

        private void yearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void monthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (monthComboBox.SelectedIndex != -1 && areaComboBox.SelectedIndex != -1)
            {
                string queryString = "select buildingID from buildings where bArea = @Area and bAddress = @Address";
                string areaParameter = areaComboBox.SelectedItem.ToString();
                string addressParameter = addressComboBox.SelectedItem.ToString();
                SelectedID = RetrieveIdBasedOnLocation(areaParameter, addressParameter, queryString);

                adapterInitialization();

                UnfilteredDataGridView.Show();
                whileNotEditingControlsStatus(true);
                GetData();
            }
            else
            {
                UnfilteredDataGridView.Hide();
            }
        }

        public override void adapterInitialization()
        {
            if(name.Length == 3)
            {
                value[0] = SelectedID;
                value[1] = monthComboBox.SelectedItem.ToString();
                value[2] = yearComboBox.SelectedItem.ToString();
            }

            SqlConnection connection = new SqlConnection(ConnectionString);

            SqlCommand selectCommand = new SqlCommand();
            selectCommand.CommandText = selectQuery;
            selectCommand.Connection = connection;

            SqlParameter[] parameter = new SqlParameter[commandParameters];

            for(int i=0; i < commandParameters; i++)
            {
                parameter[i] = new SqlParameter();
                parameter[i].ParameterName = name[i];
                parameter[i].SqlDbType = type[i];
                parameter[i].SourceColumn = column[i];
                parameter[i].Value = value[i];
                selectCommand.Parameters.Add(parameter[i]);
            }

            Adapter = new SqlDataAdapter();
            Adapter.SelectCommand = selectCommand;
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(Adapter);

            whileEditingControlsStatus(false);
            whileNotEditingControlsStatus(true);
        }
    }
}
