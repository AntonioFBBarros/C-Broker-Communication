using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using ProjectXML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DataShowGraphs
{
    public partial class Form1 : Form
    {

        MqttClient m_cClient = null;
        string[] m_strTopicsInfo = { "BridgeBrokerIS2019/20", "AlarmsChannel" };
        string xsdPath = "prjIS.xsd";
        byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS
        List<Sensor> sensors = new List<Sensor>();
        List<Sensor> alarms = new List<Sensor>();
        List<Place> places = new List<Place>();


        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxFloor.Visible = false;
            comboBoxLocation.Visible = false;
            labelLocation.Visible = false;
            labelFloor.Visible = false;
            comboBoxSearch.SelectedIndex = 0;
            comboBoxType.SelectedIndex = 0;
            comboBoxSensorId.SelectedIndex = 0;
            comboBoxFloor.SelectedIndex = 0;
            comboBoxLocation.SelectedIndex = 0;
            buttonUpdateEnabled.Enabled = true;
            buttonUpdateEnabled.Enabled = false;

            buttonAlarmsEnabled.Enabled = true;
            buttonAlarmsDisable.Enabled = false;

            connect();


        }

        private void connect()
        {
            try
            {
                m_cClient = new MqttClient("test.mosquitto.org");
                m_cClient.Connect(Guid.NewGuid().ToString());
                if (!m_cClient.IsConnected)
                {
                    Console.WriteLine("error");
                    return;
                }

                //Subscribe chat channel
                m_cClient.MqttMsgPublishReceived += M_cClient_MqttMsgPublishReceived;


                m_cClient.Subscribe(m_strTopicsInfo, qosLevels);

                if (m_cClient.IsConnected)
                    Console.WriteLine("connected");
                else
                    Console.WriteLine("Disconnected");

            }
            catch (Exception e)
            {
                //por mensagem
                MessageBox.Show("Error.Unable to connect to the server:(\n.Pls restart the application!");
                Console.WriteLine(e.Message);
                return;
            }


        }

        private void M_cClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Received Message:");


            String xmlString = Encoding.UTF8.GetString(e.Message);

            string result = validate(xmlString);

            if (result != "Mensagem Valida!")
            {

                return;
            }


            if (e.Topic == "AlarmsChannel")
            {
                addSensor(xmlString);
                if (buttonAlarmsDisable.Enabled)
                {
                    if (!buttonUpdateEnabled.Enabled)
                    {
                        draw();
                    }
                    populateDataGrid();
                }


            }




            if (e.Topic == "BridgeBrokerIS2019/20")
            {

                addSensor(xmlString);
                if (!buttonAlarmsDisable.Enabled)
                {

                    if (!buttonUpdateEnabled.Enabled)
                    {
                        draw();
                    }
                    populateDataGrid();
                }


            }

        }

        private void addSensor(string xmlString)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString); // suppose that myXmlString contains "<Names>...</Names>"


            XmlNodeList xnList = xml.SelectNodes("reading");
            string id = "";
            string battery = "";
            string humidity = "";
            string temperature = "";
            string floor = "";
            DateTime timeStamp = new DateTime();
            string reason = "";



            foreach (XmlNode xn in xnList)
            {
                //rever
                try
                {
                    timeStamp = DateTime.Parse(xn["timestamp"].InnerText);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                id = xn["id"].InnerText;
                battery = xn["battery"].InnerText;
                humidity = xn["humidity"] == null ? "" : xn["humidity"].InnerText;
                floor = xn["id"].InnerText == "1" ? "1" : "2";//mudar para algo tipo get floor  ou alguma coisa que faça para me dizer os floors
                temperature = xn["temperature"] == null ? "" : xn["temperature"].InnerText;
                reason = xn["alarms"] == null ? "" : xn["alarms"].InnerText;

            }

            Sensor sensor = new Sensor(id, battery, timeStamp, humidity, temperature, floor, "Biblioteca", reason);





            preencherComboBox(sensor.Id, sensor.Floor, sensor.Location);


            if (buttonAlarmsEnabled.Enabled == false)
            {
                //switch nas listas tenho de verificar aqui para quando recebo qd estou nos alarmes fazer ao contrário
                if (reason == "")
                {
                    alarms.Add(sensor);//lista trocado por isso é um dado normal

                    listMessages.Invoke(new MethodInvoker(delegate
                    {
                        listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Received: sensorId:" + sensor.Id + " timestamp:" + sensor.TimeStamp + " temperature:" + sensor.Temperature + " humidity:" + sensor.Humidity + " Battery:" + sensor.Battery); }));
                    }));
                    return;

                }
                sensors.Add(sensor);
                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Alerta Recebido:"); }));
                listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Received: sensorId:" + sensor.Id + " timestamp:" + sensor.TimeStamp + " temperature:" + sensor.Temperature + " humidity:" + sensor.Humidity + " Battery:" + sensor.Battery + " Reason:" + sensor.Reason); }));
                return;
            }


            if (reason == "")
            {

                sensors.Add(sensor);


                listMessages.Invoke(new MethodInvoker(delegate
                {
                    listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Received: sensorId:" + sensor.Id + " timestamp:" + sensor.TimeStamp + " temperature:" + sensor.Temperature + " humidity:" + sensor.Humidity + " Battery:" + sensor.Battery); }));
                }));
                return;
            }

            alarms.Add(sensor);


            listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Alerta Recebido:"); }));
            listMessages.Invoke(new MethodInvoker(delegate { listMessages.Items.Add("Received: sensorId:" + sensor.Id + " timestamp:" + sensor.TimeStamp + " temperature:" + sensor.Temperature + " humidity:" + sensor.Humidity + " Battery:" + sensor.Battery + " Reason:" + sensor.Reason); }));

        }

        private void preencherComboBox(string id, string floor, string location)
        {
            comboBoxSensorId.Invoke(new MethodInvoker(delegate
            {
                if (comboBoxSensorId.SelectedItem != null)
                {
                    if (comboBoxSensorId.SelectedItem.ToString() == "<---Empty--->")
                    {
                        comboBoxSensorId.Items.Remove("<---Empty--->");
                    }
                    if (!comboBoxSensorId.Items.Contains(id))
                    {
                        comboBoxSensorId.Items.Add(id);
                        comboBoxSensorId.SelectedIndex = 0;
                    }
                }
            }));


            Place place = places.Where(i => i.Local == location).FirstOrDefault();
            if (place == null)
            {
                places.Add(new Place(location));
            }
            place = places.Where(i => i.Local == location).FirstOrDefault();
            place.addFloor(floor);

            comboBoxLocation.Invoke(new MethodInvoker(delegate
            {
                foreach (var item in sensors)
                {

                    if (comboBoxLocation.SelectedItem != null)
                    {
                        if (comboBoxLocation.SelectedItem.ToString() == "<---Empty--->")
                        {
                            comboBoxLocation.Items.Remove("<---Empty--->");
                        }
                        if (!comboBoxLocation.Items.Contains(item.Location))
                        {
                            comboBoxLocation.Items.Add(item.Location);
                            comboBoxLocation.SelectedIndex = 0;
                        }
                    }
                }

                fillFloors();
            }));


        }

        private void fillFloors()
        {

            Place place = null;
            comboBoxLocation.Invoke(new MethodInvoker(delegate
            {
                place = places.Where(i => i.Local == comboBoxLocation.SelectedItem.ToString()).FirstOrDefault();
            }));


            comboBoxFloor.Invoke(new MethodInvoker(delegate
            {

                if (place == null)
                {
                    if (comboBoxFloor.SelectedItem == null || !(comboBoxFloor.SelectedItem.ToString() == "<---Empty--->"))
                    {
                        comboBoxFloor.Items.Clear();
                        comboBoxFloor.Items.Add("<---Empty--->");
                    }
                    comboBoxFloor.SelectedIndex = 0;
                    return;
                }



                List<String> floors = place.floors;


                foreach (var item in floors)
                {

                    if (comboBoxFloor.SelectedItem != null)
                    {
                        if (comboBoxFloor.SelectedItem.ToString() == "<---Empty--->")
                        {
                            comboBoxFloor.Items.Remove("<---Empty--->");
                        }
                        if (!comboBoxFloor.Items.Contains(item))
                        {

                            comboBoxFloor.Items.Add(item);
                            comboBoxFloor.SelectedIndex = 0;
                        }
                    }

                }

            }));

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







        private void comboBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!comboBoxType.Items.Contains("Temperature/Humidity"))
            {
                comboBoxType.Items.Add("Temperature/Humidity");
            }



            if (comboBoxSearch.SelectedItem.ToString() == "ID")
            {
                comboBoxFloor.Visible = false;
                comboBoxLocation.Visible = false;
                labelLocation.Visible = false;
                labelFloor.Visible = false;
                labelID.Visible = true;
                comboBoxSensorId.Visible = true;

            }
            else
            {
                comboBoxFloor.Visible = true;
                comboBoxLocation.Visible = true;
                labelLocation.Visible = true;
                //labelFloor.Visible = true;
                labelID.Visible = false;
                comboBoxSensorId.Visible = false;
                labelFloor.Visible = true;

            }
            draw();
            fillFloors();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            m_cClient.Subscribe(m_strTopicsInfo, qosLevels);

            if (m_cClient.IsConnected)
                Console.WriteLine("connected");
            else
                Console.WriteLine("Disconnected");


            m_cClient.Publish("BridgeBrokerIS2019/20", Encoding.UTF8.GetBytes("<reading><id>1</id><temperature>17</temperature><humidity>21</humidity><battery>99</battery><timestamp>11/30/2019 11:40:07 AM</timestamp></reading>"));


            m_cClient.Publish("BridgeBrokerIS2019/20", Encoding.UTF8.GetBytes("<reading><id>1</id><temperature>40</temperature><humidity>21</humidity><battery>99</battery><timestamp>11/30/2019 11:40:07 AM</timestamp></reading>"));


        }

        private void draw()
        {
            cartesianChart.Invoke(new MethodInvoker(delegate
            {
                cartesianChart.AxisX.Clear();

                #region Sensors
                if (comboBoxSensorId.SelectedItem != null && comboBoxType.SelectedItem != null && comboBoxSensorId.Visible)
                {


                    if (comboBoxType.SelectedItem.ToString() == "Temperature/Humidity")
                    {
                        drawTempHum();
                        return;
                    }

                    if (comboBoxSensorId.SelectedItem.ToString() != "All")
                    {
                        drawSingleSensor();
                        return;
                    }

                    if (comboBoxSensorId.SelectedItem.ToString() == "All")
                    {

                        drawAllSensor();
                        return;
                    }

                }
                #endregion
                #region Floors
                if (!comboBoxSensorId.Visible && comboBoxFloor.SelectedItem != null)
                {

                    if (comboBoxFloor.SelectedItem.ToString() == "All")
                    {
                        drawAllFloor();
                        return;
                    }

                    if (comboBoxType.SelectedItem.ToString() == "Temperature/Humidity")
                    {
                        drawTempHumFloor();
                        return;
                    }


                    drawSingleFloor();


                }
                #endregion
            }));






        }

        private void drawTempHumFloor()
        {


            cartesianChart.Series = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Humidity",
                    Values = new ChartValues<double>(getValuesHumFloor()),
                },
                   new LineSeries
                {
                    Title = "Temperature",
                    Values = new ChartValues<double>(getValuesTempFloor()),
                },

            };

            cartesianChart.AxisX.Add(new Axis
            {


                Labels = getDateTimesSensor(),

            });
            cartesianChart.LegendLocation = LegendLocation.Right;


        }

        private void drawSingleFloor()
        {
            cartesianChart.Series = new SeriesCollection()
                {
                new LineSeries
                {
                    Title = "Floor"+comboBoxFloor.Text.ToString(),
                    Values = new ChartValues<double>(getFloorSingle(comboBoxFloor.SelectedItem.ToString())),
                },
                };
            cartesianChart.AxisX.Add(new Axis
            {

                Labels = getDateTimesSensor(),
            }); ;
            cartesianChart.LegendLocation = LegendLocation.Right;
        }

        private void drawAllFloor()
        {

            List<String> onlyFloors = new List<string>();
            foreach (var item in sensors)
            {
                if (!onlyFloors.Contains(item.Floor))
                {
                    onlyFloors.Add(item.Floor);
                }
            }
            cartesianChart.Series = new SeriesCollection();
            foreach (var item in onlyFloors)
            {
                LineSeries line = new LineSeries
                {
                    Title = "Floor:" + item,
                    Values = new ChartValues<double>(getFloorSingle(item)),
                };
                cartesianChart.Series.Add(line);
            }
            cartesianChart.AxisX.Add(new Axis
            {

                Labels = getDateTimesSensor(),
            }); ;
            cartesianChart.LegendLocation = LegendLocation.Right;


        }


        private List<double> getFloorSingle(string floor)
        {
            List<double> values = new List<double>();
            string type = comboBoxType.SelectedItem == null ? "Temperature" : comboBoxType.SelectedItem.ToString(); ;

            foreach (var item in sensors)
            {
                if (item.Floor == floor && item.Location == comboBoxLocation.SelectedItem.ToString())
                {
                    if ("Temperature" == type)
                    {
                        if (item.Temperature == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {
                            values.Add(Double.Parse(item.Temperature));
                        }
                    }

                    if ("Humidity" == type)
                    {
                        if (item.Humidity == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {
                            values.Add(Double.Parse(item.Humidity));
                        }
                    }
                }
            }
            return values;
        }

        private void drawAllSensor()
        {

            List<String> onlyIds = new List<string>();
            foreach (var item in sensors)
            {
                if (!onlyIds.Contains(item.Id))
                {
                    onlyIds.Add(item.Id);
                }
            }
            cartesianChart.Series = new SeriesCollection();
            foreach (var item in onlyIds)
            {
                LineSeries line = new LineSeries
                {
                    Title = "Sensor:" + item,
                    Values = new ChartValues<double>(getValuesSingle(item)),
                };
                cartesianChart.Series.Add(line);
            }

            cartesianChart.AxisX.Add(new Axis
            {

                Labels = getDateTimesSensor(),
            }); ;
            cartesianChart.LegendLocation = LegendLocation.Right;
        }

        private IEnumerable<double> getValuesSingle(String id)
        {
            List<double> values = new List<double>();
            string type = comboBoxType.SelectedItem == null ? "1" : comboBoxType.SelectedItem.ToString(); ;
            foreach (var item in sensors)
            {
                if (item.Id == id)
                {
                    if ("Temperature" == type)
                    {
                        if (item.Temperature == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {
                            values.Add(Double.Parse(item.Temperature));
                        }
                    }

                    if ("Humidity" == type)
                    {
                        if (item.Humidity == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {
                            values.Add(Double.Parse(item.Humidity));
                        }
                    }


                }
            }
            return values;
        }

        private void drawSingleSensor()
        {

            cartesianChart.Series = new SeriesCollection()
                 {
                 new LineSeries
                 {
                     Title = "Sensor"+comboBoxSensorId.Text.ToString(),
                     Values = new ChartValues<double>(getValues()),
                 },
                 };

            cartesianChart.AxisX.Add(new Axis
            {
                Labels = getDateTimesSensor(),
            }); ;
            cartesianChart.LegendLocation = LegendLocation.Right;






        }

        private void drawTempHum()
        {


            cartesianChart.Series = new SeriesCollection()
            {
                new LineSeries
                {

                    Title = "Humidity",
                    Values = new ChartValues<double>(getValuesHum()),




                },
                   new LineSeries
                {

                    Title = "Temperature",
                    Values = new ChartValues<double>(getValuesTemp()),




                },



            };

            cartesianChart.AxisX.Add(new Axis
            {


                Labels = getDateTimesSensor(),

            });
            cartesianChart.LegendLocation = LegendLocation.Right;







        }

        private IEnumerable<double> getValuesHum()
        {
            List<double> values = new List<double>();
            string id = comboBoxSensorId.SelectedItem == null ? "1" : comboBoxSensorId.SelectedItem.ToString();
            foreach (var item in sensors)
            {

                if (item.Id == id)
                {
                    if (item.Humidity == "")
                    {
                        values.Add(double.NaN);
                    }
                    else
                    {
                        values.Add(Double.Parse(item.Humidity));
                    }
                }
            }
            return values;
        }


        private IEnumerable<double> getValuesHumFloor()
        {
            List<double> values = new List<double>();
            string floor = comboBoxFloor.SelectedItem == null ? "1" : comboBoxFloor.SelectedItem.ToString();
            foreach (var item in sensors)
            {

                if (item.Floor == floor && item.Location == comboBoxLocation.SelectedItem.ToString())
                {
                    if (item.Humidity == "")
                    {
                        values.Add(double.NaN);
                    }
                    else
                    {
                        values.Add(Double.Parse(item.Humidity));
                    }
                }
            }
            return values;
        }

        private IEnumerable<double> getValuesTempFloor()
        {
            List<double> values = new List<double>();
            string floor = comboBoxFloor.SelectedItem == null ? "1" : comboBoxFloor.SelectedItem.ToString();
            foreach (var item in sensors)
            {
                if (item.Floor == floor && item.Location == comboBoxLocation.SelectedItem.ToString())
                {
                    if (item.Temperature == "")
                    {
                        values.Add(double.NaN);
                    }
                    else
                    {
                        values.Add(Double.Parse(item.Temperature));
                    }
                }
            }
            return values;
        }

        private IEnumerable<double> getValuesTemp()
        {
            List<double> values = new List<double>();
            string id = comboBoxSensorId.SelectedItem == null ? "1" : comboBoxSensorId.SelectedItem.ToString();
            foreach (var item in sensors)
            {
                if (item.Id == id)
                {
                    if (item.Temperature == "")
                    {
                        values.Add(double.NaN);
                    }
                    else
                    {
                        values.Add(Double.Parse(item.Temperature));
                    }
                }
            }
            return values;
        }

        private List<double> getValues()
        {

            List<double> values = new List<double>();

            string id = comboBoxSensorId.SelectedItem == null ? "1" : comboBoxSensorId.SelectedItem.ToString();

            string type = comboBoxType.SelectedItem == null ? "1" : comboBoxType.SelectedItem.ToString(); ;

            foreach (var item in sensors)
            {

                if (item.Id == id)
                {


                    if ("Temperature" == type)
                    {
                        if (item.Temperature == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {

                            values.Add(Double.Parse(item.Temperature));
                        }
                    }

                    if ("Humidity" == type)
                    {
                        if (item.Humidity == "")
                        {
                            values.Add(double.NaN);
                        }
                        else
                        {
                            values.Add(Double.Parse(item.Humidity));
                        }
                    }

                }


            }

            return values;
        }



        private IList<string> getDateTimesSensor()
        {

            List<string> timestamps = new List<string>();
            string id = comboBoxSensorId.SelectedItem == null ? "1" : comboBoxSensorId.SelectedItem.ToString();


            if (comboBoxFloor.Visible)
            {
                //ter as datas todoas
                foreach (var item in sensors)
                {
                    if (item.Floor == item.Floor)
                    {
                        timestamps.Add(item.TimeStamp.ToString());
                    }
                }
                return timestamps;
            }




            foreach (var item in sensors)
            {
                if (item.Id == id)
                {
                    timestamps.Add(item.TimeStamp.ToString());
                }
            }
            return timestamps;
        }



        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void comboBoxFloor_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw();

            populateDataGrid();
        }

        private void comboBoxSensorId_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw();
            /*
            if (comboBoxSensorId.SelectedItem.ToString() == "All" && comboBoxType.Items.Contains("Temperature/Humidity"))
            {
                int last = comboBoxType.Items.Count - 1;
                comboBoxType.Items.RemoveAt(last);
              return;
            }
            if (!comboBoxType.Items.Contains("Temperature/Humidity"))
            {
                comboBoxType.Items.Add("Temperature/Humidity");
            }
            */
            populateDataGrid();


        }

        private void populateDataGrid()
        {
            dataGridView.Invoke(new MethodInvoker(delegate
            {
                dataGridView.Rows.Clear();
                foreach (var sensor in sensors)
                {
                    if (comboBoxSensorId.Visible && comboBoxSensorId.SelectedItem != null)
                    {
                        if (sensor.Id == comboBoxSensorId.SelectedItem.ToString())
                        {

                            dataGridView.Rows.Add(sensor.Id, sensor.TimeStamp, sensor.Temperature, sensor.Humidity, sensor.Floor, sensor.Location, sensor.Reason);

                        }
                    }
                    else
                    {
                        if (comboBoxFloor.SelectedItem != null && sensor.Floor == comboBoxFloor.SelectedItem.ToString() && sensor.Location == comboBoxLocation.SelectedItem.ToString())
                        {
                            dataGridView.Invoke(new MethodInvoker(delegate
                            {
                                dataGridView.Rows.Add(sensor.Id, sensor.TimeStamp, sensor.Temperature, sensor.Humidity, sensor.Floor, sensor.Location, sensor.Reason);
                            }));
                        }
                    }

                }

            }));

        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void comboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            draw();
            populateDataGrid();
            fillFloors();

        }

        private void listMessages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonUpdateDisable_Click(object sender, EventArgs e)
        {
            buttonUpdateEnabled.Enabled = true;
            buttonUpdateDisable.Enabled = false;
        }

        private void buttonUpdateEnabled_Click(object sender, EventArgs e)
        {
            buttonUpdateEnabled.Enabled = false;
            buttonUpdateDisable.Enabled = true;
            draw();
        }

        private void buttonAlarmsEnabled_Click(object sender, EventArgs e)
        {
            buttonAlarmsEnabled.Enabled = false;
            buttonAlarmsDisable.Enabled = true;


            swap();
            draw();

            dataGridView.Columns.Add("Reason", "Reason");
            dataGridView.ReadOnly = true;
            populateDataGrid();
        }

        private void swap()
        {
            List<Sensor> aux = new List<Sensor>();
            foreach (var item in sensors)
            {
                aux.Add(item);
            }
            sensors.Clear();
            foreach (var item in alarms)
            {
                sensors.Add(item);
            }
            alarms.Clear();
            foreach (var item in aux)
            {
                alarms.Add(item);
            }
        }

        private void buttonAlarmsDisable_Click(object sender, EventArgs e)
        {
            buttonAlarmsEnabled.Enabled = true;
            buttonAlarmsDisable.Enabled = false;


            swap();
            draw();


            dataGridView.Columns.Remove("Reason");
            populateDataGrid();


        }

        private void labelFloor_Click(object sender, EventArgs e)
        {

        }

        private void labelID_Click(object sender, EventArgs e)
        {

        }

        private void labelLocation_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_cClient != null && m_cClient.IsConnected)
            {

                m_cClient.Unsubscribe(m_strTopicsInfo);
                m_cClient.Disconnect(); //Free process and process's resources


            }

        }
    }
}

public class DateModel
{
    public System.DateTime DateTime { get; set; }
    public double Value { get; set; }
}

