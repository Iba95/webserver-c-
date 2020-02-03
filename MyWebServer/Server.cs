using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyWebServer
{
    class Server
    {
        private TcpListener listener;
        private bool runs = false;

        public void Start()
        {
            runs = true;

            listener = new TcpListener(IPAddress.Loopback, 8080);
            listener.Start();

            while (runs)
            {
                //Waits until client tries to connect
                Socket socket = listener.AcceptSocket();

                //New thread for every request
                ThreadPool.QueueUserWorkItem(HandleHTTPRequest, socket);
            }
        }

        public void Stop()
        {
            runs = false;
            listener.Stop();
        }

        private void HandleHTTPRequest(object socket)
        {
            Socket s = (Socket)socket;

            using (NetworkStream ns = new NetworkStream(s))
            {
                IRequest req = new Request(ns);

                if (!req.IsValid)
                {
                    Response err = new Response();
                    err.StatusCode = 400;
                    err.Send(ns);
                    return;
                }

                float highest = 0.0f;
                IPlugin current = null;
                foreach (IPlugin plugin in PluginManager.Plugins)
                {
                    float hc = plugin.CanHandle(req);
                    if (hc > highest)
                    {
                        highest = hc;
                        current = plugin;
                    }
                }

                // Check if any plugin is able to handle the given request
                if (current == null)
                {
                    Response err = new Response();
                    err.StatusCode = 400;
                    err.Send(ns);
                    return;
                }

                // Send the plugin response
                IResponse res = current.Handle(req);
                if (res != null)
                {
                    res.Send(ns);
                }
            }
            s.Close();
        }


        public IPluginManager PluginManager { get; set; } = new PluginManager();    
    }
}
