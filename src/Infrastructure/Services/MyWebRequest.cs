using System;
using System.IO;
using System.Net;
using System.Text;

namespace Crypto.Infrastructure.Services
{
    public class MyWebRequest
    {
        private readonly WebRequest _request;
        private Stream _dataStream;

        public MyWebRequest(string url)
        {
            // Create a request using a URL that can receive a post.

            _request = WebRequest.Create(url);
        }

        public MyWebRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                _request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public MyWebRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            var postData = data;
            var byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            _request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            _request.ContentLength = byteArray.Length;

            // Get the request stream.
            _dataStream = _request.GetRequestStream();

            // Write the data to the request stream.
            _dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            _dataStream.Close();

        }

        public string GetResponse()
        {
            // Get the original response.
            var response = _request.GetResponse();

            // Get the stream containing all content returned by the requested server.
            _dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(_dataStream ?? throw new InvalidOperationException());

            // Read the content fully up to the end.
            var responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            _dataStream.Close();
            response.Close();

            return responseFromServer;
        }

    }
}
