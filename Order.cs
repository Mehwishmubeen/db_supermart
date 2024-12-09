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

namespace WinFormsApp1
{
    public partial class Order : Form
    {
        private string connectionString = "Data Source=DESKTOP-2NPF8UT\\SQLEXPRESS01;Initial Catalog=db_supermart;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public Order()
        {
            InitializeComponent();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
         
                String orderDate = textBox1.Text; // Assuming you have a DateTimePicker
                String amount = textBox2.Text; // Assuming you have a TextBox for Amount

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "EXEC sp_add_order @orderdate, @amount";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@orderdate", orderDate);
                            cmd.Parameters.AddWithValue("@amount", amount);

                            object result = cmd.ExecuteScalar(); // Retrieve the generated OrderID
                            if (result != null)
                            {
                                MessageBox.Show($"Order added successfully! Generated OrderID: {result}");
                            }
                            else
                            {
                                MessageBox.Show("Failed to add the order.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            
        }
    }
}

