using Mandrill.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                catch (Exception ex)
                {
                    Assert.True(false, $"{myType} is not in MandrillAttachmentMime dictionary");
                }
            }
            Assert.True(true);
        }
    }
}
