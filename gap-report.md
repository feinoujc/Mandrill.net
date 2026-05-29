# Mandrill.net API Gap Report

## Context

Comparing the Mandrill OpenAPI spec at `spec/mandrill-openapi.json` (v1.4.0, 99 total operations across 15 tags) against the current implementation. The goal is a complete picture of: (1) operation groups that are entirely absent, (2) individual operations missing within partially-implemented groups, and (3) request/response POCO discrepancies in the existing code.

The codebase follows a consistent pattern: each group has an `IMandrillXxxApi` interface, a `MandrillXxxApi : IMandrillXxxApi` implementation, a `MandrillXxxRequest : MandrillRequestBase` POCO and a set of response POCOs in `src/Mandrill.net/Model/`. All groups hang off `MandrillApi.cs` as lazy-init properties. JSON serialization uses `System.Text.Json` with custom converters in `Serialization/` (notably `UnixDateTimeConverter` for Unix timestamps, `IsoDateTimeConverter` for ISO strings, and `EmptyArrayOrDictionaryConverter`).

**Current coverage: 75 of 99 operations (76%)**

---

## Category 1: Completely Missing Operation Groups

Four API groups have zero implementation — no interface, no class, no models.

---

### 1.1 IPs (13 operations — 0 implemented)

All operations live under `/ips/*`. None exist in the codebase.

**New files needed:**

- `IMandrillIpsApi.cs`
- `MandrillIpsApi.cs`

**New POCOs needed:**

`MandrillIpCustomDns` (nested in `MandrillIpInfo`):

```
Enabled: bool
Valid: bool
Error: string?
```

`MandrillIpWarmupStatus` (nested in `MandrillIpInfo`, schema name: `IPsWarmupStatus`):

```
WarmingUp: bool                   // warming_up
StartAt: DateTime?                // start_at — ISO datetime string
EndAt: DateTime?                  // end_at — ISO datetime string
```

`MandrillIpInfo` (schema name: `IP`):

```
Ip: string                        // ip
CreatedAt: DateTime               // created_at
Pool: string                      // pool
Domain: string                    // domain
CustomDns: MandrillIpCustomDns    // custom_dns
Warmup: MandrillIpWarmupStatus    // warmup
```

`MandrillIpPool` (schema name: `IPsPool`):

```
Name: string                      // name
CreatedAt: DateTime               // created_at
Ips: List<MandrillIpInfo>         // ips
```

`MandrillIpProvisionResponse`:

```
RequestedAt: DateTime             // requested_at
```

`MandrillIpDeleteResponse`:

```
Ip: string                        // ip
Deleted: bool                     // deleted
```

`MandrillIpDeletePoolResponse`:

```
Pool: string                      // pool
Deleted: bool                     // deleted
```

`MandrillIpCheckCustomDnsResponse`:

```
Valid: bool                       // valid
Error: string?                    // error
```

**Request POCOs needed** (all extend `MandrillRequestBase`):

| Request class                   | Fields                                            |
| ------------------------------- | ------------------------------------------------- |
| `MandrillIpRequest`             | `Ip: string`                                      |
| `MandrillIpPoolRequest`         | `Pool: string`                                    |
| `MandrillIpProvisionRequest`    | `Warmup: bool?`, `Pool: string?`                  |
| `MandrillIpSetPoolRequest`      | `Ip: string`, `Pool: string`, `CreatePool: bool?` |
| `MandrillIpSetCustomDnsRequest` | `Ip: string`, `Domain: string`                    |

**Interface methods:**

```csharp
Task<IList<MandrillIpInfo>> ListAsync();
Task<MandrillIpInfo> InfoAsync(string ip);
Task<MandrillIpProvisionResponse> ProvisionAsync(bool? warmup = null, string? pool = null);
Task<MandrillIpInfo> StartWarmupAsync(string ip);
Task<MandrillIpInfo> CancelWarmupAsync(string ip);
Task<MandrillIpInfo> SetPoolAsync(string ip, string pool, bool? createPool = null);
Task<MandrillIpDeleteResponse> DeleteAsync(string ip);
Task<IList<MandrillIpPool>> ListPoolsAsync();
Task<MandrillIpPool> PoolInfoAsync(string pool);
Task<MandrillIpPool> CreatePoolAsync(string pool);
Task<MandrillIpDeletePoolResponse> DeletePoolAsync(string pool);
Task<MandrillIpCheckCustomDnsResponse> CheckCustomDnsAsync(string ip, string domain);
Task<MandrillIpInfo> SetCustomDnsAsync(string ip, string domain);
```

**Required changes to `MandrillApi.cs`:** Add `IMandrillIpsApi Ips { get; }` property (and lazy-init backing field).

---

### 1.2 MC Templates (4 operations — 0 implemented)

All operations live under `/mctemplates/*`. These are Mailchimp campaign templates (distinct from Mandrill `/templates/*`).

**New files needed:**

- `IMandrillMcTemplatesApi.cs`
- `MandrillMcTemplatesApi.cs`

**New POCOs needed:**

`MandrillMcTemplate` (schema: `MctemplatesMcTemplate`):

```
McTemplateId: int                 // mc_template_id
McTemplateName: string            // mc_template_name
Labels: List<string>              // labels
Code: string                      // code
Subject: string                   // subject
FromEmail: string                 // from_email
FromName: string                  // from_name
Text: string                      // text
PublishCode: string               // publish_code
PublishText: string               // publish_text
PublishedAt: DateTime?            // published_at
CreatedAt: DateTime               // created_at
UpdatedAt: DateTime               // updated_at
DraftUpdatedAt: DateTime?         // draft_updated_at
IsBrokenTemplate: bool            // is_broken_template
```

`MandrillMcTemplateTimeSeries` — same shape as existing `MandrillMessageTimeSeries` / `MandrillTagTimeSeries` (inherits `MandrillAggregateStatisticsBase` + `Time: DateTime`).

**Request POCOs needed:**

| Request class                     | Fields                                                                                                        |
| --------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| `MandrillMcTemplateRequest`       | `McTemplateId: int`                                                                                           |
| `MandrillMcTemplateListRequest`   | `SearchQuery: string?`                                                                                        |
| `MandrillMcTemplateRenderRequest` | `McTemplateId: int`, `McTemplateVersion: string?` ("published"/"draft"), `MergeVars: List<MandrillMergeVar>?` |

**Interface methods:**

```csharp
Task<MandrillMcTemplate> InfoAsync(int mcTemplateId);
Task<IList<MandrillMcTemplate>> ListAsync(string? searchQuery = null);
Task<MandrillTemplateRenderResponse> RenderAsync(int mcTemplateId, string? mcTemplateVersion = null, List<MandrillMergeVar>? mergeVars = null);
Task<IList<MandrillMcTemplateTimeSeries>> TimeSeriesAsync(int mcTemplateId);
```

**Required changes to `MandrillApi.cs`:** Add `IMandrillMcTemplatesApi McTemplates { get; }` property.

---

### 1.3 Metadata (4 operations — 0 implemented)

All operations live under `/metadata/*`.

**New files needed:**

- `IMandrillMetadataApi.cs`
- `MandrillMetadataApi.cs`

**New POCO needed:**

`MandrillMetadataInfo`:

```
Name: string                      // name
State: string                     // state — "active"|"delete"|"index"|"failed"
ViewTemplate: string?             // view_template
```

**Request POCOs needed:**

| Request class                                   | Fields                                  |
| ----------------------------------------------- | --------------------------------------- |
| `MandrillMetadataAddRequest`                    | `Name: string`, `ViewTemplate: string?` |
| `MandrillMetadataUpdateRequest`                 | `Name: string`, `ViewTemplate: string`  |
| `MandrillMetadataDeleteRequest`                 | `Name: string`                          |
| (list uses base `MandrillRequestBase` directly) | —                                       |

**Interface methods:**

```csharp
Task<IList<MandrillMetadataInfo>> ListAsync();
Task<MandrillMetadataInfo> AddAsync(string name, string? viewTemplate = null);
Task<MandrillMetadataInfo> UpdateAsync(string name, string viewTemplate);
Task<MandrillMetadataInfo> DeleteAsync(string name);
```

**Required changes to `MandrillApi.cs`:** Add `IMandrillMetadataApi Metadata { get; }` property.

---

### 1.4 URLs / Tracking Domains (4 operations — 0 implemented)

All operations live under `/urls/*`.

**New files needed:**

- `IMandrillUrlsApi.cs`
- `MandrillUrlsApi.cs`

**New POCOs needed:**

`MandrillTrackingDomainCname`:

```
Valid: bool                       // valid
ValidAfter: string?               // valid_after
Error: string?                    // error
```

`MandrillTrackingDomain` (schema: `TrackingDomain`):

```
Domain: string                    // domain
CreatedAt: DateTime               // created_at
LastTestedAt: DateTime            // last_tested_at
Cname: MandrillTrackingDomainCname // cname
ValidTracking: bool               // valid_tracking
```

**Request POCOs needed:**

| Request class                                   | Fields           |
| ----------------------------------------------- | ---------------- |
| `MandrillTrackingDomainRequest`                 | `Domain: string` |
| (list uses base `MandrillRequestBase` directly) | —                |

**Interface methods:**

```csharp
Task<IList<MandrillTrackingDomain>> TrackingDomainsAsync();
Task<MandrillTrackingDomain> AddTrackingDomainAsync(string domain);
Task<MandrillTrackingDomain> CheckTrackingDomainAsync(string domain);
Task<MandrillTrackingDomain> DeleteTrackingDomainAsync(string domain);
```

**Required changes to `MandrillApi.cs`:** Add `IMandrillUrlsApi Urls { get; }` property.

---

## Category 2: Partially Missing Operations

---

### 2.1 Messages — 2 missing of 13

#### `/messages/send-sms`

New SMS-specific models needed:

`MandrillSmsDetails`:

```
Text: string                      // text (required)
To: List<string>                  // to — E.164 phone numbers
From: string                      // from
Consent: MandrillSmsDetailsConsent                   // consent — "onetime"|"recurring"|"recurring-no-confirm"
TrackClicks: bool?                // track_clicks
```

Add enum MandrillSmsDetailsConsent with values `Onetime`, `Recurring`, `RecurringNoConfirm`. Use `[JsonConverter(typeof(JsonStringEnumConverter))]` to serialize as strings. and `[JsonPropertyName("onetime")]` etc to match the spec values.

`MandrillSmsMessage`:

```
// SMS-specific per-recipient result
// Spec schema: MessagesSmsMessage (referenced by MessagesSendSmsResponse)
// Need to read the full MessagesSmsMessage schema — spec shows channel, to, from, text, state fields
```

New method on `IMandrillMessagesApi`/`MandrillMessagesApi`:

```csharp
Task<IList<MandrillSmsMessage>> SendSmsAsync(MandrillSmsDetails sms,
    IList<MandrillRcptMergeVar>? mergeVars = null,
    IList<MandrillMergeVar>? globalMergeVars = null,
    bool? async = null);
```

New request POCO `MandrillSendSmsRequest : MandrillRequestBase` with nested `message` object containing `sms`, `merge`, `merge_language`, `global_merge_vars`, `merge_vars`, plus top-level `async`.

#### `/messages/send-mc-template`

New method on `IMandrillMessagesApi`/`MandrillMessagesApi`:

```csharp
Task<IList<MandrillSendMessageResponse>> SendMcTemplateAsync(MandrillMessage message,
    int mcTemplateId,
    MandrillMcTemplateVersion? mcTemplateVersion = null,
    bool async = false,
    string? ipPool = null,
    DateTime? sendAtUtc = null);
```

New request POCO `MandrillSendMcTemplateRequest : MandrillSendMessageRequest` with additional fields `McTemplateId: int` and `McTemplateVersion: MandrillMcTemplateVersion?`.

---

### 2.2 Rejects — 3 missing of 6

New SMS rejection POCOs:

`MandrillSmsReject` (used by add and delete responses):

```
Phone: string                     // phone
Added: bool?                      // added (set on add response)
Deleted: bool?                    // deleted (set on delete response)
Subaccount: string?               // subaccount
```

`MandrillSmsRejectInfo` (used by list response):

```
Phone: string                     // phone
CreatedAt: DateTime               // created_at
ExpiresAt: DateTime?              // expires_at
Expired: bool                     // expired
Subaccount: string?               // subaccount
```

New methods on `IMandrillRejectsApi`/`MandrillRejectsApi`:

```csharp
Task<MandrillSmsReject> AddSmsAsync(string phone, string? comment = null, string? subaccount = null);
Task<MandrillSmsReject> DeleteSmsAsync(string phone, string? subaccount = null);
Task<IList<MandrillSmsRejectInfo>> ListSmsAsync(string? phone = null, bool? includeExpired = null, string? subaccount = null);
```

New request POCO `MandrillSmsRejectRequest : MandrillRequestBase`:

```
Phone: string
Comment: string?
Subaccount: string?
IncludeExpired: bool?
```

---

### 2.3 Senders — 1 missing of 8

#### `/senders/delete-domain`

Response uses same type as other sender domain operations (`MandrillSenderDomain`).

New method on `IMandrillSendersApi`/`MandrillSendersApi`:

```csharp
Task<MandrillSenderDomain> DeleteDomainAsync(string domain);
```

Implementation: POST to `"senders/delete-domain.json"` with `new MandrillSenderRequest { Domain = domain }`.

---

### 2.4 Exports — 1 missing of 6

#### `/exports/allowlist`

The spec has both `/exports/allowlist` (new) and `/exports/whitelist` (legacy). The existing `WhitelistAsync` covers the legacy endpoint. The new allowlist endpoint is missing.

New method on `IMandrillExportsApi`/`MandrillExportsApi`:

```csharp
Task<MandrillExportInfo> AllowlistAsync(string? notifyEmail = null);
```

Implementation: POST to `"exports/allowlist.json"` with `new MandrillExportRequest { NotifyEmail = notifyEmail }`.

---

### 2.5 Users — 1 missing of 4

#### `/users/ping2`

Returns `{ "PING": "PONG!" }` as a JSON object (strict JSON, contrasted with `/users/ping` which returns the bare string `"PONG!"`).

New POCO `MandrillPing2Response`:

```
Ping: string                      // PING — always "PONG!"
```

With `[JsonPropertyName("PING")]`.

New method on `IMandrillUsersApi`/`MandrillUsersApi`:

```csharp
Task<MandrillPing2Response> Ping2Async();
```

---

## Category 3: Request/Response Data Structure Discrepancies

---

### 3.1 `MandrillAllowlistInfo` — merged response type

**Issue:** The current `MandrillAllowlistInfo` class is used for all three allowlist responses (add, delete, list). It combines fields from all three:

- `Added: bool` and `Deleted: bool` from add/delete responses
- `Email`, `Detail`, `CreatedAt` from list response items

**Spec says:** Three distinct shapes:

- `AllowlistsAddResponse`: `email`, `added` only
- `AllowlistsDeleteResponse`: `email`, `deleted` only
- `AllowlistsListResponse` items: `email`, `detail`, `created_at` only

**Recommended fix:** The current approach works functionally (extra properties are ignored on deserialization), but the method signatures return misleading types. Consider splitting into separate response types:

- `MandrillAllowlistAddResponse` (`Email`, `Added`)
- `MandrillAllowlistDeleteResponse` (`Email`, `Deleted`)
- Keep `MandrillAllowlistInfo` for list items (`Email`, `Detail`, `CreatedAt`)

---

### 3.2 `MandrillSenderDomain` — extra `Verified` property + DMARC type mismatch

**Issue 1:** `MandrillSenderDomain` has a `Verified: MandrillSenderVerifyDomain` property that is **not** in the spec's `SendersSenderDomain` schema.

**Issue 2:** `MandrillSenderDomain.DMARC` is typed as `MandrillSenderValidDetails` (which has `Valid`, `ValidAfter`, `Error`). The spec's `DmarcDetail` only has `valid` + `error` (no `valid_after`). The existing `MandrillSenderValidDetails` shares JSON property names with `VerificationDetail` (`valid`, `valid_after`, `error`), so SPF/DKIM/DKIM2 are fine — but DMARC would deserialize `valid_after` as null (benign).

**Recommended fix:** Low priority — extra properties are silently ignored. Document that `Verified` is not in the current spec response.

---

### 3.3 `MandrillSenderDemographics` — used as `MandrillRejectInfo.Sender`

**Issue:** `MandrillRejectInfo.Sender` is typed as `MandrillSenderDemographics` (properties: `Address`, `CreatedAt`). The spec inline-defines the `sender` object in `RejectsListResponse` items without a named schema. Need to verify the actual property names align. The spec shows `sender: object|null` with no explicit shape documented in the summary — the existing mapping is likely correct, but should be verified against live API.

---

### 3.4 `ParseAsync` returns `MandrillMessage` instead of a dedicated parse-response type

**Issue:** `MandrillMessagesApi.ParseAsync` returns `Task<MandrillMessage>`. The spec's `MessagesParseResponse` has fields specific to a parsed incoming message: `from_email`, `from_name`, `headers`, `html`, `subject`, `text`, `text_flowed`, `to` (array), `attachments`. `MandrillMessage` is the outbound send-request model and includes unrelated send-specific fields.

**Recommended fix:** Add a dedicated `MandrillParsedMessage` response POCO with only the parse-response fields. This avoids confusion between inbound parsed content and outbound send parameters.

---

### 3.5 `MandrillWebHookEventType` — `Blacklist`/`Whitelist` enum values

**Issue:** The C# enum includes `Blacklist` and `Whitelist` values. With Mandrill renaming to allowlist/denylist terminology, verify these match the current webhook event type strings in the spec.

**Spec path to check:** Grep `WebhooksAddRequest` → `events` enum values in `spec/mandrill-openapi.json` around line 10741.

---

## Summary Table

| Group            | Spec ops | Implemented | Missing                             |
| ---------------- | -------- | ----------- | ----------------------------------- |
| Allowlist        | 3        | 3           | —                                   |
| Exports          | 6        | 5           | `allowlist`                         |
| **IPs**          | **13**   | **0**       | **all 13**                          |
| Inbound          | 9        | 9           | —                                   |
| **MC Templates** | **4**    | **0**       | **all 4**                           |
| Messages         | 13       | 11          | `send-sms`, `send-mc-template`      |
| **Metadata**     | **4**    | **0**       | **all 4**                           |
| Rejects          | 6        | 3           | `add-sms`, `delete-sms`, `list-sms` |
| Senders          | 8        | 7           | `delete-domain`                     |
| Subaccounts      | 7        | 7           | —                                   |
| Tags             | 5        | 5           | —                                   |
| Templates        | 8        | 8           | —                                   |
| **URLs**         | **4**    | **0**       | **all 4**                           |
| Users            | 4        | 3           | `ping2`                             |
| WebHooks         | 5        | 5           | —                                   |
| **Total**        | **99**   | **66**      | **33**                              |

> Note: The count above shows 66 implemented (not 75) because the summary reflects unique operation endpoints, not counting the `WhitelistAsync`→`/exports/whitelist` endpoint separately from `AllowlistAsync`.

---

## Verification

After implementing any group:

1. Run `dotnet test` from the repo root.
2. For new API groups, add integration-style tests following the pattern in the existing `spec/` test files (if present) or unit tests in the `Tests` project.
3. Verify JSON field names by comparing against the OpenAPI `components/schemas` section in `spec/mandrill-openapi.json` — pay attention to snake_case JSON names vs PascalCase C# properties.
4. Confirm date/time serialization: most datetime fields in responses are ISO strings (`IsoDateTimeConverter`) not Unix timestamps (`UnixDateTimeConverter`). The `Ts` fields in message events use Unix timestamps.
