using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Tests
{
    [Trait("Category", "exports")]
    [Collection("exports")]
    public class Exports : IntegrationTest
    {
        class ExportThrottledTestException : Exception
        {
            public ExportThrottledTestException() {}
            public ExportThrottledTestException(string message) : base(message) {}
            public ExportThrottledTestException(string message, Exception inner): base(message, inner) {}

        }

        protected async Task<T> HandleExportThrottleError<T>(Task<T> method)
        {
            try
            {
                return await method;
            }
            catch(MandrillException mex)
            {
                if(mex.Code == -99 && mex.Name == "UserError")
                {
                    throw new ExportThrottledTestException(mex.Message, mex);
                }
                throw mex;
            }
        }

        [Trait("Category", "exports/list.json")]
        public class List : Exports
        {
            [Fact]
            public async Task Can_list_all()
            {
                var results = await Api.Exports.ListAsync();

                //the api doesn't return results immediately, it may return no results
                var found = results.OrderBy(x => x.Id).FirstOrDefault();
                if (found != null)
                {
                    results.Count.Should().BeGreaterOrEqualTo(1);
                }
                else
                {
                    Console.Error.WriteLine("no exports found.");
                }
            }
        }

        [Trait("Category", "exports/info.json")]
        public class Info : Exports
        {
            [Fact]
            public async Task Can_retrieve_info()
            {
                var export = (await Api.Exports.ListAsync()).LastOrDefault();
                if (export != null)
                {
                    var result = await Api.Exports.InfoAsync(export.Id);
                    result.Should().NotBeNull();
                    result.Id.Should().Be(export.Id);
                }
                else
                {
                    Console.Error.WriteLine("no exports found");
                }
            }
        }

        [Trait("Category", "exports/rejects.json")]
        public class Rejects : Exports
        {
            [Fact]
            public async Task Can_export_info()
            {
                // notifyEmail is an optional field that will
                // be emailed when the export is done compiling. omitting for test purposes.
                string notifyEmail = string.Empty;
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
        public class Whitelist: Exports
        {
            [Fact]
            public async Task Can_export_info()
            {
                // notifyEmail is an optional field that will
                // be emailed when the export is done compiling. omitting for test purposes.
                string notifyEmail = string.Empty;
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
            [Fact]
            public async Task Can_export_activity()
            {
                string notifyEmail = string.Empty;
                DateTime? dateFrom = null;
                DateTime? dateTo = null;
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
                catch(ExportThrottledTestException)
                {

                }
            }
        }

    }
}
