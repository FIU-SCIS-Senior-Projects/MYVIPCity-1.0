﻿using System.Collections.Generic;

namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class BasicEmailModel {

		public string From
		{
			get;
			set;
		}

		public string To
		{
			get;
			set;
		}

		public string Subject
		{
			get;
			set;
		}

		public string Body
		{
			get;
			set;
		}

		public ICollection<string> Bccs
		{
			get;
			set;
		}

		public ICollection<string> Tos
		{
			get;
			set;
		}
	}
}
