namespace Tests
{
    using FtpProxy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using NUnit.Framework;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class FtpProxyTest
    {
        private readonly HttpClient Client;

        public FtpProxyTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());

            this.Client = server.CreateClient();
        }
   
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var response = await this.Client.PostAsync("/api/proxy?source=d:\\temp files\\38000.jpg", null);
            response.EnsureSuccessStatusCode();

            Assert.Equals(HttpStatusCode.OK, response.StatusCode);
        }
    }
}