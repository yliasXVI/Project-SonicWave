using System;

using System.Collections.Generic;

using System.Linq;

using System.Management.Automation;

using System.Management.Automation.Runspaces;

using System.Threading;

using System.Threading.Tasks;

using ProjectSonicWave.Models;

namespace ProjectSonicWave.Services

{

    /// <summary>

    /// Centrale PowerShell-service (read-only). Whitelist enkel Get-/Connect-/Disconnect-cmdlets.

    /// </summary>

    public sealed class PowerShellService : IDisposable

    {

        private readonly Runspace _runspace;

        private readonly HashSet<string> _allowedCmdlets;

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private readonly ILogService _logger;

        public PowerShellService(ILogService logger)

        {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var iss = InitialSessionState.CreateDefault2();

            _runspace = RunspaceFactory.CreateRunspace(iss);

            _runspace.Open();

            _allowedCmdlets = new HashSet<string>(StringComparer.OrdinalIgnoreCase)

            {

                // Exchange Online

                "Connect-ExchangeOnline", "Disconnect-ExchangeOnline",

                "Get-Mailbox", "Get-MailboxStatistics", "Get-MailboxPermission",

                "Get-Recipient", "Get-DistributionGroup", "Get-DistributionGroupMember",

                // Graph / Entra

                "Connect-MgGraph", "Disconnect-MgGraph",

                "Get-MgUser", "Get-MgGroup", "Get-MgGroupMember",

                "Get-MgSubscribedSku", "Get-MgUserLicenseDetail",

                // Teams (optioneel)

                "Connect-MicrosoftTeams", "Disconnect-MicrosoftTeams",

                "Get-Team", "Get-TeamUser"

            };

        }

        public Task<CmdletResult> ConnectExchangeOnlineAsync(CancellationToken ct = default)

            => RunGetCommandAsync("Connect-ExchangeOnline", null, ct);

        public Task<CmdletResult> ConnectGraphAsync(CancellationToken ct = default)

            => RunGetCommandAsync("Connect-MgGraph", null, ct);

        public async Task<CmdletResult> RunGetCommandAsync(

            string command,

            IDictionary<string, object?>? parameters = null,

            CancellationToken ct = default)

        {

            if (!_allowedCmdlets.Contains(command))

                throw new InvalidOperationException($"Cmdlet '{command}' is niet toegestaan in read-only modus.");

            await _semaphore.WaitAsync(ct).ConfigureAwait(false);

            try

            {

                _logger.Log(LogLevel.Info, "PowerShellService", $"Start: {command}{FormatParams(parameters)}");

                using var ps = PowerShell.Create();

                ps.Runspace = _runspace;

                ps.AddCommand(command);

                if (parameters != null)

                {

                    foreach (var kv in parameters)

                        ps.AddParameter(kv.Key, kv.Value);

                }

                var output = new List<PSObject>();

                var errors = new List<PsErrorInfo>();

                await Task.Run(() =>

                {

                    var results = ps.Invoke();

                    output.AddRange(results);

                    if (ps.HadErrors)

                    {

                        foreach (var err in ps.Streams.Error)

                        {

                            errors.Add(new PsErrorInfo(

                                message: err?.ToString() ?? "Onbekende PowerShell-fout",

                                category: err?.CategoryInfo?.ToString(),

                                stackTrace: err?.ScriptStackTrace

                            ));

                        }

                    }

                }, ct).ConfigureAwait(false);

                if (errors.Count > 0)

                {

                    _logger.Log(LogLevel.Error, "PowerShellService",

                        $"Cmdlet '{command}' faalde",

                        string.Join("\n", errors.Select(e => e.Message)));

                    throw new PowerShellExecutionException($"Cmdlet '{command}' faalde.", errors);

                }

                _logger.Log(LogLevel.Info, "PowerShellService", $"Gereed: {command} ({output.Count} items)");

                return new CmdletResult(output, errors);

            }

            finally

            {

                _semaphore.Release();

            }

        }

        public async Task<IReadOnlyList<MailboxDto>> GetMailboxesTypedAsync(CancellationToken ct = default)

        {

            var result = await RunGetCommandAsync("Get-Mailbox", null, ct).ConfigureAwait(false);

            var list = new List<MailboxDto>();

            foreach (var o in result.Output)

            {

                list.Add(new MailboxDto

                {

                    DisplayName = o.Properties["DisplayName"]?.Value?.ToString(),

                    PrimarySmtpAddress = o.Properties["PrimarySmtpAddress"]?.Value?.ToString(),

                    RecipientTypeDetails = o.Properties["RecipientTypeDetails"]?.Value?.ToString(),

                    Database = o.Properties["Database"]?.Value?.ToString()

                });

            }

            return list;

        }

        public async Task<IReadOnlyList<GroupDto>> GetGroupsAsync(CancellationToken ct = default)

        {

            var result = await RunGetCommandAsync("Get-DistributionGroup", null, ct).ConfigureAwait(false);

            var list = new List<GroupDto>();

            foreach (var o in result.Output)

            {

                list.Add(new GroupDto

                {

                    DisplayName = o.Properties["DisplayName"]?.Value?.ToString() ?? string.Empty,

                    PrimarySmtpAddress = o.Properties["PrimarySmtpAddress"]?.Value?.ToString() ?? string.Empty,

                    Identity = o.Properties["Identity"]?.Value?.ToString() ?? string.Empty,

                    GroupType = o.Properties["GroupType"]?.Value?.ToString() ?? string.Empty

                });

            }

            return list;

        }

        public async Task<IReadOnlyList<GroupMemberDto>> GetGroupMembersAsync(string identity, CancellationToken ct = default)

        {

            var parameters = new Dictionary<string, object?> { { "Identity", identity } };

            var result = await RunGetCommandAsync("Get-DistributionGroupMember", parameters, ct).ConfigureAwait(false);

            var list = new List<GroupMemberDto>();

            foreach (var o in result.Output)

            {

                list.Add(new GroupMemberDto

                {

                    DisplayName = o.Properties["DisplayName"]?.Value?.ToString() ?? string.Empty,

                    PrimarySmtpAddress = o.Properties["PrimarySmtpAddress"]?.Value?.ToString() ?? string.Empty,

                    RecipientType = o.Properties["RecipientType"]?.Value?.ToString() ?? string.Empty

                });

            }

            return list;

        }

        public async Task<IReadOnlyList<UserDto>> GetUsersAsync(string filter, CancellationToken ct = default)

        {

            var parameters = string.IsNullOrEmpty(filter) ? null : new Dictionary<string, object?> { { "Filter", filter } };

            var result = await RunGetCommandAsync("Get-MgUser", parameters, ct).ConfigureAwait(false);

            var list = new List<UserDto>();

            foreach (var o in result.Output)

            {

                list.Add(new UserDto

                {

                    DisplayName = o.Properties["DisplayName"]?.Value?.ToString() ?? string.Empty,

                    UserPrincipalName = o.Properties["UserPrincipalName"]?.Value?.ToString() ?? string.Empty,

                    Mail = o.Properties["Mail"]?.Value?.ToString() ?? string.Empty,

                    JobTitle = o.Properties["JobTitle"]?.Value?.ToString() ?? string.Empty,

                    Department = o.Properties["Department"]?.Value?.ToString() ?? string.Empty,

                    Licenses = string.Empty // Would need Get-MgUserLicenseDetail for actual data

                });

            }

            return list;

        }

        public void Dispose()

        {

            _runspace.Dispose();

            _semaphore.Dispose();

        }

        private static string FormatParams(IDictionary<string, object?>? parameters)

        {

            if (parameters == null || parameters.Count == 0) return "";

            return " " + string.Join(" ", parameters.Select(kv => $"-{kv.Key} \"{kv.Value}\""));

        }

    }

    public sealed class PowerShellExecutionException : Exception

    {

        public IReadOnlyList<PsErrorInfo> Errors { get; }

        public PowerShellExecutionException(string message, IReadOnlyList<PsErrorInfo> errors)

            : base(message)

        {

            Errors = errors;

        }

    }

}
 
