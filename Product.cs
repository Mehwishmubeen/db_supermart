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
    public partial class Product : Form
    {
        private string connectionString = "Data Source=DESKTOP-2NPF8UT\\SQLEXPRESS01;Initial Catalog=db_supermart;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        public Product()
        {
            InitializeComponent();
            LoadCategoryIDs();




        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productId = textBox5.Text;
            string productname = textBox4.Text;
            string categoryId = comboBox1.Text;
            string price = textBox3.Text;
            string expiryDate = textBox1.Text;
            string addingDate = textBox6.Text;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "sp_addproduct"; // Stored Procedure name
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Use parameters
                        cmd.Parameters.AddWithValue("@productid", productId);
                        cmd.Parameters.AddWithValue("@productname", productname);
                        cmd.Parameters.AddWithValue("@categoryid", categoryId);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@expirydate", expiryDate);
                        cmd.Parameters.AddWithValue("@addingDate", addingDate);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product added successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add product.");
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

            string productId = textBox5.Text;
            string productname = textBox4.Text;
            string categoryId = comboBox1.Text;
            string price = textBox3.Text;
            string expiryDate = textBox1.Text;
            string addingDate = textBox6.Text;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "sp_updateproduct"; // Stored Procedure name
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Use parameters
                        cmd.Parameters.AddWithValue("@productid", productId);
                        cmd.Parameters.AddWithValue("@productname", productname);
                        cmd.Parameters.AddWithValue("@addingDate", addingDate);


                        cmd.Parameters.AddWithValue("@categoryid", categoryId);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@expirydate", expiryDate);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to update product.");
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
            string productId = textBox5.Text;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "sp_deleteproduct"; // Stored Procedure name
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Use parameters
                        cmd.Parameters.AddWithValue("@productid", productId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product deleted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete product.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void LoadCategoryIDs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CategoryID FROM tbl_Category";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader["CategoryID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Category IDs: {ex.Message}");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
        }
        private void comboBoxCategoryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategoryID = comboBox1.Text;

            if (!string.IsNullOrEmpty(selectedCategoryID))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT Name FROM tbl_Category WHERE CategoryID = @CategoryID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryID);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    textBox2.Text = reader["Name"].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Category Name: {ex.Message}");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}



