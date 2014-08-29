namespace Apartment_Building_Management
{
    partial class filteredFormBasedOnLocation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.areaComboBox = new System.Windows.Forms.ComboBox();
            this.addressComboBox = new System.Windows.Forms.ComboBox();
            this.areaLbl = new System.Windows.Forms.Label();
            this.addressLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // areaComboBox
            // 
            this.areaComboBox.FormattingEnabled = true;
            this.areaComboBox.Location = new System.Drawing.Point(12, 25);
            this.areaComboBox.Name = "areaComboBox";
            this.areaComboBox.Size = new System.Drawing.Size(121, 21);
            this.areaComboBox.TabIndex = 0;
            this.areaComboBox.SelectedIndexChanged += new System.EventHandler(this.areaComboBox_SelectedIndexChanged);
            // 
            // addressComboBox
            // 
            this.addressComboBox.FormattingEnabled = true;
            this.addressComboBox.Location = new System.Drawing.Point(162, 25);
            this.addressComboBox.Name = "addressComboBox";
            this.addressComboBox.Size = new System.Drawing.Size(121, 21);
            this.addressComboBox.TabIndex = 1;
            this.addressComboBox.SelectedIndexChanged += new System.EventHandler(this.addressComboBox_SelectedIndexChanged);
            // 
            // areaLbl
            // 
            this.areaLbl.AutoSize = true;
            this.areaLbl.Location = new System.Drawing.Point(9, 9);
            this.areaLbl.Name = "areaLbl";
            this.areaLbl.Size = new System.Drawing.Size(32, 13);
            this.areaLbl.TabIndex = 2;
            this.areaLbl.Text = "Area:";
            this.areaLbl.Click += new System.EventHandler(this.label1_Click);
            // 
            // addressLbl
            // 
            this.addressLbl.AutoSize = true;
            this.addressLbl.Location = new System.Drawing.Point(159, 9);
            this.addressLbl.Name = "addressLbl";
            this.addressLbl.Size = new System.Drawing.Size(48, 13);
            this.addressLbl.TabIndex = 3;
            this.addressLbl.Text = "Address:";
            // 
            // filteredFormBasedOnLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 432);
            this.Controls.Add(this.addressLbl);
            this.Controls.Add(this.areaLbl);
            this.Controls.Add(this.addressComboBox);
            this.Controls.Add(this.areaComboBox);
            this.Name = "filteredFormBasedOnLocation";
            this.Text = "filteredFormBasedOnLocation";
            this.Load += new System.EventHandler(this.filteredFormBasedOnLocation_Load);
            this.Controls.SetChildIndex(this.saveBtn, 0);
            this.Controls.SetChildIndex(this.areaComboBox, 0);
            this.Controls.SetChildIndex(this.addressComboBox, 0);
            this.Controls.SetChildIndex(this.areaLbl, 0);
            this.Controls.SetChildIndex(this.addressLbl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox areaComboBox = new System.Windows.Forms.ComboBox();
        public System.Windows.Forms.ComboBox addressComboBox;
        private System.Windows.Forms.Label areaLbl;
        private System.Windows.Forms.Label addressLbl;
    }
}