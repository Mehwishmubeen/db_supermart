namespace WinFormsApp1
{
    partial class OrderLinecs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            addmorebutton = new Button();
            generatebillbutton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // addmorebutton
            // 
            addmorebutton.Location = new Point(0, 0);
            addmorebutton.Name = "addmorebutton";
            addmorebutton.Size = new Size(75, 23);
            addmorebutton.TabIndex = 0;
            // 
            // generatebillbutton
            // 
            generatebillbutton.Location = new Point(0, 0);
            generatebillbutton.Name = "generatebillbutton";
            generatebillbutton.Size = new Size(75, 23);
            generatebillbutton.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(0, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(200, 98);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.Paint += flowLayoutPanel1_Paint;
            // 
            // OrderLinecs
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1147, 450);
            Controls.Add(flowLayoutPanel1);
            Name = "OrderLinecs";
            Load += OrderLinecs_Load;
            ResumeLayout(false);
        }

        #endregion

        //private Label ProductIDlabel;
        //private Label productNamelabel;
        //private Label quantitylabel;
        //private Label pricelabel;
        //private TextBox textBox1;
        //private TextBox textBox3;
        //private TextBox textBox4;
        private ComboBox comboBoxproductID;
        private Button addmorebutton;
        private Button generatebillbutton;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}