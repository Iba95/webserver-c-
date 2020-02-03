using BIF.SWE1.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer
{
    class Response: IResponse
    {
        private IDictionary<string, string> _headers = new Dictionary<string, string>();
        private int _contentLength;
        private string _contentType;
        private byte[] _content;
        private int _statusCode;
        private string _status;
        private string _serverHeader;
        /// <summary>
        /// Initiates the Response class without a given Parameter, and sets the value for the serverHeader
        /// </summary>
        public Response()
        {
            _serverHeader = "BIF-SWE1-Server";
            
        }
        /// <summary>
        /// Returns a writable dictionary of the response headers. Never returns null.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }
        /// <summary>
        /// Returns the content length or 0 if no content is set yet.
        /// </summary>
        public int ContentLength
        {
            get { return _content.Length;}
        }
        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }
        /// <summary>
        /// Gets or sets the current status code. An Exceptions is thrown, if no status code was set.
        /// </summary>
        public int StatusCode
        {
            get {
                if (_status == null)
                {
                    throw new Exception("No StatusCode");
                }

                return _statusCode;
            }
            set
            {
                if(value == 200)
                {
                    _status = "200 OK";
                }
                else if (value == 404)
                {
                    _status = "404 Not Found";
                }
                else
                {
                    _status = "500 INTERNAL SERVER ERROR";
                }
 
                _statusCode = value; 
            }
        }
        /// <summary>
        /// Returns the status code as string. (200 OK)
        /// </summary>
        public string Status
        {
            get { return _status; }
        }
        /// <summary>
        /// Gets or sets the Server response header. Defaults to "BIF-SWE1-Server".
        /// </summary>
        public string ServerHeader
        {
            get { return _serverHeader; }
            set { _serverHeader = value; }
        }
        /// <summary>
        /// Adds or replaces a response header in the headers dictionary.
        /// </summary>
        public void AddHeader(string header, string value)
        {
            _headers[header] = value;
        }
        /// <summary>
        /// Sets a string content. The content will be encoded in UTF-8.
        /// </summary>
        public void SetContent(string content)
        {
            SetContent(Encoding.UTF8.GetBytes(content));

        }
        /// <summary>
        /// Sets a byte[] as content.
        /// </summary>
        public void SetContent(byte[] content)
        {
            _content = content;
            _contentLength = _content.Length;
        }
        /// <summary>
        /// Sets the stream as content.
        /// </summary>
        public void SetContent(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);

            SetContent(Encoding.UTF8.GetBytes(sr.ReadLine()));
        }
        /// <summary>
        /// Sends the response to the network stream.
        /// </summary>
        public void Send(Stream network)
        {
            if(!String.IsNullOrEmpty(_contentType) && _contentLength == 0)
            {
                throw new Exception("No Content");
            }

            StreamWriter sw = new StreamWriter(network);

            AddHeader("Server", _serverHeader);

            sw.WriteLine("HTTP/1.1 {0}", _status);
            sw.WriteLine("Server: {0}", _serverHeader);

            foreach (var key in _headers.Keys)
            {
                sw.WriteLine($"{key}: {_headers[key]}");
            }
  
            sw.WriteLine();
            sw.Flush();


            if (_content != null)
            {
                try
                {
                    BinaryWriter bw = new BinaryWriter(network);
                    bw.Write(_content);
                    bw.Flush();
                }
                catch (Exception)
                {

                    throw new Exception();
                }
                
            }

        }
    }
}
