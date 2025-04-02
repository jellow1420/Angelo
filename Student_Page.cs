using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StudentForm
{
    public partial class Student_Page : Form
    {
        private MySqlConnection connection;

        public Student_Page()
        {
            InitializeComponent();
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            string connectionString = "server=localhost;database=StudentInfoDB;user=root;password=12345;";
            connection = new MySqlConnection(connectionString);
        }

        private void Student_Page_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                connection.Open();
                string query = "SELECT s.studentId, s.firstName, s.lastName, c.courseName FROM StudentRecordTB s " +
                               "JOIN CourseTB c ON s.courseId = c.courseId";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;

                // Ensure the "VIEW" button column exists
                if (!dataGridView.Columns.Contains("ViewBtn"))
                {
                    DataGridViewButtonColumn viewButton = new DataGridViewButtonColumn
                    {
                        Name = "ViewBtn",
                        HeaderText = "Action",
                        Text = "VIEW",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridView.Columns.Add(viewButton);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Check if the "VIEW" button was clicked
                if (e.ColumnIndex == dataGridView.Columns["ViewBtn"].Index && e.RowIndex >= 0)
                {
                    // Get the studentId value from the clicked row
                    int studentId = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["studentId"].Value);
                    MessageBox.Show($"Opening student details for ID: {studentId}", "Debugging Info");

                    // Open the individual student form
                    StudentPage_Individual individualForm = new StudentPage_Individual(studentId);
                    individualForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling the 'VIEW' action: " + ex.Message, "Event Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}