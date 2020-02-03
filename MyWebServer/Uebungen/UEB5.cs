using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;
using MyWebServer;

namespace Uebungen
{
    public class UEB5
    {
        public void HelloWorld()
        {
        }

        public IPluginManager GetPluginManager()
        {
            return new PluginManager();
        }

        public IRequest GetRequest(System.IO.Stream network)
        {
            return new Request(network);
        }

        public IPlugin GetStaticFilePlugin()
        {
            return new StaticFilePlugin();
        }

        public string GetStaticFileUrl(string fileName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "statics");
            return Path.Combine(path,fileName);
        }

        public void SetStatiFileFolder(string folder)
        {
            Environment.CurrentDirectory = folder;
        }
    }
}
