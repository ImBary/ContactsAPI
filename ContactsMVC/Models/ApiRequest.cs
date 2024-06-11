﻿using System.Security.AccessControl;
using static Contacts_Utility.SD;

namespace ContactsMVC.Models
{
	public class ApiRequest
	{
		public ApiType ApiType { get; set; } = ApiType.GET;
		public string Url { get; set; }
		public object Data { get; set; }
		public string Token {  get; set; }
	}
}
