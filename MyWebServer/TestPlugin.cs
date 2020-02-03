using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    class TestPlugin: IPlugin
    {
        public float CanHandle(IRequest req)
        {
            float check = 0f;
             bool segmentCheck = Array.Exists(req.Url.Segments, element => element.ToLower() == "test");
            if (segmentCheck || req.Url.RawUrl == "/") check += 0.4f;

            if (req.Url.ParameterCount > 0)
            {
                bool parameterCheck = req.Url.Parameter.ContainsKey("test_plugin");
                if (parameterCheck && req.Url.Parameter["test_plugin"] == "true") check += 0.5f;
            }


            return check;
        }

        public IResponse Handle(IRequest req)
        {
            Response res = new Response();
            res.StatusCode = 200;
            res.ContentType = "text/html";

            res.SetContent("<html><body><h1>Webserver!</h1><p>Welcome</p></body></html>");
            return res;
        }
    }
}
