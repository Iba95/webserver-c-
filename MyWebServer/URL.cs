using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    public class Url : IUrl
    {
        private string _path;
        private string _raw;
        private int _parameterCount;
        private IDictionary<string, string> _parameter = new Dictionary<string, string>();
        private string _fragment;
        private string[] _segments;
        /// <summary>
        /// Initiates the URL class without a given Parameter
        /// </summary>
        public Url()
        {
            _raw = "";
            _path = "";
            _parameterCount = 0;
        }
        /// <summary>
        /// Initiates the URL class with a url as Parameter and extracts important information e.g. url parameters
        /// </summary>
        public Url(string raw)
        {
            string[] splitted;
            string[] splittedparams;
            _parameterCount = 0;

            _raw = raw;
            if (_raw == null) { _path = null; }
            else
            {
                _path = raw;

                if (_raw.Contains('?'))
                {
                    splitted = _raw.Split('?');

                    _path = splitted[0];

                    splittedparams = splitted[1].Split('&');

                    _parameterCount = splittedparams.Length;

                    foreach (string element in splittedparams)
                    {
                        string[] values = element.Split('=');
                        _parameter.Add(values[0], values[1]);
                    }
                }
                if (_raw.Contains('#'))
                {
                    splitted = _raw.Split('#');
                    _path = splitted[0];
                    _fragment = splitted[1];
                }
                if (_raw.Contains('/'))
                {
                    _segments = _path.Split('/').Skip(1).ToArray();
                }
            }    
           
        }
        /// <summary>
        /// Returns a dictionary with the parameter of the url. Never returns null.
        /// </summary>
        public IDictionary<string, string> Parameter
        {
            get { return _parameter; }
        }
        /// <summary>
        /// Returns the number of parameter of the url. Returns 0 if there are no parameter.
        /// </summary>
        public int ParameterCount
        {
            get { return _parameterCount; }
        }
        /// <summary>
        /// Returns the path of the url, without parameter.
        /// </summary>
        public string Path
        {
            get { return _path; }
        }
        /// <summary>
        /// Returns the raw url.
        /// </summary>
        public string RawUrl
        {
            get { return _raw; }
        }
        /// <summary>
        /// Returns the extension of the url filename, including the leading dot. If the url contains no filename, a empty string is returned. Never returns null.
        /// </summary>
        public string Extension
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Returns the filename (with extension) of the url path. If the url contains no filename, a empty string is returned. Never returns null. A filename is present in the url, if the last segment contains a name with at least one dot.
        /// </summary>
        public string FileName
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Returns the url fragment. A fragment is the part after a '#' char at the end of the url. If the url contains no fragment, a empty string is returned. Never returns null.
        /// </summary>
        public string Fragment
        {
            get { return _fragment; }
        }
        /// <summary>
        /// Returns the segments of the url path. A segment is divided by '/' chars. Never returns null.
        /// </summary>
        public string[] Segments
        {
            get { return _segments; }
        }
    }
}
