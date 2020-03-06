using System;
using System.Windows.Forms;

namespace Bridge
{
    public class FormAdd : Form
    {
        private Label lbName;
        private TextBox txtName;
        private ComboBox cbType;
        private Label lbType;
        private ComboBox cbBytes;
        private Label lbBytes;
        private Button btnAdd;
        private Button btnCancel;

        public string item { get; set; }

        public FormAdd()
        {
            InitializeComponents();
        }


        #region Initialization
        private void InitializeComponents()
        {
            this.lbName = new Label();
            this.txtName = new TextBox();
            this.cbType = new ComboBox();
            this.cbBytes = new ComboBox();
            this.lbBytes = new Label();
            this.lbType = new Label();
            this.btnAdd = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(30, 34);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(81, 31);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 1;
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(268, 30);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 21);
            this.cbType.TabIndex = 2;
            this.cbType.Items.Add("sbyte");
            this.cbType.Items.Add("byte");
            this.cbType.Items.Add("int16");
            this.cbType.Items.Add("uint16");
            this.cbType.Items.Add("int32");
            this.cbType.Items.Add("uint32");
            this.cbType.Items.Add("int64");
            this.cbType.Items.Add("uint64");
            this.cbType.Items.Add("single");
            this.cbType.Items.Add("double");
            this.cbType.Items.Add("Empty");
            this.cbType.SelectedIndex = 1;
            this.cbType.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(205, 34);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(34, 13);
            this.lbType.TabIndex = 3;
            this.lbType.Text = "Type:";
            // 
            // lbBytes
            // 
            this.lbBytes.AutoSize = true;
            this.lbBytes.Location = new System.Drawing.Point(410, 34);
            this.lbBytes.Name = "lbBytes";
            this.lbBytes.Size = new System.Drawing.Size(34, 13);
            this.lbBytes.Text = "Bytes:";
            // 
            // cbBytes
            // 
            this.cbBytes.FormattingEnabled = true;
            this.cbBytes.Location = new System.Drawing.Point(460, 30);
            this.cbBytes.Name = "cbType";
            this.cbBytes.Size = new System.Drawing.Size(121, 21);
            this.cbBytes.Items.Add("1");
            this.cbBytes.Items.Add("2");
            this.cbBytes.Items.Add("4");
            this.cbBytes.Items.Add("8");
            this.cbBytes.SelectedIndex = 2;
            this.cbBytes.DropDownStyle = ComboBoxStyle.DropDownList;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(339, 75);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(258, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 110);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.lbBytes);
            this.Controls.Add(this.cbBytes);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lbName);
            this.Name = "FormAdd";
            this.Text = "Add Parameter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Equals(""))
            {
                MessageBox.Show("item name is required");
                return;
            }
            if (txtName.Text.Contains(" "))
            {
                MessageBox.Show("item name cannot contain spaces");
                return;
            }
            if (txtName.Text.Contains("/"))
            {
                MessageBox.Show("item name cannot contain '/'");
                return;
            }
            if (txtName.Text.Length > 20)
            {
                MessageBox.Show("item name is to large");
                return;
            }
            if (cbType.SelectedItem.ToString().Equals("int64") || cbType.SelectedItem.ToString().Equals("uint64") || cbType.SelectedItem.ToString().Equals("double"))
            {
                if (!cbBytes.SelectedItem.ToString().Equals("8"))
                {
                    MessageBox.Show("Invalid number of bytes for given type (should be 8 bytes for 64 bit types)");
                    return;
                }
            }

            if (cbType.SelectedItem.ToString().Equals("int32") || cbType.SelectedItem.ToString().Equals("uint32") || cbType.SelectedItem.ToString().Equals("single"))
            {
                if (!cbBytes.SelectedItem.ToString().Equals("4") && !cbBytes.SelectedItem.ToString().Equals("8"))
                {
                    MessageBox.Show("Invalid number of bytes for given type (should be 4 or 8 bytes for 32 bit types)");
                    return;
                }
            }

            if (cbType.SelectedItem.ToString().Equals("int16") || cbType.SelectedItem.ToString().Equals("uint16"))
            {
                if (cbBytes.SelectedItem.ToString().Equals("1"))
                {
                    MessageBox.Show("Invalid number of bytes for given type (should be 2, 4 or 8 bytes for 16 bit types)");
                    return;
                }
            }

            item = txtName.Text + "/" + cbType.SelectedItem.ToString() + "/" + cbBytes.SelectedItem.ToString();
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
