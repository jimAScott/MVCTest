using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;

namespace MVCTest.Controllers
{
    public class PageController : Controller
    {
        //
        // GET: /Page/

        public ActionResult Index()
        {
			//Add in the site type to the view bag to show updating variables for Octopus
			ViewBag.environment = ConfigurationManager.AppSettings["environment"].ToString();
			ViewBag.site = ConfigurationManager.AppSettings["site"].ToString();
            return View();
        }
        public ActionResult JamesS()
        {
			string postData = "{\"person\": {\"email\": \"James22@example.com\"}}";
			byte[] data = Encoding.UTF8.GetBytes(postData);

			HttpWebRequest request = WebRequest.Create("https://unileversandbox.nationbuilder.com/api/v1/people?access_token=a3bb2accab1a0ba070196f142f07794bcbd75939d2a638c334e69a7ff1d2185a") as HttpWebRequest;
			request.Method = "POST";
			request.Accept = "application/json";
			request.ContentType = "application/json";
			request.ContentLength = data.Length;
			request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

			StreamWriter requestStream =  new StreamWriter(request.GetRequestStream());
			requestStream.Write(postData);
			requestStream.Close();

			HttpWebResponse response = request.GetResponse() as HttpWebResponse;
			Stream responseStream = response.GetResponseStream();

			StringBuilder sb = new StringBuilder();

			using (StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					sb.Append(line);
				}
			}			

			ViewBag.Response = sb.ToString();

			Response.ContentType = "text/json";
			Response.Write(sb.ToString());
			Response.End();

            return View();}
	    }
}
