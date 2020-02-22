using BIF.SWE1.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    [TestFixture]
    public class Unittests
    {
        [Test]
        public void tempEnoughData()
        {
            Access database = new Access();
            Assert.That(database.getTemperature().Count(), Is.GreaterThan(10000));
        }
        [Test]
        public void tempCelsiusToKelvin()
        {
            Temperature temp = new Temperature(DateTime.Now, 15);
            Assert.That(temp.Kelvin, Is.EqualTo(288.15));
        }

        [Test]
        public void tempCelsiusToFahrenheit()
        {
            Temperature temp = new Temperature(DateTime.Now, 15);
            Assert.That(temp.Fahrenheit, Is.EqualTo(59));
        }
        [Test]
        public void tempOldDataExist()
        {
            Access database = new Access();
            var from = "2010-01-01";
            var until = "2011-01-01";
            var temps = database.getTemperature(DateTime.Parse(from), DateTime.Parse(until));
            Assert.That(temps.Count(), Is.GreaterThan(50));
        }
        [Test]
        public void lowerSpecialChars()
        {
            string specialchars = "ABc +#,.-`+";

            var req = new Request(GetValidRequestStream("/lower", method: "POST", body: string.Format("text={0}", specialchars)));
            var lower = new LowerPlugin();
            var res = lower.Handle(req);
            Assert.That(res.StatusCode, Is.EqualTo(200));
            Assert.That(GetBody(res).ToString(), Does.Contain(specialchars.ToLower()));
        }

        //[Test]
        //public void send404Template()
        //{
        //    var req = new Request(GetValidRequestStream("/abcdefgh.xy", method: "GET"));
        //    var sfp = new StaticFilePlugin();
        //    var res = sfp.Handle(req);
        //    Assert.That(res.StatusCode, Is.EqualTo(404));
        //    Assert.That(GetBody(res).ToString(), Does.Contain("Error 404"));
        //}

        public static Stream GetValidRequestStream(string url, string method = "GET", string host = "localhost", string[][] header = null, string body = null)
        {
            byte[] bodyBytes = null;
            if (body != null)
            {
                bodyBytes = Encoding.UTF8.GetBytes(body);
            }

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms, Encoding.ASCII);

            sw.WriteLine("{0} {1} HTTP/1.1", method, url);
            sw.WriteLine("Host: {0}", host);
            sw.WriteLine("Connection: keep-alive");
            sw.WriteLine("Accept: text/html,application/xhtml+xml");
            sw.WriteLine("User-Agent: Unit-Test-Agent/1.0 (The OS)");
            sw.WriteLine("Accept-Encoding: gzip,deflate,sdch");
            sw.WriteLine("Accept-Language: de-AT,de;q=0.8,en-US;q=0.6,en;q=0.4");
            if (bodyBytes != null)
            {
                sw.WriteLine(string.Format("Content-Length: {0}", bodyBytes.Length));
                sw.WriteLine("Content-Type: application/x-www-form-urlencoded");
            }
            if (header != null)
            {
                foreach (var h in header)
                {
                    sw.WriteLine(string.Format("{0}: {1}", h[0], h[1]));
                }
            }
            sw.WriteLine();

            if (bodyBytes != null)
            {
                sw.Flush();
                ms.Write(bodyBytes, 0, bodyBytes.Length);
            }

            sw.Flush();

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private static StringBuilder GetBody(IResponse resp)
        {
            StringBuilder body = new StringBuilder();
            using (var ms = new MemoryStream())
            {
                resp.Send(ms);
                ms.Seek(0, SeekOrigin.Begin);
                var sr = new StreamReader(ms);
                while (!sr.EndOfStream)
                {
                    body.AppendLine(sr.ReadLine());
                }
            }
            return body;
        }
    }
}
