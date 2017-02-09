using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVipCity.Mailing.Sendgrid;

namespace TestConsoleApp {
	class Program {
		private static void Main(string[] args) {
			var apiKey = ConfigurationManager.AppSettings["myvipcity:send-grid-api"];
			SendGridEmailService sendGridEmailService = new SendGridEmailService(apiKey);
			Console.ReadLine();
		}
	}
}
