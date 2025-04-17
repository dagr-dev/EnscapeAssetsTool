namespace ReadRhinoFile
{
    partial class UserForm
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
            this.Pick_CPlane = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RevitLocation = new System.Windows.Forms.ComboBox();
            this.RevitWorkset = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Pick_CPlane
            // 
            this.Pick_CPlane.AccessibleDescription = "Pick_CPlane";
            this.Pick_CPlane.AccessibleName = "Pick_CPlane";
            this.Pick_CPlane.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Pick_CPlane.DropDownHeight = 100;
            this.Pick_CPlane.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Pick_CPlane.DropDownWidth = 250;
            this.Pick_CPlane.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.Pick_CPlane.FormattingEnabled = true;
            this.Pick_CPlane.IntegralHeight = false;
            this.Pick_CPlane.Location = new System.Drawing.Point(50, 65);
            this.Pick_CPlane.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Pick_CPlane.Name = "Pick_CPlane";
            this.Pick_CPlane.Size = new System.Drawing.Size(250, 22);
            this.Pick_CPlane.TabIndex = 0;
            this.Pick_CPlane.SelectedIndexChanged += new System.EventHandler(this.pick_CPlane_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.button1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.button1.Location = new System.Drawing.Point(142, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.label1.Location = new System.Drawing.Point(47, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Rhino CPlane as the location origin:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.label2.Location = new System.Drawing.Point(47, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select Revit target location:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // RevitLocation
            // 
            this.RevitLocation.AccessibleDescription = "RevitLocation";
            this.RevitLocation.AccessibleName = "RevitLocation";
            this.RevitLocation.BackColor = System.Drawing.SystemColors.ControlLight;
            this.RevitLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RevitLocation.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.RevitLocation.FormattingEnabled = true;
            this.RevitLocation.Location = new System.Drawing.Point(50, 145);
            this.RevitLocation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RevitLocation.Name = "RevitLocation";
            this.RevitLocation.Size = new System.Drawing.Size(250, 22);
            this.RevitLocation.TabIndex = 3;
            this.RevitLocation.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // RevitWorkset
            // 
            this.RevitWorkset.AccessibleDescription = "RevitLocation";
            this.RevitWorkset.AccessibleName = "RevitLocation";
            this.RevitWorkset.BackColor = System.Drawing.SystemColors.ControlLight;
            this.RevitWorkset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RevitWorkset.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.RevitWorkset.FormattingEnabled = true;
            this.RevitWorkset.Location = new System.Drawing.Point(50, 225);
            this.RevitWorkset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RevitWorkset.Name = "RevitWorkset";
            this.RevitWorkset.Size = new System.Drawing.Size(250, 22);
            this.RevitWorkset.TabIndex = 3;
            this.RevitWorkset.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lucida Sans", 8F);
            this.label3.Location = new System.Drawing.Point(47, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select Revit target workset:";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // CPlane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(359, 344);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RevitWorkset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RevitLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Pick_CPlane);
            this.Font = new System.Drawing.Font("Circular Pro Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CPlane";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AssetSync for Enscape";
            this.Load += new System.EventHandler(this.CPlane_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Pick_CPlane;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox RevitLocation;
        private System.Windows.Forms.ComboBox RevitWorkset;
        private System.Windows.Forms.Label label3;
    }
}