using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Mandrill.Model;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "exports")]
    [Collection("exports")]
    public class Exports(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        class ExportThrottledTestException : Exception
        {
            public ExportThrottledTestException() { }
            public ExportThrottledTestException(string message) : base(message) { }
            public ExportThrottledTestException(string message, Exception inner) : base(message, inner) { }

        }

        protected async Task<T> HandleExportThrottleError<T>(Task<T> method)
        {
            try
            {
                return await method;
            }
            catch (MandrillException mex)
            {
                if (mex.Code == -99 && mex.Name == "UserError")
                {
                    throw new ExportThrottledTestException(mex.Message, mex);
                }
                throw;
            }
        }

        public virtual Task InitializeAsync() => Task.CompletedTask;
        public virtual Task DisposeAsync() => Task.CompletedTask;

        [Trait("Category", "exports/list.json")]
        public class List : Exports
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_all()
            {
                var results = await Api.Exports.ListAsync();
                var found = results.Cast<MandrillExportInfo>().OrderBy(x => x.Id).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Output.WriteLine("no exports found.");
                }
            }
        }

        [Trait("Category", "exports/info.json")]
        public class Info : Exports
        {
            public Info(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_retrieve_info()
            {
                var export = (await Api.Exports.ListAsync()).Cast<MandrillExportInfo>().LastOrDefault();
                if (export != null)
                {
                    var result = await Api.Exports.InfoAsync(export.Id);
                    result.Should().NotBeNull();
                    result.Id.Should().Be(export.Id);
                }
                else
                {
                    Output.WriteLine("no exports found");
                }
            }
        }

        [Trait("Category", "exports/rejects.json")]
        public class Rejects : Exports
        {
            public Rejects(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_export_info()
            {
                string notifyEmail = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                try
                {
                    var result = await HandleExportThrottleError(Api.Exports.RejectsAsync(notifyEmail));
                    result.Should().NotBeNull();
                    result.Type.Should().Be("reject");
                    result.State.Should().Be("waiting");
                }
                catch (ExportThrottledTestException)
                {
                }
            }
        }

        [Trait("Category", "exports/whitelist.json")]
        public class Whitelist : Exports
        {
            public Whitelist(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_export_info()
            {
                
                string notifyEmail = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                try
                {
                    var result = await HandleExportThrottleError(Api.Exports.WhitelistAsync(notifyEmail));
                    result.Should().NotBeNull();
                    result.Type.Should().Be("whitelist");
                    result.State.Should().Be("waiting");
                }
                catch (ExportThrottledTestException)
                {
                }
            }
        }

        [Trait("Category", "exports/activity.json")]
        public class Activity : Exports
        {
            public Activity(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_export_activity()
            {
                string notifyEmail = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                DateOnly? dateFrom = null;
                DateOnly? dateTo = null;
                IList<string> tags = null;
                IList<string> senders = null;
                IList<string> states = null;
                IList<string> apiKeys = null;

                try
                {
                    var result = await HandleExportThrottleError(Api.Exports.ActivityAsync(notifyEmail,
                        dateFrom,
                        dateTo,
                        tags,
                        senders,
                        states,
                        apiKeys));
                    result.Should().NotBeNull();
                    result.Type.Should().Be("activity");
                    result.State.Should().Be("waiting");
                }
                catch (ExportThrottledTestException)
                {

                }
            }
        }

    }
}
