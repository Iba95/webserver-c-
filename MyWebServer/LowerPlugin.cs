using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class LowerPlugin : IPlugin
    {
        public float CanHandle(IRequest req)
        {
            float check = 0f;

            bool segmentCheck = Array.Exists(req.Url.Segments, element => element.ToLower() == "lower");
            if (segmentCheck) check += 0.2f;

            if (req.ContentString != null && req.ContentString.StartsWith("text="))
            {
                check += 0.2f;
            }

            if (req.Url.ParameterCount > 0)
            {
                bool parameterCheck = req.Url.Parameter.ContainsKey("lower_plugin");
                if (parameterCheck && req.Url.Parameter["lower_plugin"]=="true")check +=0.5f;
            }
            

            return check;
        }

        public IResponse Handle(IRequest req)
        {
            string str = req.ContentString;
            //remove "text="
            str = str.Remove(0, str.IndexOf('=') + 1);

            Response res = new Response();
            res.StatusCode = 200;
            res.AddHeader("Content-Type", "text/plain; charset=UTF-8");
            res.AddHeader("connection", "close");

            if (string.IsNullOrEmpty(str.Trim()))
            {
                res.SetContent("Bitte geben Sie einen Text ein");
                return res;
            }

            res.SetContent(str.ToLower());
            return res;
        }
    }
}
