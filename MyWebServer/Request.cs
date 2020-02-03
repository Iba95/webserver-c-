using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIF.SWE1.Interfaces;

namespace MyWebServer
{
    class Request: IRequest
    {
        private bool _isValid;
        private string _method;
        private IUrl _url;
        private IDictionary<string, string> _headers = new Dictionary<string, string>();
        private string _userAgent;
        private int _headerCount;
        //private int _contentLength;
        //private string _contentType;
        private string _contentString;
        private byte[] _contentBytes;
        private Stream _contentStream;
        /// <summary>
        /// Initiates the Request class with a stream as Parameter and extracts important information e.g. request method (get,post) 
        /// </summary>
        public Request(Stream stream)
        {    
            _userAgent = "Unit-Test-Agent/1.0 (The OS)";        

            StreamReader sr = new StreamReader(stream);

            _headers.Add("user-agent", _userAgent);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
                if (string.IsNullOrEmpty(line)) { break; }

                if (!line.Contains(':'))
                {
                    string[] firstLine = line.Split(' ');

                    if (firstLine.Length >= 1)
                        _method = firstLine[0].ToUpper();

                    if (firstLine.Length > 1)
                    {
                        _url = new Url(firstLine[1]);
                    }
                        
                }
                else
                {
                    string[] pair = line.Split(':');
                    _headers.Add(pair[0], pair[1].Trim());
                }

                _headerCount++;
            }

            if (ContentLength > 0)
            {
                char[] contentLine = new char[ContentLength];
                sr.Read(contentLine, 0, ContentLength);
                _contentString = new string(contentLine);
                _contentBytes = Encoding.UTF8.GetBytes(_contentString);
                _contentStream = new MemoryStream(_contentBytes);
            }

            _isValid = true;

            if (_method != "GET" && _method != "POST")
                _isValid = false;         

            if (_headerCount <= 0)
                _isValid = false;

            if (_url == null)
                _isValid = false;

        }
        /// <summary>
        /// Returns true if the request is valid. A request is valid, if method and url could be parsed. A header is not necessary.
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }
        /// <summary>
        /// Returns the request method in UPPERCASE. get -> GET.
        /// </summary>
        public string Method
        { 
            get { return _method; }
        }
        /// <summary>
        /// Returns a URL object of the request. Never returns null.
        /// </summary>
        public IUrl Url 
        { 
            get { return _url;  }
        }
        /// <summary>
        /// Returns the request header. Never returns null. All keys must be lower case.
        /// </summary>
        public IDictionary<string, string> Headers
        {
            get { return _headers; }
        }
        /// <summary>
        /// Returns the user agent from the request header
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
        }
        /// <summary>
        /// Returns the number of header or 0, if no header where found.
        /// </summary>
        public int HeaderCount 
        { 
            get { return _headerCount; }
        }
        /// <summary>
        /// Returns the parsed content length request header.
        /// </summary>
        public int ContentLength 
        {
            get
            {
                if (_headers.ContainsKey("Content-Length"))
                    return Int32.Parse(_headers["Content-Length"]);
                return 0; 
            }
        }
        /// <summary>
        /// Returns the parsed content type request header. Never returns null.
        /// </summary>
        public string ContentType 
        { 
            get
            {
                if (_headers.ContainsKey("Content-Type"))
                    return _headers["Content-Type"];
                return "";
            }
        }
        /// <summary>
        /// Returns the request content (body) stream or null if there is no content stream.
        /// </summary>
        public Stream ContentStream
        {
            get { return _contentStream; }
        }
        /// <summary>
        /// Returns the request content (body) as string or null if there is no content.
        /// </summary>
        public string ContentString 
        {
            get { return _contentString; }
        }
        /// <summary>
        /// Returns the request content (body) as byte[] or null if there is no content.
        /// </summary>
        public byte[] ContentBytes
        {
            get { return _contentBytes; }
        }
    }
}
