using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class OrderLinecs : Form
    {
        // Connection string for the database
        private string connectionString = "Data Source=DESKTOP-2NPF8UT\\SQLEXPRESS01;Initial Catalog=db_supermart;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        // List to store order lines
        private List<OrderLine> orderLines = new List<OrderLine>();

        public OrderLinecs()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set up the form
            this.Text = "Product Order Form";
            this.Width = 2600;
            this.Height = 1600;

            // Initialize FlowLayoutPanel
            flowLayoutPanel1 = new FlowLayoutPanel
            {
                Width = 2550,
                Height = 1300,
                Location = new System.Drawing.Point(30, 70),
                AutoScroll = true
            };
            this.Controls.Add(flowLayoutPanel1);


            TextBox txtTotalBill = new TextBox
            {
                Width = 150,
                Location = new System.Drawing.Point(100, 40),
                ReadOnly = true
            };
            // Add New Product Button
            Button btnAddProduct = new Button
            {
                Text = "Add Product",
                Width = 100,
                Location = new System.Drawing.Point(100, 25)
            };
            btnAddProduct.Click += (sender, e) => AddProductLine(flowLayoutPanel1, txtTotalBill);
            flowLayoutPanel1.Controls.Add(btnAddProduct);


            // Submit Order Button (for later, when you have added order lines)
            Button btnSubmitOrder = new Button
            {
                Text = "Submit Order",
                Width = 100,
                Location = new System.Drawing.Point(100, 25)
            };



            btnSubmitOrder.Click += BtnSubmitOrder_Click;
            flowLayoutPanel1.Controls.Add(btnSubmitOrder);
            flowLayoutPanel1.Controls.Add(txtTotalBill);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoSize = true;

        }

        // Handle the Submit Order button click
        private void BtnSubmitOrder_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Start a transaction for consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Insert into the Order table and retrieve the generated OrderID
                        decimal totalAmount = orderLines.Sum(orderLine =>
                        {
                            if (decimal.TryParse(orderLine.PriceTextBox.Text, out decimal price) &&
                                int.TryParse(orderLine.QuantityTextBox.Text, out int quantity))
                            {
                                return price * quantity;
                            }
                            return 0;
                        });

                        string insertOrderQuery = @"
                    INSERT INTO tbl_Order (OrderDate, Amount)
                    VALUES (@OrderDate, @Amount);
                    SELECT SCOPE_IDENTITY();"; // Retrieve the newly generated OrderID

                        int orderId;
                        using (SqlCommand cmd = new SqlCommand(insertOrderQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Amount", totalAmount);

                            // Retrieve the OrderID
                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Insert each order line into the OrderLine table
                        foreach (var orderLine in orderLines)
                        {
                            var selectedProduct = orderLine.ComboBox.SelectedItem as KeyValuePair<int, string>?;

                            if (selectedProduct.HasValue &&
                                                    int.TryParse(orderLine.QuantityTextBox.Text, out int quantity))

                            {
                                int productId = selectedProduct.Value.Key;
                                string insertOrderLineQuery = @"
                            INSERT INTO tbl_OrderLine (OrderID, ProductID, Quantity)
                            VALUES (@OrderID, @ProductID, @Quantity);";

                                using (SqlCommand cmd = new SqlCommand(insertOrderLineQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                                    cmd.Parameters.AddWithValue("@ProductID", productId);
                                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid data in one or more order lines. Please check and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();
                        MessageBox.Show("Order submitted successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while submitting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



      
        private void AddProductLine(FlowLayoutPanel flowLayoutPanel, TextBox txtTotalBill)
        {
            // Create a panel for the order line
            Panel orderLinePanel = new Panel
            {
                Width = flowLayoutPanel.Width - 50, // Leave space for the scrollbar
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Create Label for Product ID
            Label lblProductId = new Label
            {
                Text = "Product ID:",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            // Create ComboBox for Product ID
            ComboBox cmbProductId = new ComboBox
            {
                Width = 150,
                Location = new System.Drawing.Point(100, 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Populate Product IDs in ComboBox
            PopulateProductIds(cmbProductId);

            // Create Label for Product Name
            Label lblProductName = new Label
            {
                Text = "Product Name:",
                Location = new System.Drawing.Point(10, 40),
                AutoSize = true
            };

            // Create TextBox for Product Name
            TextBox txtProductName = new TextBox
            {
                Width = 150,
                Location = new System.Drawing.Point(100, 40),
                ReadOnly = true
            };

            // Create Label for Price
            Label lblPrice = new Label
            {
                Text = "Price:",
                Location = new System.Drawing.Point(270, 10),
                AutoSize = true
            };

            // Create TextBox for Price
            TextBox txtPrice = new TextBox
            {
                Width = 100,
                Location = new System.Drawing.Point(320, 10),
                ReadOnly = true
            };

            // Create Label for Quantity
            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new System.Drawing.Point(270, 40),
                AutoSize = true
            };

            // Create TextBox for Quantity
            TextBox txtQuantity = new TextBox
            {
                Width = 150,
                Location = new System.Drawing.Point(330, 50),
                PlaceholderText = "Enter quantity"
            };
            // Create TextBox for Total Price
            TextBox txtTotalPrice = new TextBox
            {
                Width = 150,
                Location = new System.Drawing.Point(520, 50),
                ReadOnly = true // Make this field read-only since it will be calculated
            };
            txtQuantity.TextChanged += (sender, e) =>
            {
                // Calculate total price when quantity changes
                if (decimal.TryParse(txtQuantity.Text, out decimal quantity) &&
                    decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    txtTotalPrice.Text = (quantity * price).ToString("F2"); // Format as currency
                }
                else
                {
                    txtTotalPrice.Text = "0.00"; // Default value if input is invalid
                }
            };

            txtTotalPrice.TextChanged += (sender, e) =>
            {
                UpdateTotalBill(txtTotalBill);
            };

            // Add controls to the panel
            orderLinePanel.Controls.Add(lblProductId);
            orderLinePanel.Controls.Add(cmbProductId);
            orderLinePanel.Controls.Add(lblProductName);
            orderLinePanel.Controls.Add(txtProductName);
            orderLinePanel.Controls.Add(lblPrice);
            orderLinePanel.Controls.Add(txtPrice);
            orderLinePanel.Controls.Add(lblQuantity);
            orderLinePanel.Controls.Add(txtQuantity);
            orderLinePanel.Controls.Add(txtTotalPrice);


            // Add the panel to the FlowLayoutPanel
            flowLayoutPanel.Controls.Add(orderLinePanel);

            // Add this order line to the list
            var orderLine = new OrderLine
            {
                ComboBox = cmbProductId,
                ProductNameTextBox = txtProductName,
                PriceTextBox = txtPrice,
                QuantityTextBox = txtQuantity
            };
            orderLines.Add(orderLine);

            // Handle Product Selection to Populate Product Name and Price
            cmbProductId.SelectedIndexChanged += (sender, e) => OnProductSelected(orderLine);
        }

        private void OnProductSelected(OrderLine orderLine)
        {

            // Get the selected product ID
            var selectedProduct = orderLine.ComboBox.SelectedItem as KeyValuePair<int, string>?;
            if (selectedProduct == null) return;

            int productId = selectedProduct.Value.Key;

            // Fetch the product details from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductName, Price FROM tbl_Product WHERE ProductID = @ProductID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Populate Product Name and Price in the TextBoxes
                    orderLine.ProductNameTextBox.Text = reader["ProductName"].ToString();
                    orderLine.PriceTextBox.Text = reader["Price"].ToString();
                }

                reader.Close();
            }
        }

        private void UpdateTotalBill(TextBox txtTotalBill)
        {
            decimal totalBill = 0;

            // Iterate through all the order lines
            foreach (var orderLine in orderLines)
            {
                if (decimal.TryParse(orderLine.PriceTextBox.Text, out decimal unitPrice) &&
                    decimal.TryParse(orderLine.QuantityTextBox.Text, out decimal quantity))
                {
                    // Calculate the total price for the current order line
                    decimal totalPrice = unitPrice * quantity;

                    // Update the Total Price TextBox for this order line
                    //orderLine.PriceTextBox.Text = totalPrice.ToString("F2");

                    // Add to the total bill
                    totalBill += totalPrice;
                }
            }

            // Update the Total Bill TextBox
            txtTotalBill.Text = totalBill.ToString("F2");
        }


        //Populate ComboBox with product IDs from the database
        private void PopulateProductIds(ComboBox cmbProductId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductID, ProductName FROM tbl_Product"; // Assuming you have a Products table
                SqlCommand cmd = new SqlCommand(query, connection);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbProductId.Items.Add(new KeyValuePair<int, string>((int)reader["ProductID"], reader["ProductName"].ToString()));
                }
                cmbProductId.DisplayMember = "Key"; // Display the ProductName
                cmbProductId.ValueMember = "Key";    // Use ProductID as the value

                reader.Close();
            }
        }

        // Handle the product selection event to show the name and price based on Product ID
        private void OnProductSelected(object sender, EventArgs e, ComboBox cmbProductId)
        {
            var selectedProduct = (KeyValuePair<int, string>)cmbProductId.SelectedItem;
            int productId = selectedProduct.Key;

            // Find the corresponding TextBox for Product Name and Price
            var orderLine = orderLines.Find(ol => ol.ComboBox == cmbProductId);
            if (orderLine != null)
            {
                // Fetch the product name and price from the database using the product ID
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ProductName, Price FROM tbl_Product WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        MessageBox.Show(reader["Price"].ToString());

                         orderLine.ProductNameTextBox.Text = reader["ProductName"].ToString();
                        orderLine.PriceTextBox.Text = reader["Price"].ToString();
                    }

                    reader.Close();
                }
            }
        }



        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OrderLinecs_Load(object sender, EventArgs e)
        {

        }
    }

    // Helper class to represent each order line
    public class OrderLine
    {
        public ComboBox ComboBox { get; set; }
        public TextBox ProductNameTextBox { get; set; }
        public TextBox PriceTextBox { get; set; }
        public TextBox QuantityTextBox { get; set; }
    }
}
