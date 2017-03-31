using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Common.WebApi
{
    public static class HttpResponseMessageExtension
    {
        public static HttpResponseMessage String(string message)
        {
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent(message, Encoding.GetEncoding("UTF-8"), "text/plain");
            return responseMessage;
        }

        public static HttpResponseMessage Json<T>(T value)
        {
            return SerializeObject(value);
        }

        public static HttpResponseMessage Xml<T>(T value)
        {
            return SerializeObject(value,"xml");
        }

        public static HttpResponseMessage SerializeObject<T>(T value,string format="json")
        {
            var responseMessage = new HttpResponseMessage();
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            if (format.ToLower() == "xml")
            {
                //T类是Serializable
                formatter=new XmlMediaTypeFormatter();
            }
            responseMessage.Content = new ObjectContent(typeof(T), value, formatter);
            return responseMessage;
        }
    }
}
