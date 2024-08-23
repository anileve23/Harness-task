using System;
using System.Windows.Forms;

namespace Pyniu_tikrinimas
{
    public partial class Form1 : Form
    {
        private DatabaseManager dbManager = new DatabaseManager();
        private HarnessManager harnessManager = new HarnessManager();

        public Form1()
        {
            InitializeComponent();
            dbManager.CreateDatabase();
            dbManager.CreateTables();
            dbManager.InsertSampleData();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            harnessManager.GenerateRandomHarnesses(dataGridView1);
            harnessManager.CheckForDuplicatesAndHighlight(dataGridView1);
        }
    }
}
