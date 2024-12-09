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
    public partial class Supplier : Form
    {
        private string connectionString = "Data Source=DESKTOP-2NPF8UT\\SQLEXPRESS01;Initial Catalog=db_supermart;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public Supplier()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string id = textBox1.Text;  // Supplier ID from TextBox
            string name = textBox2.Text;  // Name from TextBox
            string phone = textBox3.Text;  // Phone from TextBox
            string address = textBox4.Text;  // Address from TextBox

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "EXEC sp_addsupplier @supplierid, @name, @phoneno, @address";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@supplierid", id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@phoneno", phone);
                        cmd.Parameters.AddWithValue("@address", address);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Supplier added successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add supplier.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
                string id = textBox1.Text;  // Supplier ID from TextBox
                string name = textBox2.Text;  // Name from TextBox
                string phone = textBox3.Text;  // Phone from TextBox
                string address = textBox4.Text;  // Address from TextBox

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "EXEC sp_updatesupplier @supplierid, @name, @phoneno, @address";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Use parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@supplierid", id);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@phoneno", phone);
                            cmd.Parameters.AddWithValue("@address", address);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Supplier updated successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Failed to update supplier.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        

        private void button3_Click(object sender, EventArgs e)
        {
            
                string id = textBox1.Text;  // Supplier ID from TextBox

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "EXEC sp_deletesupplier @supplierid";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Use parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@supplierid", id);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Supplier deleted successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete supplier.");
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





