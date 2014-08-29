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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void buildingsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            string queryString = "select ID as 'Building ID', b_address as Address, area_ID as Area from tbl_buildings order by ID";
            UnfilteredForm buildings = new UnfilteredForm(queryString);
            buildings.GetData();
            buildings.whileEditingControlsStatus(false);
            buildings.Show();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void categoriesOfCostBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            string queryString = "select costCategory as 'Cost Category', costDescription as 'Cost Description' from costPredefinedItems";
            UnfilteredForm categoriesOfCost = new UnfilteredForm(queryString);
            categoriesOfCost.GetData();
            categoriesOfCost.whileEditingControlsStatus(false);
            categoriesOfCost.Show();
        }

        private void apartmentsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            filteredFormBasedOnLocation apartments = new filteredFormBasedOnLocation();
            string queryString = "select distinct bArea from buildings order by bArea";
            apartments.fillTheComboBox(queryString, apartments.areaComboBox);

            apartments.whileEditingControlsStatus(false);
            apartments.whileNotEditingControlsStatus(true);
            apartments.EditBtn.Hide();
            apartments.UnfilteredDataGridView.Hide();
            apartments.addressComboBox.Enabled = false;
            apartments.Show();
        }

        private void dapanesBtn_Click(object sender, EventArgs e)
        {
            this.Hide();

            string[] name = { "@buildingID", "@theMonth", "@theYear" };
            SqlDbType[] type = { SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.VarChar };
            string[] column = { "buildingID", "theMonth", "theYear" };
            int commandParameters = name.Length;

            string selectQuery = "select buildingID as 'Building ID', theMonth as Month, theYear as Year, costCategory as 'Cost Category'" +
            ", costDescription as 'Cost Description', cost as Cost from dapanes where buildingID = @buildingID and theMonth = @theMonth and theYear = @theYear";

            filteredFormBasedOnLocationAndTime dapanes = new filteredFormBasedOnLocationAndTime(selectQuery, commandParameters);

            dapanes._Name = name;
            dapanes.Type = type;
            dapanes.Column = column;

            string queryString = "select distinct bArea from buildings order by bArea";
            dapanes.fillTheComboBox(queryString, dapanes.areaComboBox);

            dapanes.whileEditingControlsStatus(false);
            dapanes.whileNotEditingControlsStatus(true);
            dapanes.EditBtn.Hide();
            dapanes.UnfilteredDataGridView.Hide();
            dapanes.addressComboBox.Enabled = false;
            dapanes.Show();
        }

        private void aggregateBtn_Click(object sender, EventArgs e)
        {
            this.Hide();

            string[] name = { "@buildingID", "@theMonth", "@theYear" };
            SqlDbType[] type = { SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.VarChar };
            string[] column = { "buildingID", "theMonth", "theYear" };
            int commandParameters = name.Length;

            string selectQuery =
                "select [Apartment ID], sum([General Proportion]) as 'General Proportion', format(sum([General Cost]), 'C', 'el-GR') as 'General Cost', " + 
                "sum([Elevator Proportion]) as 'Elevator Proportion', format(sum([Elevator Cost]), 'C', 'el-GR') as 'Elevator Cost', " + 
                "format(sum([Total Cost]), 'C', 'el-GR') as 'Total Cost' from (" + 
                "select [Apartment ID], [General Proportion], [General Cost] as 'General Cost', " +
                "[Elevator Proportion], [Elevator Cost] as 'Elevator Cost', " +
                "[General Cost] + [Elevator Cost] as 'Total Cost' from (" +
                    "select apartmentID as 'Apartment ID', " +
                        "generalProportion as 'General Proportion', " +
                        "(select convert(decimal(7,2), (sum(cost) * ( A1.generalProportion / 1000 ))) as generalCost " +
                        "from dapanes A2 " +
                        "where theMonth = @theMonth and theYear = @theYear and A2.buildingID = A1.buildingID and costCategory = N'Γενικά κοινόχρηστα') as 'General Cost', " +
                        "elevatorProportion as 'Elevator Proportion', " +
                        "(select convert(decimal(7,2), (sum(cost) * ( A1.elevatorProportion / 1000 ))) as elevatorCost " +
                        "from dapanes A3 " +
                        "where theMonth = @theMonth and theYear = @theYear and A3.buildingID = A1.buildingID and costCategory = N'Ασανσέρ') as 'Elevator Cost' " +
                    "from Apartments A1 " +
                    "where A1.buildingID = @buildingID) as table1) as table2 group by [Apartment ID] with rollup";

            filteredFormBasedOnLocationAndTime aggregate = new filteredFormBasedOnLocationAndTime(selectQuery, commandParameters);

            aggregate._Name = name;
            aggregate.Type = type;
            aggregate.Column = column;

            string queryString = "select distinct bArea from buildings order by bArea";
            aggregate.fillTheComboBox(queryString, aggregate.areaComboBox);

            aggregate.whileEditingControlsStatus(false);
            aggregate.whileNotEditingControlsStatus(true);
            aggregate.EditBtn.Hide();
            aggregate.UnfilteredDataGridView.Hide();
            aggregate.addressComboBox.Enabled = false;
            aggregate.Show();
        }

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            id_check check1 = new id_check();
            check1.Show();
        }
    }
}
