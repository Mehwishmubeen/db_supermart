using System.Data.SqlClient;
using WinFormsApp1;
using WinFormsApp1.Properties;

namespace supermart
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Simple validation logic (replace with actual authentication logic)
            if (username == "admin" && password == "admin")
            {
                //  MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                OrderLinecs orders=new OrderLinecs();
  //              orders.Show();

                Product product = new Product();
                product.Show();


                // Hide the login form (optional, use Close() if you want to close it)
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void picBanner_Click(object sender, EventArgs e)
        {

        }
    }
}
