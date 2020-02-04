using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class TempPlugin : IPlugin
    {
        private Access database;
        public TempPlugin()
        {
            database = new Access();
            database.getTemperature();
            ThreadPool.QueueUserWorkItem(Sensor);
        }
        private void Sensor(object obj)
        {
            Random random = new Random();
            while (true)
            {
                if (database != null)
                {
                    database.addTemp(new Temperature(DateTime.Now, random.NextDouble() * (100.00 - (-20.00)) + (-20.00)));
                }
                Thread.Sleep(10000);
            }
        }
        public float CanHandle(IRequest req)
        {
            //if (req == null || req.Url == null || req.Url.Segments.Length < 1)
            //{
            //    return 0.0f;
            //}
            //else if (req.Url.Segments[0] == "temp")
            //{
            //    return 0.9f;
            //}
            //else
            //return 0.5f;
            //return 0f;
  

            float check = 0f;

            bool segmentCheck = Array.Exists(req.Url.Segments, element => element.ToLower() == "temp");
            if (segmentCheck) check += 0.2f;

            if (req.Url.ParameterCount > 0)
            {
                bool parameterCheck = req.Url.Parameter.ContainsKey("temp_plugin");
                if (parameterCheck && req.Url.Parameter["temp_plugin"] == "true") check += 0.5f;
            }


            return check;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse res = new Response();
            res.StatusCode = 400;

            string content;

            string from;
            string until;

            if (req.Url.Parameter.ContainsKey("from") && req.Url.Parameter.ContainsKey("until"))
            {
                from = req.Url.Parameter["from"];
                until = req.Url.Parameter["until"];
            }
            else
            {
                return res;
            }

            res.StatusCode = 200;
            //if (req.Url.Parameter.ContainsKey("type"))
            //{
                res.ContentType = "text/xml";
                content = createXML(database.getTemperature(DateTime.Parse(from), DateTime.Parse(until)));
            //}         
            //else
            //{
            //    res.ContentType = "text/json";
            //    content = returnJSON(database.getTemperature());
            //}
                

            res.SetContent(content);
            return res;
        }
        /// <summary>
        ///  Creates and returns html string with a table containing temperature data
        /// </summary>
        public string returnJSON(string[] data)
        {
            //StringBuilder content = new StringBuilder();


            // StringBuilder content = new StringBuilder();
            //content.Append("[");
            //for (int i = 0; i < data.Length; i++)
            //{
            //    string[] line = data[i].Split(';');
            //    content.Append("{");
            //    content.Append("'id':").Append(line[0]).Append(",");
            //    content.Append("'date':'").Append(line[1]).Append("',");
            //    content.Append("'celsius':").Append(line[2]).Append(",");
            //    content.Append("'fahrenheit':").Append(line[3]).Append("");
            //    content.Append("},");
            //}
            //content.Append("]");
            //return content.ToString();
            return null;
        }
        /// <summary>
        ///  Creates and returns xml string with a table containing temperature data
        /// </summary>
        public string createXML(List<Temperature> data)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = (XmlElement)document.AppendChild(document.CreateElement("Temperatures"));

            foreach (Temperature current in data)
            {
                // Entry root element
                XmlElement entry = (XmlElement)root.AppendChild(document.CreateElement("Entry"));
                entry.SetAttribute("id", current.ID.ToString());

                // Date element containing the date in yyyy-MM-dd hh:mm:ss format
                XmlElement date = (XmlElement)entry.AppendChild(document.CreateElement("Date"));
                date.AppendChild(document.CreateTextNode(current.Date.ToString("yyyy-MM-dd hh:mm:ss")));

                // Temperature element in Kelvin
                XmlElement kelvin = (XmlElement)entry.AppendChild(document.CreateElement("Kelvin"));
                kelvin.AppendChild(document.CreateTextNode(current.Kelvin.ToString().Replace(',', '.')));

                // Temperature element in Celsius
                XmlElement celsius = (XmlElement)entry.AppendChild(document.CreateElement("Celsius"));
                celsius.AppendChild(document.CreateTextNode(current.Celsius.ToString().Replace(',', '.')));

                // Temperature element in Fahrenheit
                XmlElement fahrenheit = (XmlElement)entry.AppendChild(document.CreateElement("Fahrenheit"));
                fahrenheit.AppendChild(document.CreateTextNode(current.Fahrenheit.ToString().Replace(',', '.')));
            }

            return document.OuterXml;
        }

        /// <summary>
        /// Returns string array with the temperature data read from file
        /// </summary>
        //public string[] getValues(string from, string until)
        //{
        //    string path = @"C:\Users\islam\Documents\Arbeit\FH\SWE\SWE1-CS\MyWebServer\Data\temps.txt";
        //    string readText = File.ReadAllText(path);
        //    string[] vals = readText.Split('\n');

        //    if(!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(until))
        //    {
        //        List<string> newVals = new List<string>();
        //        foreach (var item in vals)
        //        {
        //            string[] line = item.Split(';');

        //            DateTime date = DateTime.Parse(line[1]);
        //            DateTime f = DateTime.Parse(from);
        //            DateTime u = DateTime.Parse(until);

        //            if (date.Ticks > f.Ticks && date.Ticks < u.Ticks)
        //            {
        //                 newVals.Add(item);
        //            }
        //            vals = newVals.ToArray();
        //        }
        //    }
        //    return vals;
        //}
        /// <summary>
        /// Creates Random Values for temperature data and saves them to a file
        /// </summary>
        //public void createRandomValues()
        //{
        //    Random rnd = new Random();
        //    List<string> vals = new List<string>();
        //    string path = @"C:\Users\islam\Documents\Arbeit\FH\SWE\SWE1-CS\MyWebServer\Data\temps.txt";
        //    string createText = "";
        //    for (int i = 0; i < 10000; i++)
        //    {
                
        //        int id = rnd.Next(1, 1000);  
        //        int celsius = rnd.Next(1, 40);   
        //        int fahrenheit = (celsius * 9) / 5 + 32;
        //        string date = RandomDay(rnd).ToString();

                
        //        createText += $"{id};{date};{celsius};{fahrenheit};\n";
        //        //vals.Add(createText);
        //    }

        //    File.WriteAllText(path, createText);
            
        //}
        /// <summary>
        /// Returns random generated Date
        /// </summary>
        //DateTime RandomDay(Random gen)
        //{    
        //    DateTime start = new DateTime(2010, 1, 1);
        //    int range = (DateTime.Today - start).Days;
        //    return start.AddDays(gen.Next(range));
        //}
    }
}
