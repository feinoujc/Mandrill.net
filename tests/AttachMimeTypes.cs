using Mandrill.Model;
using System;
using Xunit;

namespace Tests
{
    [Trait("Category", "AttachMimeTypes")]
    [Collection("AttachMimeTypes")]
    public class AttachMimeTypes
    {
        [Fact]
        public void AreAllEnumValuesPresent_ShouldReturnTrueWhenAllValuesPresent()
        {
            foreach (MandrillAttachmentType myType in Enum.GetValues(typeof(MandrillAttachmentType)))
            {
                try
                {
                    MandrillAttachmentMime.GetMimeType(myType);
                }
                catch
                {
                    Assert.Fail($"{myType} is not in MandrillAttachmentMime dictionary");
                }
            }
            Assert.True(true);
        }
    }
}
