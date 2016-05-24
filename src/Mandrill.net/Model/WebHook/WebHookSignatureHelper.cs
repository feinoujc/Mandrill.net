#if NET45 || NETSTANDARD1_3
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Mandrill.Model
{
    public static class WebHookSignatureHelper
    {
        public static bool VerifyWebHookSignature(string signature, string key, Uri absoluteUri, NameValueCollection formPost)
        {
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (absoluteUri == null) throw new ArgumentNullException(nameof(absoluteUri));
            if (formPost == null) throw new ArgumentNullException(nameof(formPost));
            if (!absoluteUri.IsAbsoluteUri) throw new ArgumentException("uri must be an absolute uri", nameof(absoluteUri));

            var toSign = new StringBuilder();
            toSign.Append(absoluteUri);
            foreach (var k in formPost.AllKeys.OrderBy(k => k))
            {
                toSign.Append(k);
                toSign.Append(formPost[k]);
            }

            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var bytes = Encoding.UTF8.GetBytes(toSign.ToString());
                var hash = Convert.ToBase64String(hmac.ComputeHash(bytes));

                return signature == hash;
            }
        }
    }
}
#endif
