using System.Collections.Generic;
using System.Management.Automation;

namespace ProjectSonicWave.Models
{
    public sealed class CmdletResult
    {
        public IReadOnlyList<PSObject> Output { get; }
        public IReadOnlyList<PsErrorInfo> Errors { get; }

        public bool HasErrors => Errors.Count > 0;
        public CmdletResult(IReadOnlyList<PSObject> output, IReadOnlyList<PsErrorInfo> errors)
        {
            Output = output;
            Errors = errors;
        }
    }
}