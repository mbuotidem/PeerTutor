using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;


using Microsoft.Net.Http.Headers;

namespace PeerTutor.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }
        
        public static readonly string AntiForgeryFieldName = "__ATFField";

        public static readonly string AntiForgeryCookieName = "AFTCookie";


        public TestServerFixture()
        {
            var builder = new WebHostBuilder();

            builder.ConfigureAppConfiguration((context, config) =>
            {

                var builtConfig = config.Build();

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

                config.AddAzureKeyVault(
                    "https://Web.vault.azure.net/",
                    keyVaultClient,
                    new DefaultKeyVaultSecretManager());

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            })

                .UseContentRoot(GetContentRootPath())
                .UseEnvironment("Development")
                .UseStartup<PeerTutor.Startup>()
                .ConfigureServices(X =>
                {
                    X.AddAntiforgery(t =>
                    {
                        t.Cookie.Name = AntiForgeryCookieName;
                        t.FormFieldName = AntiForgeryFieldName;
                    });
                });


            _testServer = new TestServer(builder);

            Client = _testServer.CreateClient();

        }

        public async Task<(string fieldValue, string cookieValue)> ExtractAntiForgeryValues(HttpResponseMessage response)
        {
            return (ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync()),
                                            ExtractAntiForgeryCookieValueFrom(response));
        }

        private string GetContentRootPath()
        {
            string testProjectPath = System.AppContext.BaseDirectory;

            var relativePathToWebProject = @"..\..\..\..\PeerTutor";

            return Path.Combine(testProjectPath, relativePathToWebProject);
        }

        private string ExtractAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch =
                Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
        }

        private static string ExtractAntiForgeryCookieValueFrom(
            HttpResponseMessage response)
        {
            string antiForgeryCookie = response.Headers.GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains(AntiForgeryCookieName));

            if (antiForgeryCookie is null)
            {
                throw new ArgumentException(
                    $"Cookie '{AntiForgeryCookieName}' not found in HTTP response",
                    nameof(response));
            }

            string antiForgeryCookieValue =
                SetCookieHeaderValue.Parse(antiForgeryCookie).Value.ToString();

            return antiForgeryCookieValue;
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}