﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace craftersmine.GameEngine.Network
{
    /// <summary>
    /// Represents web request helper. This class cannot be inherited
    /// </summary>
    public sealed class WebRequestHelper
    {
        /// <summary>
        /// Makes GET request at specified URL
        /// </summary>
        /// <param name="url">URL with GET request</param>
        /// <returns></returns>
        public static string MakeGetRequest(string url)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString(url);
        }

        /// <summary>
        /// Makes POST request at specified URL
        /// </summary>
        /// <param name="url">URL for post request</param>
        /// <param name="data">Data for request</param>
        /// <returns></returns>
        public static string MakePostRequest(string url, string data)
        {
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(data);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }
    }
}
