using EcommerceBackendOrderSystem.API;
using EcommerceBackendOrderSystem.Domain.Entities;
using EcommerceBackendOrderSystem.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommerceBackendOrderSystem.UnitTestCases.UnitTests
{
    [TestFixture]
    public class RoleAccessTests
    {
        private HttpClient _client;
        private JwtTokenGenerator _tokenGenerator;

        [SetUp]
        public void Setup()
        {
            // Setup in-memory config for JwtTokenGenerator
            var inMemorySettings = new Dictionary<string, string>
            {
                {"JwtSettings:SecretKey", "ThisIsMySecretKey12345678901234567890"},
                {"JwtSettings:issuer}", "issuer" },
                { "JWtSettings:audience", "audience"},
                {"JwtSettings:ExpiryMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenGenerator = new JwtTokenGenerator(configuration);

            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        private string GenerateToken(int userId, string username, List<string> roles)
        {
            DateTime expiration;
            var user = new User { Id = userId, Username = username };
            return _tokenGenerator.GenerateToken(user, roles, out expiration);
        }

        [Test]
        public async Task AdminEndpoint_Should_Return_Forbidden_For_NonAdmin()
        {
            var token = GenerateToken(2, "normaluser", new List<string> { "User" });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/api/Admin/RestrictedAction", null);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        public async Task AdminEndpoint_Should_Return_Success_For_Admin()
        {
            var token = GenerateToken(1, "adminuser", new List<string> { "Admin" });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsync("/api/Admin/RestrictedAction", null);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

    }
}
