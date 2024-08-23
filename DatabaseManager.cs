using System;
using System.Data;
using System.Data.SQLite;

namespace Pyniu_tikrinimas
{
    public class DatabaseManager
    {
        private const string dbPath = "pynes.db";

        public void CreateDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }
        }

        public void CreateTables()
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                string dropHarnessDrawingTable = "DROP TABLE IF EXISTS harness_drawing;";
                string dropHarnessWiresTable = "DROP TABLE IF EXISTS harness_wires;";

                using (var command = new SQLiteCommand(dropHarnessDrawingTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(dropHarnessWiresTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                string createHarnessDrawingTable = @"
                    CREATE TABLE IF NOT EXISTS harness_drawing (
                        ID INTEGER PRIMARY KEY,
                        Harness VARCHAR(30),
                        Issue VARCHAR(30), 
                        Drw VARCHAR(30),
                        DrwIssue VARCHAR(30)
                    );";

                string createHarnessWiresTable = @"
                    CREATE TABLE IF NOT EXISTS harness_wires (
                        ID INTEGER PRIMARY KEY,
                        Harness_ID INTEGER NOT NULL,
                        Length REAL,
                        Color VARCHAR(30),
                        Housing_1 VARCHAR(30),
                        Housing_2 VARCHAR(30),
                        FOREIGN KEY (Harness_ID) REFERENCES harness_drawing(ID)
                    );";

                using (var command = new SQLiteCommand(createHarnessDrawingTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createHarnessWiresTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertSampleData()
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                string insertDrawing = @"
                    INSERT OR IGNORE INTO harness_drawing (ID, Harness, Issue, Drw, DrwIssue) VALUES
                    (40953, 'S2563532M', 'S-6', 'EP', 'S-4'),
                    (40442, 'S2563545M', 'S12', 'EP', 'S-4'),
                    (39087, 'S2563549M', 'S-9', 'EP', 'S-4'),
                    (39077, 'S2641137M', 'S-9', 'EP', 'S-4'),
                    (38643, 'S2656843M', '5', 'EP', 'S-4');";

                using (var command = new SQLiteCommand(insertDrawing, connection))
                {
                    command.ExecuteNonQuery();
                }

                string insertWires = @"
                    INSERT OR IGNORE INTO harness_wires (ID, Harness_ID, Length, Color, Housing_1, Housing_2) VALUES
                    (3115654, 38643, 950, 'R', 'C604:19', 'P2.BX2:1'),
                    (3115655, 38643, 450, 'R', 'C604:23', 'C521:1'),
                    (3158749, 39077, 665, 'BN', 'E71.B:1', 'C604:21'),
                    (3158750, 39077, 665, 'GR', 'E71.B:4', 'C604:23'),
                    (3159894, 39087, 465, 'W', 'E71.A:1', 'C681'),
                    (3159895, 39087, 680, 'SB', 'E71.P:3', 'G504-2'),
                    (3277678, 40442, 475, 'GN', 'P2.E85:1', 'C680'),
                    (3277679, 40442, 980, 'R', 'P2.BX2:1', 'E30.P:1'),
                    (3328453, 40953, 365, 'W', 'C621:6', 'C681'),
                    (3328454, 40953, 305, 'SB', 'C620:24', 'G508-3');";

                using (var command = new SQLiteCommand(insertWires, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetRandomHarnesses(int numberOfHarnesses)
        {
            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();

                string query = $@"
                    SELECT hd.Harness, hd.Issue, hd.Drw, hd.DrwIssue, hw.Housing_1, hw.Housing_2 
                    FROM harness_drawing hd
                    JOIN harness_wires hw ON hd.ID = hw.Harness_ID
                    ORDER BY RANDOM()
                    LIMIT {numberOfHarnesses};";

                var adapter = new SQLiteDataAdapter(query, connection);
                var table = new DataTable();
                adapter.Fill(table);

                return table;
            }
        }
    }
}
