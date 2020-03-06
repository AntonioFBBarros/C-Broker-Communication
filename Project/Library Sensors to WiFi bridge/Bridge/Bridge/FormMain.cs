using System;
using System.Threading;
using System.Windows.Forms;

namespace Bridge
{
    public class FormMain : Form
    {

        FileManager fileManager = FileManager.getInstance();
        Bridge bridge = new Bridge();
        BrokerManager brokerManager;
        private Thread thread;

        private static String[] MANDATORYITEMS = { "id/sbyte", "battery/sbyte", "timestamp/uint32" };

        public FormMain()
        {
            InitializeComponents();
        }

        #region Initialization
        private void InitializeComponents()
        {
            this.lbFile = new Label();
            this.lbTitleBacklog = new Label();
            this.lbTitleParameters = new Label();
            this.btnOpen = new Button();
            this.btnAdd = new Button();
            this.btnRemove = new Button();
            this.btnMoveRight = new Button();
            this.btnMoveLeft = new Button();
            this.btnMoveUp = new Button();
            this.btnMoveDown = new Button();
            this.btnStop = new Button();
            this.btnStart = new Button();
            this.btnHelp = new Button();
            this.cbOptionAll = new CheckBox();
            this.cbOptionNew = new CheckBox();
            this.listBacklog = new ListBox();
            this.listParameters = new ListBox();
            this.txtFile = new TextBox();
            this.SuspendLayout();

            #region Labels
            //***********************************
            //************* lbFile **************
            //***********************************
            this.lbFile.AutoSize = true;
            this.lbFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFile.Location = new System.Drawing.Point(35, 27);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(43, 20);
            this.lbFile.Text = "File:";
            //***********************************
            //********* lbTitleBacklog **********
            //***********************************
            this.lbTitleBacklog.AutoSize = true;
            this.lbTitleBacklog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitleBacklog.Location = new System.Drawing.Point(140, 64);
            this.lbTitleBacklog.Name = "lbTitleBacklog";
            this.lbTitleBacklog.Size = new System.Drawing.Size(73, 20);
            this.lbTitleBacklog.Text = "Backlog";
            //***********************************
            //******* lbTitleParameters *********
            //***********************************
            this.lbTitleParameters.AutoSize = true;
            this.lbTitleParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitleParameters.Location = new System.Drawing.Point(462, 64);
            this.lbTitleParameters.Name = "lbTitleParameters";
            this.lbTitleParameters.Size = new System.Drawing.Size(135, 20);
            this.lbTitleParameters.Text = "File Parameters";
            #endregion

            #region Buttons
            //***********************************
            //************ btnOpen **************
            //***********************************
            this.btnOpen.Location = new System.Drawing.Point(637, 25);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            //***********************************
            //************* btnAdd **************
            //***********************************
            this.btnAdd.Location = new System.Drawing.Point(23, 99);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(47, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //***********************************
            //*********** btnRemove *************
            //***********************************
            this.btnRemove.Location = new System.Drawing.Point(23, 141);
            this.btnRemove.Name = "BtnRemove";
            this.btnRemove.Size = new System.Drawing.Size(47, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            //***********************************
            //********* btnMoveRight ************
            //***********************************
            this.btnMoveRight.Location = new System.Drawing.Point(321, 141);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(75, 23);
            this.btnMoveRight.TabIndex = 6;
            this.btnMoveRight.Text = ">>";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            //***********************************
            //********** btnMoveLeft ************
            //***********************************
            this.btnMoveLeft.Location = new System.Drawing.Point(321, 99);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(75, 23);
            this.btnMoveLeft.TabIndex = 5;
            this.btnMoveLeft.Text = "<<";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            //***********************************
            //*********** btnMoveUp *************
            //***********************************
            this.btnMoveUp.Location = new System.Drawing.Point(651, 99);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(50, 23);
            this.btnMoveUp.TabIndex = 7;
            this.btnMoveUp.Text = "Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            //***********************************
            //********** btnMoveDown ************
            //***********************************
            this.btnMoveDown.Location = new System.Drawing.Point(651, 141);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(50, 23);
            this.btnMoveDown.TabIndex = 8;
            this.btnMoveDown.Text = "Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            //***********************************
            //************ btnStop **************
            //***********************************
            this.btnStop.Location = new System.Drawing.Point(522, 352);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Enabled = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            //***********************************
            //*********** btnStart **************
            //***********************************
            this.btnStart.Location = new System.Drawing.Point(610, 352);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Enabled = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //***********************************
            //************ btnHelp **************
            //***********************************
            this.btnHelp.Location = new System.Drawing.Point(20, 352);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(25, 25);
            this.btnHelp.Text = "?";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            #endregion

            #region CheckBoxs
            //***********************************
            //*********** cbOptionAll ***********
            //***********************************
            this.cbOptionAll.AutoSize = true;
            this.cbOptionAll.Location = new System.Drawing.Point(400, 356);
            this.cbOptionAll.Name = "cbOptionAll";
            this.cbOptionAll.Size = new System.Drawing.Size(80, 20);
            this.cbOptionAll.Text = "Read complete File";
            this.cbOptionAll.UseVisualStyleBackColor = true;
            //***********************************
            //********** cbOptionLast ***********
            //***********************************
            this.cbOptionNew.AutoSize = true;
            this.cbOptionNew.Location = new System.Drawing.Point(300, 356);
            this.cbOptionNew.Name = "cbOptionNew";
            this.cbOptionNew.Size = new System.Drawing.Size(80, 20);
            this.cbOptionNew.Text = "Only read new";
            this.cbOptionNew.UseVisualStyleBackColor = true;
            #endregion

            #region RichTextBoxs
            //***********************************
            //*********** listBacklog ***********
            //***********************************
            this.listBacklog.Location = new System.Drawing.Point(96, 99);
            this.listBacklog.Name = "rtbBacklog";
            this.listBacklog.Size = new System.Drawing.Size(205, 192);
            this.listBacklog.Text = "";
            loadBacklog();
            //***********************************
            //********* listParameters **********
            //***********************************
            this.listParameters.Location = new System.Drawing.Point(424, 99);
            this.listParameters.Name = "rtbParameters";
            this.listParameters.Size = new System.Drawing.Size(205, 192);
            this.listParameters.Text = "";
            loadParameters();
            #endregion

            #region TextBoxs
            //***********************************
            //************ txtFile **************
            //***********************************
            this.txtFile.Location = new System.Drawing.Point(102, 27);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(520, 20);
            this.txtFile.ReadOnly = true;
            #endregion

            #region FormMain
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 400);
            this.Name = "MyForm";
            this.Text = "Library Sensors to WiFi bridge";
            this.Controls.Add(lbFile);
            this.Controls.Add(lbTitleBacklog);
            this.Controls.Add(lbTitleParameters);
            this.Controls.Add(btnOpen);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnRemove);
            this.Controls.Add(btnMoveRight);
            this.Controls.Add(btnMoveLeft);
            this.Controls.Add(btnMoveUp);
            this.Controls.Add(btnMoveDown);
            this.Controls.Add(btnStop);
            this.Controls.Add(btnStart);
            this.Controls.Add(btnHelp);
            this.Controls.Add(txtFile);
            this.Controls.Add(cbOptionAll);
            this.Controls.Add(cbOptionNew);
            this.Controls.Add(listBacklog);
            this.Controls.Add(listParameters);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MyForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();
            #endregion
        }
        #endregion

        private void btnOpen_Click(object sender, EventArgs e)
        {
            fileManager.selectFile();
            txtFile.Text = fileManager.FilePath;
            btnStart.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var formAdd = new FormAdd())
            {
                formAdd.ShowDialog();
                if(formAdd.item == null)
                {
                    return;
                }
                listBacklog.Items.Add(formAdd.item);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbOptionAll.Checked && cbOptionNew.Checked)
            {
                MessageBox.Show("Can only have 1 option selected at a time");
                return;
            }

            brokerManager = new BrokerManager();
            if (brokerManager.domain == null)
            {
                return;
            }
            bridge.brokerManager = brokerManager;

            updatedApplication(true);

            if (cbOptionAll.Checked)
            {
                bridge.lastLine = 0;
            }
            else
            {
                if (cbOptionNew.Checked)
                {
                    bridge.lastLine = fileManager.fileSize();
                }
                else
                {
                    bridge.lastLine = Convert.ToInt64(FileManager.readTxtFile("checkpoint.txt"));
                }
            }

            thread = new Thread(bridge.start);
            thread.Start(listParameters);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            updatedApplication(false);
        }

        private void updatedApplication(Boolean running)
        {
            bridge.READING = running;

            cbOptionAll.Enabled = !running;
            cbOptionNew.Enabled = !running;
            btnOpen.Enabled = !running;
            btnStop.Enabled = running;
            btnStart.Enabled = !running;
            btnMoveLeft.Enabled = !running;
            btnMoveRight.Enabled = !running;
            btnMoveUp.Enabled = !running;
            btnMoveDown.Enabled = !running;
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            if (listBacklog.SelectedItem == null)
            {
                return;
            }

            string aux = listBacklog.SelectedItem.ToString();
            listBacklog.Items.Remove(listBacklog.SelectedItem);
            listParameters.Items.Add(aux);
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            if (listParameters.SelectedItem == null)
            {
                return;
            }

            string aux = listParameters.SelectedItem.ToString();
            foreach (string item in MANDATORYITEMS)
            {
                if (aux.Equals(item))
                {
                    MessageBox.Show(item + ": is mandatory");
                    return;
                }
            }

            listParameters.Items.Remove(listParameters.SelectedItem);
            listBacklog.Items.Add(aux);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (listParameters.SelectedItem == null)
            {
                return;
            }

            object aux = listParameters.SelectedItem;
            int postion = listParameters.FindString(aux.ToString());

            if (postion == 0)
            {
                return;
            }

            listParameters.Items.RemoveAt(postion);
            listParameters.Items.Insert(postion - 1, aux);
            listParameters.SelectedIndex = postion - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (listParameters.SelectedItem == null)
            {
                return;
            }

            object aux = listParameters.SelectedItem;
            int postion = listParameters.FindString(aux.ToString());

            if (postion == listParameters.Items.Count - 1)
            {
                return;
            }

            listParameters.Items.RemoveAt(postion);
            listParameters.Items.Insert(postion + 1, aux);
            listParameters.SelectedIndex = postion + 1;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listBacklog.Items.Remove(listBacklog.SelectedItem);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            using (var formHelp = new FormHelp())
            {
                formHelp.ShowDialog();
            }
        }

        private void saveLists()
        {
            FileManager.writeCollectionToFile("backlog.txt", listBacklog.Items);
            FileManager.writeCollectionToFile("parameters.txt", listParameters.Items);
        }


        private void loadBacklog()
        {
            string file = FileManager.readTxtFile("backlog.txt");
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            file = file.Remove(file.Length - 1); //removes last /n
            foreach (string item in file.Split('\n'))
            {
                listBacklog.Items.Add(item);
            }

        }

        private void loadParameters()
        {
            string file = FileManager.readTxtFile("parameters.txt");
            if (string.IsNullOrEmpty(file))
            {
                listParameters.Items.Add("id/sbyte/4");
                listParameters.Items.Add("temperature/single/4");
                listParameters.Items.Add("humidity/single/4");
                listParameters.Items.Add("battery/sbyte/4");
                listParameters.Items.Add("timestamp/uint32/4");
                listParameters.Items.Add("Empty/Empty/4");
                return;
            }

            if (file.Length > 0)
            {
                file = file.Remove(file.Length - 1); //removes last /n
                foreach (string item in file.Split('\n'))
                {
                    listParameters.Items.Add(item);
                }
            }
        }

        private void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            updatedApplication(false);
            saveLists();
        }

        private Label lbFile;
        private Label lbTitleBacklog;
        private Label lbTitleParameters;
        private Button btnOpen;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnMoveRight;
        private Button btnMoveLeft;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnStop;
        private Button btnStart;
        private Button btnHelp;
        private TextBox txtFile;
        private CheckBox cbOptionAll;
        private CheckBox cbOptionNew;
        private ListBox listBacklog;
        private ListBox listParameters;
    }
}
