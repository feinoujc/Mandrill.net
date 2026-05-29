using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mandrill;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [Trait("Category", "metadata")]
    [Collection("metadata")]
    public class Metadata(MandrillFixture fixture, ITestOutputHelper output) : IClassFixture<MandrillFixture>, IAsyncLifetime
    {
        protected IMandrillApi Api => fixture.Api;
        protected ITestOutputHelper Output => output;

        public virtual Task InitializeAsync() => Task.CompletedTask;

        public virtual async Task DisposeAsync()
        {
            foreach (var name in _added)
            {
                try
                {
                    await Api.Metadata.DeleteAsync(name);
                }
                catch
                {
                    // best-effort cleanup
                }
            }
        }

        private readonly HashSet<string> _added = new HashSet<string>();

        [Trait("Category", "metadata/list.json")]
        public class List : Metadata
        {
            public List(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_list_metadata()
            {
                var results = await Api.Metadata.ListAsync();
                results.Should().NotBeNull();
                var found = results.FirstOrDefault();
                if (found != null)
                {
                    found.Name.Should().NotBeNullOrEmpty();
                    found.State.Should().NotBeNullOrEmpty();
                }
                else
                {
                    Output.WriteLine("no metadata fields found.");
                }
            }
        }

        [Trait("Category", "metadata/add.json")]
        public class Add : Metadata
        {
            public Add(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_add_metadata()
            {
                var name = "test_" + Guid.NewGuid().ToString("N")[..8];
                var result = await Api.Metadata.AddAsync(name);
                _added.Add(name);
                result.Should().NotBeNull();
                result.Name.Should().Be(name);
                result.State.Should().NotBeNullOrEmpty();
            }
        }

        [Trait("Category", "metadata/update.json")]
        public class Update : Metadata
        {
            public Update(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_update_metadata()
            {
                var name = "test_" + Guid.NewGuid().ToString("N")[..8];
                await Api.Metadata.AddAsync(name);
                _added.Add(name);

                var result = await Api.Metadata.UpdateAsync(name, "<a>{{value}}</a>");
                result.Should().NotBeNull();
                result.Name.Should().Be(name);
            }
        }

        [Trait("Category", "metadata/delete.json")]
        public class Delete : Metadata
        {
            public Delete(MandrillFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [Fact]
            public async Task Can_delete_metadata()
            {
                var name = "test_" + Guid.NewGuid().ToString("N")[..8];
                await Api.Metadata.AddAsync(name);

                var result = await Api.Metadata.DeleteAsync(name);
                result.Should().NotBeNull();
                result.Name.Should().Be(name);
            }
        }
    }
}
