using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
namespace Mandrill.Model
{
    public static class WebHookSignatureHelper
    {
        public static bool VerifyWebHookSignature(string signature, string key, string absoluteUri, IEnumerable<KeyValuePair<string, string>> formPost)
        {
            if (signature == null) throw new ArgumentNullException(nameof(signature));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (absoluteUri == null) throw new ArgumentNullException(nameof(absoluteUri));
            if (formPost == null) throw new ArgumentNullException(nameof(formPost));

            var toSign = new StringBuilder();
            toSign.Append(absoluteUri);
            foreach (var group in formPost.GroupBy(x => x.Key).OrderBy(g => g.Key))
            {
                toSign.Append(group.Key);
                toSign.Append(string.Join(",", group.Select(x => x.Value)));
            }

            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
            {
                var bytes = Encoding.UTF8.GetBytes(toSign.ToString());
                var hash = Convert.ToBase64String(hmac.ComputeHash(bytes));

                return signature == hash;
            }
        }

        public static bool VerifyWebHookSignature(string signature, string key, Uri absoluteUri, IEnumerable<KeyValuePair<string, string>> formPost)
        {
            if (absoluteUri == null) throw new ArgumentNullException(nameof(absoluteUri));
            if (!absoluteUri.IsAbsoluteUri) throw new ArgumentException("uri must be an absolute uri", nameof(absoluteUri));

            return VerifyWebHookSignature(signature, key, absoluteUri.ToString(), formPost);
        }
    }
}
