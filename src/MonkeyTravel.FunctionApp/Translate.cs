using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MonkeyTravel.FunctionApp
{
    public static class Translate
    {
        private static readonly string TranslateKey = Environment.GetEnvironmentVariable("TRANSLATE_KEY");
        private static readonly string TranslateUri = Environment.GetEnvironmentVariable("TRANSLATE_URI");
        [FunctionName("Translate")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var content = await req.Content.ReadAsStringAsync();
            var requestBody = JsonConvert.DeserializeObject<TranslationRequest>(content);
            var sb = new StringBuilder();

            foreach (var to in requestBody.To)
            {
                sb.Append($"&to={to}");
            }

            var toLanguages = sb.ToString();
            var requestUri = string.IsNullOrEmpty(requestBody.From) ? $"{TranslateUri}{toLanguages}" : $"{TranslateUri}&from={requestBody.From}{toLanguages}";


            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key",TranslateKey );
            var body = new object[] {new {Text = requestBody.Text}};
            var json = JsonConvert.SerializeObject(body);
            var request = new HttpRequestMessage(HttpMethod.Post,requestUri )
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            log.LogInformation($"Translating {requestBody.From} {requestBody.Text} to {requestBody.To} ");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return  new BadRequestObjectResult("Please provide valid request body");
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TranslationResult[]>(responseContent);

            return new OkObjectResult(result.ToList());
        }
    }

    public class TranslationRequest
    {
        public string From { get; set; }
        public IEnumerable<string> To { get; set; }
        public string Text { get; set; }
    }

    /// <summary>
    /// The C# classes that represents the JSON returned by the Translator Text API.
    /// </summary>
    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; }
        public TextResult SourceText { get; set; }
        public Translation[] Translations { get; set; }
    }

    public class DetectedLanguage
    {
        public string Language { get; set; }
        public float Score { get; set; }
    }

    public class TextResult
    {
        public string Text { get; set; }
        public string Script { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public TextResult Transliteration { get; set; }
        public string To { get; set; }
        public Alignment Alignment { get; set; }
        public SentenceLength SentLen { get; set; }
    }

    public class Alignment
    {
        public string Proj { get; set; }
    }

    public class SentenceLength
    {
        public int[] SrcSentLen { get; set; }
        public int[] TransSentLen { get; set; }
    }
}