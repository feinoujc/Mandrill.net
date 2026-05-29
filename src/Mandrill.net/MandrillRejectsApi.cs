#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Model;

namespace Mandrill
{
    internal partial class MandrillRejectsApi : IMandrillRejectsApi
    {
        public MandrillRejectsApi(MandrillApi mandrillApi)
        {
            MandrillApi = mandrillApi;
        }

        public MandrillApi MandrillApi { get; private set; }
        public Task<MandrillRejectAddResponse> AddAsync(string email, string? comment = null, string? subaccount = null)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return MandrillApi.PostAsync<MandrillRejectRequest, MandrillRejectAddResponse>("rejects/add",
                new MandrillRejectRequest
                {
                    Email = email,
                    Comment = comment,
                    Subaccount = subaccount
                });
        }

        public Task<MandrillRejectDeleteResponse> DeleteAsync(string email, string? subaccount = null)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return MandrillApi.PostAsync<MandrillRejectRequest, MandrillRejectDeleteResponse>("rejects/delete",
                new MandrillRejectRequest
                {
                    Email = email,
                    Subaccount = subaccount
                });
        }

        public Task<IList<MandrillRejectInfo>> ListAsync(string? email = null, bool? includeExpired = null, string? subaccount = null)
        {
            return MandrillApi.PostAsync<MandrillRejectRequest, IList<MandrillRejectInfo>>("rejects/list",
                new MandrillRejectRequest
                {
                    Email = email,
                    IncludeExpired = includeExpired,
                    Subaccount = subaccount
                });
        }

        public Task<MandrillSmsReject> AddSmsAsync(string phone, string? comment = null, string? subaccount = null)
        {
            if (phone == null) throw new ArgumentNullException(nameof(phone));
            return MandrillApi.PostAsync<MandrillSmsRejectRequest, MandrillSmsReject>("rejects/add-sms",
                new MandrillSmsRejectRequest
                {
                    Phone = phone,
                    Comment = comment,
                    Subaccount = subaccount
                });
        }

        public Task<MandrillSmsReject> DeleteSmsAsync(string phone, string? subaccount = null)
        {
            if (phone == null) throw new ArgumentNullException(nameof(phone));
            return MandrillApi.PostAsync<MandrillSmsRejectRequest, MandrillSmsReject>("rejects/delete-sms",
                new MandrillSmsRejectRequest
                {
                    Phone = phone,
                    Subaccount = subaccount
                });
        }

        public Task<IList<MandrillSmsRejectInfo>> ListSmsAsync(string? phone = null, bool? includeExpired = null, string? subaccount = null)
        {
            return MandrillApi.PostAsync<MandrillSmsRejectRequest, IList<MandrillSmsRejectInfo>>("rejects/list-sms",
                new MandrillSmsRejectRequest
                {
                    Phone = phone,
                    IncludeExpired = includeExpired,
                    Subaccount = subaccount
                });
        }
    }
}
