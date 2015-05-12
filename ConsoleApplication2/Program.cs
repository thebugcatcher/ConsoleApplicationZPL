using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ConsoleApplication2
{
    class Program
    {
        static string zpl_code;
        
        static void Main(string[] args)
        {
            
            if (args.Length > 0)
            {
                zpl_code = string.Join("", args);
                Console.WriteLine("You provided the following code: " + zpl_code );
                send_request(zpl_code);
            }
            else
            {
                Console.WriteLine("Please provide the zpl code!");
            }

        }

        static void send_request(string zpl_code)
        {
            byte[] zpl = Encoding.UTF8.GetBytes(zpl_code);

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/12dpmm/labels/4x6/0/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create("label.png");
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();
            }
            catch (WebException we)
            {
                Console.WriteLine("Error: {0}", we.Status);
            }
        }
        static string ConvertArraytoString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
            }
            return builder.ToString();
        }
    }
}
