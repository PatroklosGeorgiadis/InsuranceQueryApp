using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace InsuranceQueryApp
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private string query;
        public Form1()
        {
            InitializeComponent();
        }

        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=me;Password=1234;Database=Insurance;");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "(SELECT \"Κωδικός Συμβολαίου\",\"Ονοματεπώνυμο\",\"Αριθμός άδειας οδήγησης\""+
                            "FROM \"Ασφάλεια\".\"Συμβόλαια\" FULL OUTER JOIN \"Ασφάλεια\".\"Πελάτες\" "+
                            "ON \"Ασφάλεια\".\"Συμβόλαια\".\"Πελάτης (Αριθμός άδειας οδήγησης)\" = \"Ασφάλεια\".\"Πελάτες\".\"Αριθμός άδειας οδήγησης\""+
                            "WHERE EXTRACT(MONTH FROM \"Ημερομηνία Έναρξης\") = EXTRACT(MONTH FROM CURRENT_DATE)"+
                            "AND EXTRACT(YEAR FROM \"Ημερομηνία Έναρξης\") = EXTRACT(YEAR FROM CURRENT_DATE))"+
                            "UNION ALL (SELECT \"Κωδικός Συμβολαίου\",\"Ονοματεπώνυμο\",\"Αριθμός άδειας οδήγησης\""+
                            "FROM \"Ασφάλεια\".\"Συμβόλαια\" FULL OUTER JOIN \"Ασφάλεια\".\"Κωδικοί Συμβολαίων\""+
                            "ON \"Ασφάλεια\".\"Συμβόλαια\".\"Κωδικός Συμβολαίου\" = \"Ασφάλεια\".\"Κωδικοί Συμβολαίων\".\"Συμβόλαια_Κωδικός Συμβολαίου\" "+
                            "FULL OUTER JOIN \"Ασφάλεια\".\"Οδηγοί\" "+
                            "ON \"Ασφάλεια\".\"Κωδικοί Συμβολαίων\".\"Οδηγοί_Αρ άδειας οδήγησης\" = \"Ασφάλεια\".\"Οδηγοί\".\"Αριθμός άδειας οδήγησης\""+
                            "WHERE EXTRACT(MONTH FROM \"Ημερομηνία Έναρξης\") = EXTRACT(MONTH FROM CURRENT_DATE)"+
                            "AND EXTRACT(YEAR FROM \"Ημερομηνία Έναρξης\") = EXTRACT(YEAR FROM CURRENT_DATE))";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "SELECT \"Κωδικός Συμβολαίου\",\"Τηλέφωνο επικοινωνίας\",\"Ημερομηνία Λήξης\" "+
                            "FROM \"Ασφάλεια\".\"Συμβόλαια\" FULL OUTER JOIN \"Ασφάλεια\".\"Πελάτες\" "+
                            "ON \"Ασφάλεια\".\"Συμβόλαια\".\"Πελάτης (Αριθμός άδειας οδήγησης)\" = \"Ασφάλεια\".\"Πελάτες\".\"Αριθμός άδειας οδήγησης\""+
                            "WHERE EXTRACT(MONTH FROM \"Ημερομηνία Λήξης\") = EXTRACT(MONTH FROM CURRENT_DATE + '1 month'::interval)"+
                            "AND EXTRACT(YEAR FROM \"Ημερομηνία Λήξης\") = EXTRACT(YEAR FROM CURRENT_DATE)";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "SELECT \"Ασφαλιστική ομάδα-κατηγορία\", EXTRACT(YEAR FROM \"Ημερομηνία Έναρξης\") AS \"Έτος δημιουργίας συμβολαίου\", COUNT(*)"+
                            "FROM \"Ασφάλεια\".\"Συμβόλαια\" FULL OUTER JOIN \"Ασφάλεια\".\"Οχήματα\""+
                            "ON \"Οχήματα\".\"Αριθμός άδειας κυκλοφορίας\" = \"Συμβόλαια\".\"Όχημα (Αριθμός Άδειας Κυκλοφορίας)\""+
                            "WHERE EXTRACT(YEAR FROM \"Ημερομηνία Έναρξης\") > 2015 AND EXTRACT(YEAR FROM \"Ημερομηνία Έναρξης\") < 2021"+
                            "GROUP BY \"Ασφαλιστική ομάδα-κατηγορία\",\"Έτος δημιουργίας συμβολαίου\"";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "WITH euro_cut(\"ευρώ\") AS (SELECT \"Κόστος Συμβολαίου\" FROM \"Ασφάλεια\".\"Συμβόλαια\"),"+
                                 "dot_replace(\"καθαρός αριθμός\") AS (SELECT LTRIM(\"ευρώ\",'€') FROM euro_cut),"+
                                 "sum_of_money(\"Σύνολο χρημάτων\") AS (SELECT REPLACE(\"καθαρός αριθμός\",',','.')::DOUBLE PRECISION FROM dot_replace),"+
                                 "maximum_sum_of_money(\"Συνολικός Τζίρος\",\"Ασφαλιστική ομάδα-κατηγορία\") AS (SELECT SUM(\"Σύνολο χρημάτων\"),\"Ασφαλιστική ομάδα-κατηγορία\""+
                                 "FROM sum_of_money, \"Ασφάλεια\".\"Οχήματα\" GROUP BY \"Ασφαλιστική ομάδα-κατηγορία\"),"+
                                 "category_of_max(\"Μέγιστος Τζίρος\") AS (SELECT MAX(\"Συνολικός Τζίρος\") FROM maximum_sum_of_money)"+
                            "SELECT \"Μέγιστος Τζίρος\",\"Ασφαλιστική ομάδα-κατηγορία\""+
                            "FROM category_of_max, maximum_sum_of_money WHERE \"Μέγιστος Τζίρος\" = \"Συνολικός Τζίρος\"";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "WITH allvehicles(\"Όλα τα οχήματα\") AS ("+
                                "SELECT COUNT(*) FROM \"Ασφάλεια\".\"Οχήματα\"),"+
                            "avg0to4years(\"Μέσος όρος 0-4 έτη\") AS ("+
                                "SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Χαρακτηριστικά Οχήματος\" WHERE \"Έτος πρώτης κυκλοφορίας\" > 2017),"+
                            "avg5to9years(\"Μέσος όρος 5-9 έτη\") AS ("+
                                "SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Χαρακτηριστικά Οχήματος\" WHERE \"Έτος πρώτης κυκλοφορίας\" > 2010 AND \"Έτος πρώτης κυκλοφορίας\" < 2016),"+
                            "avg10to19years(\"Μέσος όρος 10-19 έτη\") AS ("+
                                "SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Χαρακτηριστικά Οχήματος\" WHERE \"Έτος πρώτης κυκλοφορίας\" > 2001 AND \"Έτος πρώτης κυκλοφορίας\" < 2011),"+
                            "avg20plusyears(\"Μέσος όρος 20+ έτη\") AS ("+
                                "SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Χαρακτηριστικά Οχήματος\" WHERE \"Έτος πρώτης κυκλοφορίας\" < 2002)"+
                            "SELECT CONCAT(((\"Μέσος όρος 0-4 έτη\"/\"Όλα τα οχήματα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 0-4 έτη\","+
                            "CONCAT(((\"Μέσος όρος 5-9 έτη\"/\"Όλα τα οχήματα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 5-9 έτη\","+
                            "CONCAT(((\"Μέσος όρος 10-19 έτη\"/\"Όλα τα οχήματα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 10-19 έτη\","+
                            "CONCAT(((\"Μέσος όρος 20+ έτη\"/\"Όλα τα οχήματα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 20+ έτη\" FROM avg0to4years, avg5to9years, avg10to19years, avg20plusyears, allvehicles";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = GetConnection())
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        try
                        {
                            query = "WITH alldrivers(\"Όλα τα συμβάντα\") AS ("+
                            "SELECT COUNT(*) FROM \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\"), avg18to24years(\"Μέσος όρος 18-24 χρονών\") AS ("+
                            "SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Οδηγοί\" FULL OUTER JOIN \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\""+
                            "ON \"Ασφάλεια\".\"Οδηγοί\".\"Αριθμός άδειας οδήγησης\" = \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\".\"Εμπλεκόμενος Οδηγός\" WHERE EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") > 1996),"+
                            "avg25to49years(\"Μέσος όρος 25-49 χρονών\") AS ( SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Οδηγοί\" FULL OUTER JOIN \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\""+
                            "ON \"Ασφάλεια\".\"Οδηγοί\".\"Αριθμός άδειας οδήγησης\" = \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\".\"Εμπλεκόμενος Οδηγός\" WHERE EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") < 1997 AND EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") > 1971),"+
                            "avg50to69years(\"Μέσος όρος 50-69 χρονών\") AS ( SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Οδηγοί\" FULL OUTER JOIN \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\""+
                            "ON \"Ασφάλεια\".\"Οδηγοί\".\"Αριθμός άδειας οδήγησης\" = \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\".\"Εμπλεκόμενος Οδηγός\" WHERE EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") < 1972 AND EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") > 1951),"+
                            "avg70plusyears(\"Μέσος όρος 70+ χρονών\") AS ( SELECT COUNT(*)::FLOAT FROM \"Ασφάλεια\".\"Οδηγοί\" FULL OUTER JOIN \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\""+
                            "ON \"Ασφάλεια\".\"Οδηγοί\".\"Αριθμός άδειας οδήγησης\" = \"Ασφάλεια\".\"Παρεμβάσεις-Συμβάντα\".\"Εμπλεκόμενος Οδηγός\" WHERE EXTRACT(YEAR FROM \"Ημερομηνία γέννησης\") < 1952)"+
                            "SELECT CONCAT(((\"Μέσος όρος 18-24 χρονών\"/\"Όλα τα συμβάντα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 18-24 χρονών\", CONCAT(((\"Μέσος όρος 25-49 χρονών\"/\"Όλα τα συμβάντα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 25-49 χρονών\","+
                            "CONCAT(((\"Μέσος όρος 50-69 χρονών\"/\"Όλα τα συμβάντα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 50-69 χρονών\", CONCAT(((\"Μέσος όρος 70+ χρονών\"/\"Όλα τα συμβάντα\")*100)::INT::VARCHAR,'%') AS \"Μέσος όρος 70+ χρονών\""+
                            "FROM avg18to24years, avg25to49years, avg50to69years, avg70plusyears, alldrivers; ";
                            cmd = new NpgsqlCommand(query, conn);
                            dt = new DataTable();
                            dt.Load(cmd.ExecuteReader());
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = dt;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    conn.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
