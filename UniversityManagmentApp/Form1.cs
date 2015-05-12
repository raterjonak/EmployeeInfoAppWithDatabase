using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UniversityManagmentApp
{
    public partial class UniversityManagmentUI : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["UniversityManagmentConnString"].ConnectionString;
        bool updateMode = false;
        private int studentId;
        public UniversityManagmentUI()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string regNo = regNoTextBox.Text;
            string address = addressTextBox.Text;

            if (updateMode)
            {
                SqlConnection connection = new SqlConnection(connectionString);

                string query = "UPDATE  Students SET Name='" + name + "',Address='" + address + "' WHERE ID='"+studentId+"'";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Your Data has been update.");
                    saveButton.Text = "Save";
                    updateMode = false;
                }

                else
                {
                    MessageBox.Show(" Update Failed!");
                }
            }

            else
            {
                if (IsRegNoExist(regNo))
                {
                    MessageBox.Show("Registration Number already exist.");
                    return;
                }


                SqlConnection connection = new SqlConnection(connectionString);

                string query = "INSERT INTO Students VALUES('" + name + "','" + regNo + "','" + address + "')";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Your Data insertion has been saved.");
                }

                else
                {
                    MessageBox.Show("Insertion Failed!");
                }

            }
           
            ShowAllStudent();
        }

        public bool IsRegNoExist(string regNo)
        {

            bool isRegNoExist = false;
            

            SqlConnection connection=new SqlConnection(connectionString);

            string query = "SELECT * FROM Students WHERE RegNo='" + regNo + "'";

            SqlCommand command=new SqlCommand(query,connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                isRegNoExist = true;
                break;
            }

            reader.Close();
            connection.Close();

            return isRegNoExist;
        }

       

       


        public void ShowAllStudent()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM Students";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Student> studentList = new List<Student>();
            

            while (reader.Read())
            {
                Student student = new Student();
                student.Id = int.Parse(reader["ID"].ToString());
                student.Name = reader["Name"].ToString();
                student.RegNo = reader["RegNo"].ToString();
                student.Address = Convert.ToString(reader["Address"]);

                studentList.Add(student);

            }

            reader.Close();
            connection.Close();

            LoadStudentListView(studentList);    
 
        }


        public void LoadStudentListView(List<Student> students)
        {
            studentListView.Items.Clear();
            foreach (var student in students)
            {
                ListViewItem item = new ListViewItem(student.Id.ToString());
                item.SubItems.Add(student.Name);
                item.SubItems.Add(student.RegNo);
                item.SubItems.Add(student.Address);
                studentListView.Items.Add(item);
            }
        }

        private void UniversityManagmentUI_Load(object sender, EventArgs e)
        {
            ShowAllStudent();
        }

        private void studentListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = studentListView.SelectedItems[0];
            int ID = Convert.ToInt16(item.Text);
            Student student = GetStudentById(ID);

            if (student!=null)
            {
                updateMode = true;
                saveButton.Text = "Update";
                studentId = ID;

                nameTextBox.Text = student.Name;
                regNoTextBox.Text = student.RegNo;
                addressTextBox.Text = student.Address;
                regNoTextBox.Enabled = false;
            }

        }


        public Student GetStudentById(int id)
        {
            SqlConnection connection=new SqlConnection(connectionString);
            string query = "SELECT * FROM Students WHERE ID='" + id + "'";
            SqlCommand command= new SqlCommand(query,connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Student>studentList= new List<Student>();

            while (reader.Read())
            {
               Student student=new Student();

                student.Id = Convert.ToInt16(reader["ID"]);
                student.Name = reader["Name"].ToString();
                student.RegNo = reader["RegNo"].ToString();
                student.Address = reader["Address"].ToString();

                studentList.Add(student);

            }
            reader.Close();
            connection.Close();
            return studentList.FirstOrDefault();
        }
    }
}
