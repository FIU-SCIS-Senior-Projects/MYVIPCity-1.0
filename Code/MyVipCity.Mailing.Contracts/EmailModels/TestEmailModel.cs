using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class TestEmailModel {

		public string From
		{
			get;set;
		}

		public string To
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}
	}
}
