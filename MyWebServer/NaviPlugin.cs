using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class NaviPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            //if (req.Url.Segments[0] == "navi")
            //{
            //    if (req.Method.ToLower() == "get")
            //    {
            //        return 0.8f;
            //    }
            //    if (req.Method.ToLower() == "post")
            //    {
            //        return 0.9f;
            //    }
            //    return 0.5f;
            //}
            return 0.0f;
        }

        public IResponse Handle(IRequest req)
        {
            IResponse res = new Response();
            res.StatusCode = 200;
            string body = "";
            string street = "";

            if (req.ContentString.Contains("street="))
            {
                street = req.ContentString.Split('=')[1];
                
            }
            if (string.IsNullOrEmpty(street))
            {
                body += "Bitte geben Sie eine Anfrage ein";
            }
                
            else
            {
                body += "Orte gefunden für " + street;               
            }
            
            res.SetContent(body);
           
            return res;
        }
    }
}
