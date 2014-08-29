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
    public partial class UnfilteredForm : Form
    {
        private string connectionString;
        private Control[] whileNotEditingControls;
        private Control[] whileEditingControls;
        private BindingSource bindingSource1;
        private SqlDataAdapter adapter;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public int MyProperty { get; set; }

        public DataGridView UnfilteredDataGridView
        {
            get { return unfilteredDataGridView; }
            set { unfilteredDataGridView = value; }
        }

        public Button EditBtn
        {
            get { return editBtn; }
            set { editBtn = value; }
        }

        public SqlDataAdapter Adapter
        {
            get { return adapter; }
            set { adapter = value; }
        }

        public BindingSource BindingSource1
        {
            get { return bindingSource1; }
            set { bindingSource1 = value; }
        }

        public UnfilteredForm()
        {
            InitializeComponent();
            connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\databases\new_version\_abmDB.mdf;Integrated Security=True;Connect Timeout=30";

            initializeControlGroups();

            bindingSource1 = new BindingSource();
            unfilteredDataGridView.DataSource = bindingSource1;
        }

        public UnfilteredForm(string queryString)
        {
            InitializeComponent();
            connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\databases\new_version\_abmDB.mdf;Integrated Security=True;Connect Timeout=30";

            initializeControlGroups();

            bindingSource1 = new BindingSource();
            unfilteredDataGridView.DataSource = bindingSource1;

            adapter = new SqlDataAdapter(queryString, connectionString);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
        }

        public void initializeControlGroups()
        {
            whileNotEditingControls = new Control[2];
            whileEditingControls = new Control[3];

            whileNotEditingControls[0] = editBtn;
            whileNotEditingControls[1] = exitBtn;
            whileEditingControls[0] = saveBtn;
            whileEditingControls[1] = deleteBtn;
            whileEditingControls[2] = cancelBtn;
        }

        private void unfilteredDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                MessageBox.Show("lala");
            }
            else { MessageBox.Show("eisai paparas"); }
        }

        private void UnfilteredForm_Load(object sender, EventArgs e)
        {

        }

        public void GetData()
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            bindingSource1.DataSource = dt;
        }

        public void whileNotEditingControlsStatus(bool displayStatus)
        {
            for (int i = 0; i < whileNotEditingControls.Length; i++)
            {
                if (displayStatus)
                {
                    whileNotEditingControls[i].Show();
                }
                else
                {
                    whileNotEditingControls[i].Hide();
                }
            }
        }

        public void whileEditingControlsStatus(bool displayStatus)
        {
            if (displayStatus)
            {
                unfilteredDataGridView.ReadOnly = false;

                foreach (Control c in whileEditingControls)
                {
                    c.Show();
                }

                //find which column is the first visible column and set the cursor of the DataGridView to point there.
                int firstVisibleColumnIndex = 0;

                try
                {
                    for (int columnIndex = 0; columnIndex < unfilteredDataGridView.Columns.Count; columnIndex++)
                    {
                        if (unfilteredDataGridView.Columns[columnIndex].Visible == true)
                        {
                            firstVisibleColumnIndex = columnIndex;
                            break;
                        }
                    }
                    unfilteredDataGridView.CurrentCell = unfilteredDataGridView[firstVisibleColumnIndex, unfilteredDataGridView.Rows.Count - 1];
                }
                catch
                {
                    MessageBox.Show("go on");
                }
            }
            else
            {
                foreach (Control c in whileEditingControls)
                {
                    c.Hide();
                }
                unfilteredDataGridView.ReadOnly = true;
                //resetLabelsText();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            whileNotEditingControlsStatus(false);
            whileEditingControlsStatus(true);
            //messageBoardLbl.Text = "Edit in progress ...";
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            MainForm myMainForm = new MainForm();
            myMainForm.Show();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to Abort?";
            string caption = "Confirm Cancellation";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if (result == DialogResult.Yes)
            {
                try
                {
                    GetData();
                    whileEditingControlsStatus(false);
                    whileNotEditingControlsStatus(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.unfilteredDataGridView.SelectedRows)
            {
                try
                {
                    unfilteredDataGridView.Rows.RemoveAt(item.Index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public virtual void saveBtn_Click(object sender, EventArgs e)
        {
            save();
        }

        public void save()
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
                    DataTable dt = (DataTable)bindingSource1.DataSource;
                    int r = adapter.Update(dt);
                    whileEditingControlsStatus(false);
                    MessageBox.Show("Saved! " + r + " row(s) affected.");
                    //GetData(adapter.SelectCommand.CommandText);
                    whileNotEditingControlsStatus(true);
                }
                catch (SqlException sqlEx)
                {
                    GetData();
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
                    GetData();
                    MessageBox.Show("Not Saved!");
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
