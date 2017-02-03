using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using MyVipCity.Mailing.Sendgrid;

namespace TestingConsoleApp {

	class Program {
		static void Main(string[] args) {
			Mine().Wait();
		}

		static async Task Mine() {
			string apiKey = "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";
			IEmailService myEmailService = new SendGridEmailService(apiKey);

			TestEmailModel testModel = new TestEmailModel {
				To = "dseba006@fiu.edu",
				From = "contact@myvipcity.com",
				Name = "Darshana"
			};

			await myEmailService.SendTestEmail(testModel);
		}
	}
}
