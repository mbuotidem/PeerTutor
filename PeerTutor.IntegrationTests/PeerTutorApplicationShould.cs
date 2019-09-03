using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Xunit;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.EntityFrameworkCore;
using PeerTutor.Models;

namespace PeerTutor.IntegrationTests
{
    public class PeerTutorApplicationShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        public PeerTutorApplicationShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task RenderHomePage()
        {
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseInMemoryDatabase(databaseName: "TestDB")
            //    .Options;

            //using (var context = new AppDbContext(options))
            //{
            //    var response = await _fixture.Client.GetAsync("/Home");

            //    response.EnsureSuccessStatusCode();

            //    var responseString = await response.Content.ReadAsStringAsync();

            //    Assert.Contains("Welcome to Peer Tutor!", responseString);
            //}

            var response = await _fixture.Client.GetAsync("/Home");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome to Peer Tutor!", responseString);

        }
    }
}
