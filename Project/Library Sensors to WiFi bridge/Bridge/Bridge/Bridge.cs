using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Bridge
{
    public class Bridge
    {

        private int nParameters;
        private ListBox list;

        public long lastLine { get; set; }
        public string domain { get; set; }
        public BrokerManager brokerManager { get; set; }
        public Boolean READING { get; set; }

        XMLManager xmlManager = new XMLManager();
        FileManager fileManager = FileManager.getInstance();

        public Bridge()
        {
        }

        public void start(Object lst)
        {
            list = (ListBox)lst;
            nParameters = list.Items.Count;

            try
            {
                while (READING)
                {
                    using (var stream = File.OpenRead(fileManager.FilePath))
                    {
                        stream.Position = lastLine;
                        if (stream.Position != stream.Length)
                        {
                            lastLine = readLine(stream);
                        }
                    }
                }
                FileManager.writeTxtToFile("checkpoint.txt", lastLine.ToString());

                brokerManager.disconnectBroker();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }


        private long readLine(FileStream stream)
        {
            Byte[] vs;
            List<Tuple<string, string>> listData = new List<Tuple<string, string>>();

            try
            {
                using (var reader = new BinaryReader(stream))
                {
                    foreach (var item in list.Items)
                    {
                        string[] type = item.ToString().Split('/');
                        Int32 bytesToRead = Convert.ToInt32(type[2]);
                        vs = new Byte[bytesToRead];
                        vs = reader.ReadBytes(bytesToRead);

                        switch (type[1])
                        {
                            case "sbyte": //int8
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString((sbyte)vs[0])));
                                break;
                            case "byte": //uint8
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString((byte)vs[0])));
                                break;
                            case "int16":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToInt16(vs, 0))));
                                break;
                            case "uint16":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToUInt16(vs, 0))));
                                break;
                            case "int32":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToInt32(vs, 0))));
                                break;
                            case "uint32":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToUInt32(vs, 0))));
                                break;
                            case "int64":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToInt64(vs, 0))));
                                break;
                            case "uint64":
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToUInt64(vs, 0))));
                                break;
                            case "single": //float32
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToSingle(vs, 0))));
                                break;
                            case "double": //float64
                                listData.Add(new Tuple<string, string>(type[0], Convert.ToString(BitConverter.ToDouble(vs, 0))));
                                break;
                        }
                    }

                    Send(listData);
                    return reader.BaseStream.Position;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }

            return 0;
        }


        private void Send(List<Tuple<string, string>> listItems)
        {
            string content = xmlManager.MakeXML(listItems);
            brokerManager.SendContent(content);
        }
    }
}
