using System;
using Mandrill;
using NUnit.Framework;

namespace Tests
{
    [Category("integration")]
    internal abstract class IntegrationTest
    {
        private static readonly Lazy<string> ApiKeyLazy = new Lazy<string>(() =>
        {
            var apiKey = Environment.GetEnvironmentVariable("MANDRILL_API_KEY", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new AssertionException("You must set the user environment variable MANDRILL_API_KEY in order to run these tests. " +
                                             "Go to https://mandrillapp.com/ to obtain an api key.");
            }
            return apiKey;
        });

        protected string ApiKey
        {
            get { return ApiKeyLazy.Value; }
        }
        private Lazy<MandrillApi> LazyApi;

        protected MandrillApi Api { get { return LazyApi.Value; } }

        [TestFixtureSetUp]
        public virtual void SetUp()
        {
            LazyApi = new Lazy<MandrillApi>(() => new MandrillApi(ApiKey));
        }

        [TestFixtureTearDown]
        public virtual void TearDown()
        {
            LazyApi = null;
        }
    }
}