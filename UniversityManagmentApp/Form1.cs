using System;
using System.Collections.Generic;
using System.ComponentModel;
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


            string connectionString =@"Server=RATERJONAK;Database=UniversityManagmentDB;Integrated Security=true";

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
        }

        public bool IsRegNoExist(string regNo)
        {

            bool isRegNoExist = false;
            String connectionString = "Server=RATERJONAK;Database=UniversityManagmentDB;Integrated Security=true";

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
    }
}
