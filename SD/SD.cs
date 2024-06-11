namespace Contacts_Utility
{
	public static class SD
	{
		public enum ContactType
		{
			Personal,
			Buisness,
			Other
		}
		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
		public static string SesstionToken = "JWTToken";
	}
}
