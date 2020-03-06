using ProjectXML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace projIs_Alerts
{
    public partial class Form1 : Form
    {
        //MqttClient mqttClient = new MqttClient("test.mosquitto.org");
        MqttClient mqttClient = new MqttClient("test.mosquitto.org");
        string[] choose = { "<", ">", "=","between" };
        string[] valores = { "Temperatura", "Humidade"};
        string[] topics = { "BridgeBrokerIS2019/20" };
        byte[] qosLevels =  
            {
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
            };
        string alert=null;
        string path = "alertas.txt";
        string temp_path = "alertas_temp.txt";
        string xsdPath = "prjIS.xsd";
        string xmlPath = "prjIS.xml";

        Form3 frm = null;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.alertChoose.DataSource = choose;
            this.comboBox1.DataSource = valores;

            this.alertChoose.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            mqttClient.Connect(Guid.NewGuid().ToString());
            if (!mqttClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker");
                return;
            }

            using (StreamReader r = new StreamReader(path))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                }
            }

            frm = new Form3();
            frm.Show();

            mqttClient.Subscribe(topics, qosLevels);
            mqttClient.MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;
        }

        private void recursividadeXml(string message,XmlNode node,XmlNode item,XmlDocument xml)
        {
            
                string name = item.Name;
                if (!name.Equals("id") && !name.Equals("battery") && !name.Equals("timestamp"))//caso o nome do elemento seja "id,"battery" ou "timestamp" eu nao quero
                //fazer nada com ele, apenas quero os elementos q contêm os valores dos sensores
                {
                    string value = item.InnerText;
                    switch (name)
                    {
                        case "temperature"://caso o elemento tenha o nome "temperature" ou seja, ele é o sensor com os valores da temperatura
                            if (message.Contains("Temperatura") && message.Contains("entre"))//aqui vejo se o requesito atual contem palvras chave, tais como a "Temperatura" e o "entre" para 
                            //poder fazer as verificaçoes de acordo
                            {
                                string valMaior = message.Substring(message.Length - 4, 3);//obtenho o maior valor do requesito "between" ou  "entre"
                                string valMenor = message.Substring(message.Length - 10, 3);//obtenho o menor valor do requesito "between" ou  "entre"

                            if (decimal.Parse(valMenor) > decimal.Parse(item.InnerText) || decimal.Parse(valMaior) < decimal.Parse(item.InnerText))//se o menor valor q o utiliador inseriu por ex "5" for maior que o valor da temperatura
                                //ou se o maior valor que o utilizador inseriu por ex "50" for menor que o valor da temperatura
                                //tem que se verificar assim pois nesta app defenisse requesitos, nao alertas em si, logo nós queremos verificar se a temperatura recebida do sensor está dentro daquilo que nós queremos que seja a temperatura
                                //neste caso verificamos se a temperatura é maior ou menor que os valores extremos que o utilizador inseriu ou seja, se é menor que 5 ou maior que 50
                                {
                                    if (node["alarms"] == null)//este if serve para verificar se o elemento "alarm" já existe, ou seja se existem 2 requesitos que deram erros
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    { 
                                        if(node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                                
                                    }
                                }
                                
                                
                            }
                            else if (message.Contains("Temperatura") && message.Contains("maior"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                if (decimal.Parse(val) > decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            else if (message.Contains("Temperatura") && message.Contains("menor"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                if (decimal.Parse(val) < decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            else if (message.Contains("Temperatura") && message.Contains("igual"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                    if (decimal.Parse(val) != decimal.Parse(item.InnerText))
                                    {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            break;
                        case "humidity":
                            if (message.Contains("Humidade") && message.Contains("entre"))
                            {
                                string valMaior = message.Substring(message.Length - 4, 3);
                                string valMenor = message.Substring(message.Length - 10, 3);
                                if (decimal.Parse(valMenor) > decimal.Parse(item.InnerText) || decimal.Parse(valMaior) < decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            else if (message.Contains("Humidade") && message.Contains("maior"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                if (decimal.Parse(val) > decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            else if (message.Contains("Humidade") && message.Contains("menor"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                if (decimal.Parse(val) < decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            else if (message.Contains("Humidade") && message.Contains("igual"))
                            {
                                string val = message.Substring(message.Length - 3, 3);
                                if (decimal.Parse(val) != decimal.Parse(item.InnerText))
                                {
                                    if (node["alarms"] == null)
                                    {
                                        XmlElement alarm = xml.CreateElement("alarms");
                                        alarm.InnerText = message;
                                        node.AppendChild(alarm);
                                    }
                                    else
                                    {
                                        if (node["alarms"].InnerText != message)
                                        {
                                            node["alarms"].InnerText = node["alarms"].InnerText + "/ " + message;
                                        }
                                    }
                                }
                            }
                            break;
                    }
            }
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            String xmlString = Encoding.UTF8.GetString(e.Message);
            HandlerXML xmlHelper = new HandlerXML(xmlString, xsdPath);
            bool valid = xmlHelper.ValidateXML();
            if (!valid)
            {
                Console.WriteLine("Mensagem Invalida! " + xmlHelper.ValidationMessage);
                return;

            }
            else
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(Encoding.UTF8.GetString(e.Message));
                    XmlNode node = xml.SelectSingleNode("/reading");//obtem o root element do xml 
                    XmlNodeList elements = xml.SelectNodes("//*[count(*)=0]");//obtem todos os elementos sem filhos,
                    foreach (XmlNode item in elements)//os elementos sem filhos sao percorridos 
                    {
                        for(int i = 0; i < frm.listBox1.Items.Count; i++)//aqui percorro todas os requesitos que o utizador escolheu ter ativo
                        //eu chamo lhe requesitos pois nesta aplicaçao apesar de no final eu verificar e mandar alertas, nesta aplicaçao o utilizador define como quer que
                        //o valor dos sensores que existem sejam, ou seja, se o utilizador quiser uma alerta para quando a temperatura é maior que 5 entao o ele define
                        //um requesito que diz "quero que a temperatura seja menor que 5"
                        {
                            recursividadeXml(frm.listBox1.Items[i].ToString(), node, item, xml);//nesta funçao mando a mensagem do requesito atual, o node do xml, o atual item do xml, e o xml em si
                        }
                        richTextBox1.AppendText($"{item.Name}:{item.InnerText}\n");
                    }
                    
                    xml.Save(xmlPath);
                    if (xml.InnerXml.Contains("alarms"))
                    {
                        mqttClient.Publish("AlarmsChannel", Encoding.UTF8.GetBytes(xml.InnerXml));
                    }
                    richTextBox1.AppendText($"\n");
                    
                });
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void alertChoose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.alertChoose.SelectedItem.ToString() == "between")
            {
                this.label3.Visible = true;
                this.textBox2.Visible = true;
            }
            else
            {
                this.label3.Visible = false;
                this.textBox2.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string alert = null;
            if (this.alertChoose.SelectedItem.ToString() == "between")
            {
                if(this.textBox1.Text == "" || this.textBox2.Text=="" || this.comboBox1.SelectedItem==null)
                {
                    return;
                }
                decimal val1 = decimal.Parse(this.textBox1.Text);
                decimal val2 = decimal.Parse(this.textBox2.Text);
                if (val1 >= val2 || val1==100)
                {
                    return;
                }
                alert = "A " + comboBox1.SelectedItem.ToString() + " tem de estar entre  " + this.textBox1.Text + "  e  " + this.textBox2.Text + " ";
            }
            else
            {
                if (this.textBox1.Text == "" || this.comboBox1.SelectedItem == null)
                {
                    return;
                }
                switch (this.alertChoose.SelectedItem.ToString())
                {
                    case "<":
                        if (decimal.Parse(this.textBox1.Text) > 100)
                        {
                            return;
                        }
                        alert = "A "+comboBox1.SelectedItem.ToString() +"  tem de ser menor que  " + this.textBox1.Text;
                        break;
                    case ">":
                        if (decimal.Parse(this.textBox1.Text) > 100)
                        {
                            return;
                        }
                        alert = "A " + comboBox1.SelectedItem.ToString() + "  tem de ser maior que  " + this.textBox1.Text;
                        break;
                    case "=":
                        if (decimal.Parse(this.textBox1.Text) > 100)
                        {
                            return;
                        }
                        alert = "A " + comboBox1.SelectedItem.ToString() + "  tem de ser igual a  " + this.textBox1.Text;
                        break;
                }
            }
            listBox1.Items.Add(alert);
            
            if (!File.Exists(path))
            {
                MessageBox.Show("Select an alert first!");
                return; 
            }
            using (var fileSStream = File.Open(path, FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(fileSStream))
                {
                    writer.WriteLine(alert);
                    writer.Flush();
                    writer.Close();
                }
                fileSStream.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                if (!frm.listBox1.Items.Contains(this.listBox1.SelectedItem.ToString()))
                {
                    alert = this.listBox1.SelectedItem.ToString();
                    frm.listBox1.Items.Add(alert);
                    MessageBox.Show("Enabled!");
                }
            }
            else
            {
                MessageBox.Show("Select an alert first!");
                return;
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mqttClient.Unsubscribe(topics);
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
    

            using (var sr = new StreamReader(path))
            using (var sw = new StreamWriter(temp_path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line != this.listBox1.SelectedItem.ToString())
                        sw.WriteLine(line);
                }
            }
            if (this.listBox1.SelectedItem != null)
            {
                File.Delete(path);
                File.Move(temp_path, path);
            
                if (frm.listBox1.Items.Contains(this.listBox1.SelectedItem.ToString()))
                {
                    frm.listBox1.Items.Remove(this.listBox1.SelectedItem.ToString());
                }
                listBox1.Items.Remove(this.listBox1.SelectedItem.ToString());
                MessageBox.Show("Removed");
            }
        }
    }
}
