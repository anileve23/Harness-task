using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pyniu_tikrinimas
{
    public class HarnessManager
    {
        private DatabaseManager dbManager = new DatabaseManager();

        public void GenerateRandomHarnesses(DataGridView dataGridView, int minHarnesses = 3, int maxHarnesses = 5)
        {
            Random rnd = new Random();
            int numberOfHarnesses = rnd.Next(minHarnesses, maxHarnesses + 1);

            DataTable table = dbManager.GetRandomHarnesses(numberOfHarnesses);
            dataGridView.DataSource = table;

            // Clear previous highlights
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["Housing_1"].Style.BackColor = Color.White;
                    row.Cells["Housing_2"].Style.BackColor = Color.White;
                }
            }
        }

        public void CheckForDuplicatesAndHighlight(DataGridView dataGridView)
        {
            var housingValues = new Dictionary<string, DataGridViewCell>();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                string housing1Value = row.Cells["Housing_1"].Value as string;
                string housing2Value = row.Cells["Housing_2"].Value as string;

                // Check Housing_1 column for duplicates
                if (!string.IsNullOrEmpty(housing1Value))
                {
                    if (housingValues.ContainsKey(housing1Value))
                    {
                        var duplicateCell = housingValues[housing1Value];
                        if (duplicateCell != null)
                        {
                            duplicateCell.Style.BackColor = Color.Red;
                        }
                        row.Cells["Housing_1"].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        housingValues[housing1Value] = row.Cells["Housing_1"];
                    }
                }

                // Check Housing_2 column for duplicates
                if (!string.IsNullOrEmpty(housing2Value))
                {
                    if (housingValues.ContainsKey(housing2Value))
                    {
                        var duplicateCell = housingValues[housing2Value];
                        if (duplicateCell != null)
                        {
                            duplicateCell.Style.BackColor = Color.Red;
                        }
                        row.Cells["Housing_2"].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        housingValues[housing2Value] = row.Cells["Housing_2"];
                    }
                }
            }
        }
    }
}
