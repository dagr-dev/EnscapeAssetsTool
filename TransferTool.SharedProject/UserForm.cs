using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadRhinoFile
{
    /// <summary>
    /// Represents a Windows Form for selecting coordinate planes and related settings in a Rhino to Revit data transfer tool.
    /// </summary>
    public partial class UserForm : System.Windows.Forms.Form
    {
        // Properties to store selected values
        public int SelectedCPlaneIndex { get; private set; } = -1;
        public int SelectedRevitOrigin { get; private set; } = -1;
        public string SelectedRevitWorkset { get; private set; }
        public bool WorkSharing { get; private set; }

        /// <summary>
        /// Initializes a new instance of the CPlane class with the specified lists of coordinate planes and worksets.
        /// </summary>
        /// <param name="cplanes">List of coordinate planes from Rhino file.</param>
        /// <param name="worksets">List of worksets from Revit file.</param>
        /// <param name="isWorkSharing">Flag indicating whether work sharing is enabled.</param>
        public UserForm(List<string> cplanes, List<string> worksets, bool isWorkSharing)
        {
            InitializeComponent();
            // Set up form controls with provided data
            Pick_CPlane.DataSource = cplanes;

            if (isWorkSharing) RevitWorkset.DataSource = worksets;
            if (worksets.Count < 2) RevitWorkset.Enabled = false;

            RevitLocation.Items.Add("Internal Origin");
            RevitLocation.Items.Add("Project Base Point");
            RevitLocation.Items.Add("Survey Point");
            RevitLocation.SelectedIndex= 1;

            this.KeyPreview = true;
            this.KeyPress += MainForm_KeyPress;
            this.WorkSharing = isWorkSharing;
        }

        private void CPlane_Load(object sender, EventArgs e)
        {

        }

        private void pick_CPlane_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            // Retrieve the selected item
            SelectedCPlaneIndex = Pick_CPlane.SelectedIndex;
            SelectedRevitOrigin = RevitLocation.SelectedIndex;
            if (WorkSharing) SelectedRevitWorkset = RevitWorkset.SelectedItem.ToString();
            this.Close();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is the "Esc" key
            if (e.KeyChar == (char)27)  // ASCII value for "Esc" key
            {
                // Handle the "Esc" key press (e.g., close the form)
                this.Close();
            }
        }

    }
}
