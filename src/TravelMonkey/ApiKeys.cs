namespace TravelMonkey
{
	public static class ApiKeys
	{
        #warning You need to set up your API keys.
		public static string ComputerVisionApiKey = "20ff4602d34f47da83f31ad946bc0c1d";
		public static string TranslationsApiKey = "b9a10f0a398149748393621e6e8eb25d";
		public static string BingImageSearch = "b9a10f0a398149748393621e6e8eb25d";

		// Change this to the Azure Region you are using
		public static string ComputerVisionEndpoint = "https://westeurope.api.cognitive.microsoft.com/";
		// public static string TranslationsEndpoint = "https://api.cognitive.microsofttranslator.com/";
		public static string TranslationsEndpoint = "http://localhost:7071/api/translate";
		
		
	}
}