using System;
using System.Windows.Forms;

namespace Bridge
{
    public class FormBroker : Form
    {
        private Button btnConnect;
        private TextBox txtDomain;
        private Label lbDomain;
        private CheckBox cbLogging;

        public string domain { get; set; }
        public Boolean logging { get; set; }

        public FormBroker()
        {
            InitializeComponents();
        }

        #region Initialization
        private void InitializeComponents()
        {
            this.btnConnect = new Button();
            this.txtDomain = new TextBox();
            this.lbDomain = new Label();
            this.cbLogging = new CheckBox();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(225, 32);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 21);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(109, 32);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(100, 20);
            this.txtDomain.TabIndex = 1;
            this.txtDomain.Text = "test.mosquitto.org";
            // 
            // lbDomain
            // 
            this.lbDomain.AutoSize = true;
            this.lbDomain.Location = new System.Drawing.Point(23, 35);
            this.lbDomain.Name = "lbDomain";
            this.lbDomain.Size = new System.Drawing.Size(80, 13);
            this.lbDomain.TabIndex = 2;
            this.lbDomain.Text = "Broker Domain:";
            //
            // cbLogging
            //
            this.cbLogging.AutoSize = true;
            this.cbLogging.Location = new System.Drawing.Point(23, 55);
            this.cbLogging.Name = "cbLogging";
            this.cbLogging.Size = new System.Drawing.Size(75, 21);
            this.cbLogging.Text = "Enable Logging";
            this.cbLogging.UseVisualStyleBackColor = true;
            // 
            // FormBroker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 79);
            this.Controls.Add(this.lbDomain);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cbLogging);
            this.Name = "FormBroker";
            this.Text = "Broker";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtDomain.Text.Length == 0)
            {
                MessageBox.Show("Domain cannot be empty");
            }
            if (txtDomain.Text.Contains(" "))
            {
                MessageBox.Show("Domain cannot contain spaces");
            }

            logging = cbLogging.Checked;
            domain = txtDomain.Text;
            this.Close();
        }
    }
}
