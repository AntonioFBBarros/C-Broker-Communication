using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace Bridge
{
    public class BrokerManager
    {
        Boolean logging = false;
        MqttClient mqttClient;

        public string domain { get; set; }

        string[] channels = { "BridgeBrokerIS2019/20" };

        public BrokerManager()
        {
            try
            {
                using (var formBroker = new FormBroker())
                {
                    formBroker.ShowDialog();
                    if (formBroker.domain == null)
                    {
                        return;
                    }
                    logging = formBroker.logging;
                    domain = formBroker.domain;
                }

                mqttClient = new MqttClient(domain);
                mqttClient.Connect(Guid.NewGuid().ToString());
                if (!mqttClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...");
                    return;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.ToString());
                domain = null;
                return;
            }
        }

        public void disconnectBroker()
        {
            if (mqttClient.IsConnected)
            {
                mqttClient.Unsubscribe(channels); //Put this in a button to see notif!
                mqttClient.Disconnect(); //Free process and process's resources
            }
        }

        public void SendContent(string content)
        {

            foreach (string channel in channels)
            {
                mqttClient.Publish(channel, Encoding.UTF8.GetBytes(content));
                if (logging)
                {
                    FileManager.writeLineToFile("log.txt", channel + ": " + DateTime.Now.ToString() + "\n");
                    FileManager.writeLineToFile("log.txt", content + "\n");
                }
            }
        }

    }
}
