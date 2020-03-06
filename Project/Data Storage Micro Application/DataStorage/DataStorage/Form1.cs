using ProjectXML;
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
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ISProj_DB
{

    public partial class Form1 : Form
    {

        string connectionString = Properties.Settings.Default.connectionString;
        SqlConnection conn = null;

        string xsdPath = "prjIS.xsd";
        //MqttClient m_cClient = new MqttClient(IPAddress.Parse("192.168.237.155"));
        MqttClient m_cClient = null;
        string[] m_strTopicsInfo = { "BridgeBrokerIS2019/20", "AlarmsChannel" };
        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            try
            {

                conn = new SqlConnection(connectionString);
                conn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Server is currently down.");

            }

            connect();
        }

        private void connect()
        {
            try
            {
                m_cClient = new MqttClient("test.mosquitto.org");
                m_cClient.Subscribe(m_strTopicsInfo, qosLevels);
                m_cClient.Connect(Guid.NewGuid().ToString());
                if (!m_cClient.IsConnected)
                {
                    Console.WriteLine("error");
                    return;
                }
                //Subscribe chat channel
                m_cClient.MqttMsgPublishReceived += M_cClient_MqttMsgPublishReceived;
                if (m_cClient.IsConnected)
                    Console.WriteLine("connected");
                else
                    Console.WriteLine("Disconnected");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error.Unable to connect to the server:(\n.Pls restart the application!");
                listMessages.Items.Add("Sorry for the inconvininece :(.Pls restart the application!");
                Console.WriteLine(e.Message);
                start.Enabled = false;
                return;
            }



            listMessages.Items.Add("Waiting for a message");


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cClient.IsConnected)
            {
                m_cClient.Unsubscribe(m_strTopicsInfo);
                m_cClient.Disconnect(); //Free process and process's resources
            }
            if (conn != null)
            {
                conn.Close();
            }
        }





        private void M_cClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {



            String xmlString = Encoding.UTF8.GetString(e.Message);


            string result = validate(xmlString);
            if (!(result == "Mensagem Valida!"))
            {
                if (listMessages.InvokeRequired)
                {

                    return;

                }

            }



            if (e.Topic == "AlarmsChannel")
            {


                Dictionary<string, string> map = saveToXml(xmlString);
                //ver se a data é valida

                try
                {
                    DateTime dateTime = DateTime.Parse(map["timestamp"].ToString());
                }
                catch (Exception execption)
                {
                    listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Error related to the date recieved:" + execption.Message); }));
                    return;
                }

                storeInDBAlarm(map);

            }




            if (e.Topic == "BridgeBrokerIS2019/20")
            {


                Dictionary<string, string> map = saveToXml(xmlString);

                try
                {
                    DateTime dateTime = DateTime.Parse(map["timestamp"].ToString());
                }
                catch (Exception exception)
                {
                    listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Error:" + exception.Message); }));
                    return;
                }

                storeInDBData(map);


            }







        }


        private void storeInDBAlarm(Dictionary<string, string> map)
        {


            SqlCommand cmd = new SqlCommand();

            #region ver se o sensorId existe
            cmd = new SqlCommand("SELECT * FROM SensorInfo WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", map["sensorId"]);
            SqlDataReader read = cmd.ExecuteReader();

            if (!read.HasRows)
            {


                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Sensor: " + map["sensorId"] + " not registered!Pls register the new sensor first!"); }));
                read.Close();
                return;
            }
            read.Close();
            #endregion

            #region Ver se os dados ja existem no SensorAlarms varios alarms ja existem para este caso
            /*
            cmd = new SqlCommand("SELECT * FROM SensorData d  JOIN Alarms a on (d.timestamp=a.timestamp) WHERE d.SensorID = @id AND d.timestamp= @time", conn);
            cmd.Parameters.AddWithValue("@id", map["sensorId"]);
            DateTime dateTime = DateTime.Parse(map["timestamp"]);
            cmd.Parameters.AddWithValue("@time", dateTime);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Dados recebidos ja estao na base de dados."); }));


                reader.Close();
                return;
            }
            reader.Close();
            */
            #endregion


            int idSensorData = 0;
            #region Ver se os dados existem no SensorData
            cmd = new SqlCommand("SELECT id FROM SensorData WHERE SensorId = @id AND timestamp=@time", conn);
            cmd.Parameters.AddWithValue("@id", map["sensorId"]);
            DateTime dateTime = DateTime.Parse(map["timestamp"]);
            cmd.Parameters.AddWithValue("@time", dateTime);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Dados recebidos ja estao na base de dados.Nao foi preciso criar no SensorData"); }));
                while (reader.Read())
                {
                    idSensorData = reader.GetInt32(0);
                }
                reader.Close();
            }
            else
            {
                reader.Close();
                #region Guardar no sensorData
                try
                {


                    //InsertIntoSensorData
                    cmd = new SqlCommand("INSERT INTO SensorData VALUES (@sensorId,@temperature,@humidity,@timestamp,@valid,@battery)", conn);
                    cmd.Parameters.AddWithValue("@sensorId", map["sensorId"]);
                    cmd.Parameters.AddWithValue("@temperature", map["temperature"].ToString() == DBNull.Value.ToString() ? (object)DBNull.Value : map["temperature"]);
                    cmd.Parameters.AddWithValue("@humidity", map["humidity"].ToString() == DBNull.Value.ToString() ? (object)DBNull.Value : map["humidity"]);
                    dateTime = DateTime.Parse(map["timestamp"]);
                    cmd.Parameters.AddWithValue("@timestamp", dateTime);
                    cmd.Parameters.AddWithValue("@valid", 1);
                    cmd.Parameters.AddWithValue("@battery", map["battery"]);

                    int result = cmd.ExecuteNonQuery();


                    if (result > 0)
                    {

                        if (listMessages.InvokeRequired)
                        {
                            listMessages.Items.Add("Data: sensorId:" + map["sensorId"] + " timestamp:" + map["timestamp"] + " temperature:" + map["temperature"] + " humidity:" + map["humidity"] + " Battery:" + map["battery"]);
                            listMessages.Items.Add("Stored data in SensorData!");
                        }
                    }


                }
                catch (Exception e)
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Error Storing SensorData.ErrorType:" + e.Message); }));

                    }

                }
                #endregion




                #region Receber o id do SensorData
                cmd = new SqlCommand("SELECT id FROM SensorData WHERE SensorId = @id AND timestamp=@time", conn);
                cmd.Parameters.AddWithValue("@id", map["sensorId"]);
                dateTime = DateTime.Parse(map["timestamp"]);
                cmd.Parameters.AddWithValue("@time", dateTime);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idSensorData = reader.GetInt32(0);
                }

                reader.Close();
                #endregion




            }

            #endregion


            #region Guardar no sensorAlarm
            try
            {
                //InsertIntoSensorData
                cmd = new SqlCommand("INSERT INTO Alarms VALUES (@sensorId,@alarms)", conn);
                cmd.Parameters.AddWithValue("@sensorId", idSensorData);
                cmd.Parameters.AddWithValue("@alarms", map["alarms"]);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    if (listMessages.InvokeRequired)
                    {
                        listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Data: sensorId:" + map["sensorId"] + " timestamp:" + map["timestamp"] + " temperature:" + map["temperature"] + " humidity:" + map["humidity"] + " Battery:" + map["battery"] + " Reason:" + map["alarms"]); })); ;
                        listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Alarms Saved!"); })); ;
                    }

                }
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Error Storing SensorData.Error:" + e.Message); }));
                }

            }
            #endregion















        }

        private string validate(string xmlString)
        {
            HandlerXML xmlHelper = new HandlerXML(xmlString, xsdPath);
            bool valid = xmlHelper.ValidateXML();
            if (!valid)
            {
                return "Mensagem Invalida! " + xmlHelper.ValidationMessage;

            }
            return "Mensagem Valida!";

        }

        private void storeInDBData(Dictionary<string, string> map)
        {

            SqlCommand cmd = new SqlCommand();

            //Mudar para so ver se ja existe 
            //update da bateria

            #region ver se o sensorId existe
            cmd = new SqlCommand("SELECT * FROM SensorInfo WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", map["sensorId"]);
            SqlDataReader read = cmd.ExecuteReader();

            if (!read.HasRows)
            {


                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Sensor:" + map["sensorId"] + " not registered!Pls register the new sensor first!"); }));
                read.Close();
                return;
            }
            read.Close();



            #endregion

            #region Ver se os dados ja existem
            cmd = new SqlCommand("SELECT * FROM SensorData WHERE sensorId = @id AND TimeStamp=@time", conn);


            cmd.Parameters.AddWithValue("@id", map["sensorId"]);
            DateTime dateTime = DateTime.Parse(map["timestamp"]);
            cmd.Parameters.AddWithValue("@time", dateTime);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Dados recebidos ja estao na base de dados."); }));
                reader.Close();
                return;
            }
            reader.Close();
            #endregion

            #region Guardar no sensorData
            try
            {


                //InsertIntoSensorData
                cmd = new SqlCommand("INSERT INTO SensorData VALUES (@sensorId,@temperature,@humidity,@timestamp,@valid,@battery)", conn);
                cmd.Parameters.AddWithValue("@sensorId", map["sensorId"]);
                cmd.Parameters.AddWithValue("@temperature", map["temperature"].ToString() == DBNull.Value.ToString() ? (object)DBNull.Value : map["temperature"]);
                cmd.Parameters.AddWithValue("@humidity", map["humidity"].ToString() == DBNull.Value.ToString() ? (object)DBNull.Value : map["humidity"]);
                dateTime = DateTime.Parse(map["timestamp"]);
                cmd.Parameters.AddWithValue("@timestamp", dateTime);
                cmd.Parameters.AddWithValue("@valid", 1);
                cmd.Parameters.AddWithValue("@battery", map["battery"]);

                int result = cmd.ExecuteNonQuery();


                if (result > 0)
                {

                    if (listMessages.InvokeRequired)
                    {
                        listMessages.Invoke(new MethodInvoker(delegate
                        {
                            listMessages.Items.Add("Data: sensorId:" + map["sensorId"] + " timestamp:" + map["timestamp"] + " temperature:" + map["temperature"] + " humidity:" + map["humidity"] + " Battery:" + map["battery"]);
                            listMessages.Items.Add("Stored data in SensorData!");
                        }));
                    }
                }


            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Error Storing SensorData.Error:" + e.Message); }));

                }

            }
            #endregion









        }

        private string getFloor(string sensorId)
        {


            if (sensorId == "1")
            {
                return "1";
            }
            if (sensorId == "2")
            {
                return "2";
            }

            return "-1";
        }


        private Dictionary<string, string> saveToXml(string xmlString)
        {

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString); // suppose that myXmlString contains "<Names>...</Names>"

            Dictionary<string, string> hash = new Dictionary<string, string>();

            XmlNodeList xnList = xml.SelectNodes("reading");

            foreach (XmlNode xn in xnList)
            {
                //rever supostamente so faz uma vez so se fosse varias temperaturas no msm

                hash.Add("sensorId", xn["id"].InnerText);
                hash.Add("temperature", xn["temperature"] == null ? DBNull.Value.ToString() : xn["temperature"].InnerText);
                hash.Add("humidity", xn["humidity"] == null ? DBNull.Value.ToString() : xn["humidity"].InnerText);
                hash.Add("battery", xn["battery"].InnerText);
                hash.Add("timestamp", xn["timestamp"].InnerText);
                hash.Add("alarms", xn["alarms"] == null ? "" : xn["alarms"].InnerText);

            }
            listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Received: sensorId:" + hash["sensorId"] + " timestamp:" + hash["timestamp"] + " temperature:" + hash["temperature"] + " humidity:" + hash["humidity"] + " Battery:" + hash["battery"]); }));


            return hash;
        }


        private void stop_Click(object sender, EventArgs e)
        {
            if (m_cClient.IsConnected)
            {

                m_cClient.Unsubscribe(m_strTopicsInfo);
                m_cClient.Disconnect(); //Free process and process's resources


            }

            if (conn != null)
            {
                conn.Close();

            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listMessages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listMessages_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_cClient != null && m_cClient.IsConnected)
            {

                m_cClient.Unsubscribe(m_strTopicsInfo);
                m_cClient.Disconnect(); //Free process and process's resources


            }

            if (conn != null)
            {
                conn.Close();

            }

        }
    }
}
