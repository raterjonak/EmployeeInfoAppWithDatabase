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
    }
}
