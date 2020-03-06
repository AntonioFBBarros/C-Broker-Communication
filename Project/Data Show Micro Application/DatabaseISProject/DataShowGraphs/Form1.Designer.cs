namespace DataShowGraphs
{
    partial class Form1
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
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxSearch = new System.Windows.Forms.ComboBox();
            this.comboBoxSensorId = new System.Windows.Forms.ComboBox();
            this.labelID = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.comboBoxFloor = new System.Windows.Forms.ComboBox();
            this.labelFloor = new System.Windows.Forms.Label();
            this.cartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.listMessages = new System.Windows.Forms.ListBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.SensorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Humidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Floor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelSensor = new System.Windows.Forms.Label();
            this.buttonUpdateEnabled = new System.Windows.Forms.Button();
            this.buttonUpdateDisable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAlarmsDisable = new System.Windows.Forms.Button();
            this.buttonAlarmsEnabled = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSearch.Location = new System.Drawing.Point(42, 58);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(53, 17);
            this.labelSearch.TabIndex = 0;
            this.labelSearch.Text = "Search";
            this.labelSearch.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBoxSearch
            // 
            this.comboBoxSearch.AutoCompleteCustomSource.AddRange(new string[] {
            "ID",
            "Location"});
            this.comboBoxSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearch.FormattingEnabled = true;
            this.comboBoxSearch.Items.AddRange(new object[] {
            "ID",
            "Floor/Location"});
            this.comboBoxSearch.Location = new System.Drawing.Point(101, 54);
            this.comboBoxSearch.Name = "comboBoxSearch";
            this.comboBoxSearch.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSearch.TabIndex = 1;
            this.comboBoxSearch.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearch_SelectedIndexChanged);
            // 
            // comboBoxSensorId
            // 
            this.comboBoxSensorId.AutoCompleteCustomSource.AddRange(new string[] {
            "ID",
            "Location"});
            this.comboBoxSensorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSensorId.FormattingEnabled = true;
            this.comboBoxSensorId.Items.AddRange(new object[] {
            "<---Empty--->"});
            this.comboBoxSensorId.Location = new System.Drawing.Point(99, 105);
            this.comboBoxSensorId.Name = "comboBoxSensorId";
            this.comboBoxSensorId.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSensorId.Sorted = true;
            this.comboBoxSensorId.TabIndex = 3;
            this.comboBoxSensorId.SelectedIndexChanged += new System.EventHandler(this.comboBoxSensorId_SelectedIndexChanged);
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelID.Location = new System.Drawing.Point(40, 105);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(53, 17);
            this.labelID.TabIndex = 2;
            this.labelID.Text = "Sensor";
            this.labelID.Click += new System.EventHandler(this.labelID_Click);
            // 
            // comboBoxType
            // 
            this.comboBoxType.AutoCompleteCustomSource.AddRange(new string[] {
            "Temperature",
            "Humidity"});
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Temperature",
            "Humidity",
            "Temperature/Humidity"});
            this.comboBoxType.Location = new System.Drawing.Point(293, 105);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(141, 21);
            this.comboBoxType.TabIndex = 5;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(234, 109);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(40, 17);
            this.labelType.TabIndex = 4;
            this.labelType.Text = "Type";
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.AutoCompleteCustomSource.AddRange(new string[] {
            ""});
            this.comboBoxLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Items.AddRange(new object[] {
            "<---Empty--->"});
            this.comboBoxLocation.Location = new System.Drawing.Point(532, 105);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLocation.TabIndex = 7;
            this.comboBoxLocation.SelectedIndexChanged += new System.EventHandler(this.comboBoxLocation_SelectedIndexChanged);
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLocation.Location = new System.Drawing.Point(464, 109);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(62, 17);
            this.labelLocation.TabIndex = 6;
            this.labelLocation.Text = "Location";
            this.labelLocation.Click += new System.EventHandler(this.labelLocation_Click);
            // 
            // comboBoxFloor
            // 
            this.comboBoxFloor.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2"});
            this.comboBoxFloor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFloor.FormattingEnabled = true;
            this.comboBoxFloor.Items.AddRange(new object[] {
            "<---Empty--->"});
            this.comboBoxFloor.Location = new System.Drawing.Point(99, 105);
            this.comboBoxFloor.Name = "comboBoxFloor";
            this.comboBoxFloor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFloor.Sorted = true;
            this.comboBoxFloor.TabIndex = 9;
            this.comboBoxFloor.SelectedIndexChanged += new System.EventHandler(this.comboBoxFloor_SelectedIndexChanged);
            // 
            // labelFloor
            // 
            this.labelFloor.AutoSize = true;
            this.labelFloor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFloor.Location = new System.Drawing.Point(40, 105);
            this.labelFloor.Name = "labelFloor";
            this.labelFloor.Size = new System.Drawing.Size(40, 17);
            this.labelFloor.TabIndex = 8;
            this.labelFloor.Text = "Floor";
            this.labelFloor.Click += new System.EventHandler(this.labelFloor_Click);
            // 
            // cartesianChart
            // 
            this.cartesianChart.Location = new System.Drawing.Point(45, 160);
            this.cartesianChart.Name = "cartesianChart";
            this.cartesianChart.Size = new System.Drawing.Size(1102, 276);
            this.cartesianChart.TabIndex = 10;
            this.cartesianChart.Text = "cartesianChart";
            this.cartesianChart.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.cartesianChart1_ChildChanged);
            // 
            // listMessages
            // 
            this.listMessages.FormattingEnabled = true;
            this.listMessages.HorizontalScrollbar = true;
            this.listMessages.Location = new System.Drawing.Point(62, 488);
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(415, 160);
            this.listMessages.TabIndex = 15;
            this.listMessages.SelectedIndexChanged += new System.EventHandler(this.listMessages_SelectedIndexChanged);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SensorId,
            this.timestamp,
            this.Temperature,
            this.Humidity,
            this.Floor,
            this.Location});
            this.dataGridView.Location = new System.Drawing.Point(503, 488);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(644, 160);
            this.dataGridView.TabIndex = 18;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // SensorId
            // 
            this.SensorId.HeaderText = "Id";
            this.SensorId.MinimumWidth = 10;
            this.SensorId.Name = "SensorId";
            this.SensorId.ReadOnly = true;
            // 
            // timestamp
            // 
            this.timestamp.HeaderText = "Timestamp";
            this.timestamp.Name = "timestamp";
            this.timestamp.ReadOnly = true;
            this.timestamp.Width = 105;
            // 
            // Temperature
            // 
            this.Temperature.HeaderText = "Temperature";
            this.Temperature.Name = "Temperature";
            this.Temperature.ReadOnly = true;
            // 
            // Humidity
            // 
            this.Humidity.HeaderText = "Humidity";
            this.Humidity.Name = "Humidity";
            this.Humidity.ReadOnly = true;
            // 
            // Floor
            // 
            this.Floor.HeaderText = "Floor";
            this.Floor.Name = "Floor";
            this.Floor.ReadOnly = true;
            // 
            // Location
            // 
            this.Location.HeaderText = "Location";
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            // 
            // labelSensor
            // 
            this.labelSensor.AutoSize = true;
            this.labelSensor.Location = new System.Drawing.Point(500, 463);
            this.labelSensor.Name = "labelSensor";
            this.labelSensor.Size = new System.Drawing.Size(66, 13);
            this.labelSensor.TabIndex = 19;
            this.labelSensor.Text = "Sensor Data";
            // 
            // buttonUpdateEnabled
            // 
            this.buttonUpdateEnabled.Location = new System.Drawing.Point(944, 57);
            this.buttonUpdateEnabled.Name = "buttonUpdateEnabled";
            this.buttonUpdateEnabled.Size = new System.Drawing.Size(82, 33);
            this.buttonUpdateEnabled.TabIndex = 20;
            this.buttonUpdateEnabled.Text = "Enabled";
            this.buttonUpdateEnabled.UseVisualStyleBackColor = true;
            this.buttonUpdateEnabled.Click += new System.EventHandler(this.buttonUpdateEnabled_Click);
            // 
            // buttonUpdateDisable
            // 
            this.buttonUpdateDisable.Location = new System.Drawing.Point(1085, 57);
            this.buttonUpdateDisable.Name = "buttonUpdateDisable";
            this.buttonUpdateDisable.Size = new System.Drawing.Size(82, 33);
            this.buttonUpdateDisable.TabIndex = 21;
            this.buttonUpdateDisable.Text = "Disable";
            this.buttonUpdateDisable.UseVisualStyleBackColor = true;
            this.buttonUpdateDisable.Click += new System.EventHandler(this.buttonUpdateDisable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(995, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "Automatic Update";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(740, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Only Alarms";
            // 
            // buttonAlarmsDisable
            // 
            this.buttonAlarmsDisable.Location = new System.Drawing.Point(825, 57);
            this.buttonAlarmsDisable.Name = "buttonAlarmsDisable";
            this.buttonAlarmsDisable.Size = new System.Drawing.Size(82, 33);
            this.buttonAlarmsDisable.TabIndex = 24;
            this.buttonAlarmsDisable.Text = "Disable";
            this.buttonAlarmsDisable.UseVisualStyleBackColor = true;
            this.buttonAlarmsDisable.Click += new System.EventHandler(this.buttonAlarmsDisable_Click);
            // 
            // buttonAlarmsEnabled
            // 
            this.buttonAlarmsEnabled.Location = new System.Drawing.Point(684, 57);
            this.buttonAlarmsEnabled.Name = "buttonAlarmsEnabled";
            this.buttonAlarmsEnabled.Size = new System.Drawing.Size(82, 33);
            this.buttonAlarmsEnabled.TabIndex = 23;
            this.buttonAlarmsEnabled.Text = "Enabled";
            this.buttonAlarmsEnabled.UseVisualStyleBackColor = true;
            this.buttonAlarmsEnabled.Click += new System.EventHandler(this.buttonAlarmsEnabled_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 703);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAlarmsDisable);
            this.Controls.Add(this.buttonAlarmsEnabled);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUpdateDisable);
            this.Controls.Add(this.buttonUpdateEnabled);
            this.Controls.Add(this.labelSensor);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.listMessages);
            this.Controls.Add(this.cartesianChart);
            this.Controls.Add(this.comboBoxFloor);
            this.Controls.Add(this.labelFloor);
            this.Controls.Add(this.comboBoxLocation);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.comboBoxSensorId);
            this.Controls.Add(this.labelID);
            this.Controls.Add(this.comboBoxSearch);
            this.Controls.Add(this.labelSearch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.ComboBox comboBoxSearch;
        private System.Windows.Forms.ComboBox comboBoxSensorId;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxLocation;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.ComboBox comboBoxFloor;
        private System.Windows.Forms.Label labelFloor;
        private LiveCharts.WinForms.CartesianChart cartesianChart;
        private System.Windows.Forms.ListBox listMessages;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label labelSensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SensorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn Humidity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Floor;
        private new System.Windows.Forms.DataGridViewTextBoxColumn Location;
        private System.Windows.Forms.Button buttonUpdateEnabled;
        private System.Windows.Forms.Button buttonUpdateDisable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAlarmsDisable;
        private System.Windows.Forms.Button buttonAlarmsEnabled;
    }
}

