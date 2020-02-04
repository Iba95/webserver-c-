using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class Program
    {

        static void Main(string[] args)
        {
            //Listen();         
            Server server = new Server();
            server.Start();

        }
  
        //public static void Listen()
        //{
        //    TcpListener listener = new TcpListener(IPAddress.Loopback, 8080);
        //    listener.Start();

        //    while (true)
        //    {
        //        // Waits until client tries to connect
        //        Socket socket = listener.AcceptSocket();
  
        //        //New Thread for every client
        //        ThreadPool.QueueUserWorkItem(newReq, socket);

        //    }
        //}

        //private static void newReq(object s)
        //{
        //    Socket socket = (Socket)s;
        //    using (NetworkStream ns = new NetworkStream(socket))
        //    {
        //        IRequest req = new Request(ns);

        //        if (!req.IsValid)
        //        {
        //            Response invalidRes = new Response();
        //            invalidRes.StatusCode = 400;
        //            invalidRes.Send(ns);
        //        }


        //        float highest = 0.0f;
        //        PluginManager pluginmanager = new PluginManager();
        //        IPlugin plugin = null;
        //        foreach (IPlugin p in pluginmanager.Plugins)
        //        {
        //            float hc = p.CanHandle(req);
        //            if (hc > highest)
        //            {
        //                highest = hc;
        //                plugin = p;
        //            }
        //        }

        //        IResponse res = plugin.Handle(req);
        //        res.Send(ns);

                
        //    }
        //    socket.Close();
        //}

    }
}
