using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UniversityManagmentApp
{
    public partial class UniversityManagmentUI : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["UniversityManagmentConnString"].ConnectionString;
        public UniversityManagmentUI()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string regNo = regNoTextBox.Text;
            string address = addressTextBox.Text;


            if (IsRegNoExist(regNo))
            {
                MessageBox.Show("Registration Number already exist.");
                return;
            }

            
            SqlConnection connection = new SqlConnection(connectionString);

            string query="INSERT INTO Students VALUES('"+name + "','" + regNo + "','" + address + "')";

            SqlCommand command=new SqlCommand(query,connection);

            connection.Open(); 
            int rowAffected=command.ExecuteNonQuery();
            connection.Close();

            if (rowAffected>0)
            {
                MessageBox.Show("Your Data has been saved.");
            }

            else
            {
                MessageBox.Show("Failed!");
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

       

        public void LoadStudentListView(List<Student> students)
        {
            studentListView.Items.Clear();
            foreach (var student in students)
            {
                ListViewItem item=new ListViewItem(student.Id.ToString());
                item.SubItems.Add(student.Name);
                item.SubItems.Add(student.RegNo);
                item.SubItems.Add(student.Address);
                studentListView.Items.Add(item);
            }
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

        private void UniversityManagmentUI_Load(object sender, EventArgs e)
        {
            ShowAllStudent();
        }
    }
}
