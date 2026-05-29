using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    Assert.True(results.Count >= 1);
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
                    Assert.NotNull(result);
                    Assert.Equal(export.Id, result.Id);
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
                    Assert.NotNull(result);
                    Assert.Equal("reject", result.Type);
                    Assert.Equal("waiting", result.State);
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
                    Assert.NotNull(result);
                    Assert.Equal("whitelist", result.Type);
                    Assert.Equal("waiting", result.State);
                }
                catch (ExportThrottledTestException)
                {
                }
            }
        }

        [Trait("Category", "exports/allowlist.json")]
        public class Allowlist : Exports
        {
            public Allowlist(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_export_allowlist()
            {
                string notifyEmail = Guid.NewGuid().ToString("N") + "@mandrilldotnet.org";
                try
                {
                    var result = await HandleExportThrottleError(Api.Exports.AllowlistAsync(notifyEmail));
                    Assert.NotNull(result);
                    Assert.Equal("whitelist", result.Type);
                    Assert.Equal("waiting", result.State);
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
                    Assert.NotNull(result);
                    Assert.Equal("activity", result.Type);
                    Assert.Equal("waiting", result.State);
                }
                catch (ExportThrottledTestException)
                {

                }
            }
        }

    }
}
