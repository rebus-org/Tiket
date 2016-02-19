using System.Collections.Generic;

namespace Reincheck
{
    public class DecodingResult
    {
        public IDictionary<string, string> Properties { get; }

        public DecodingResultDetails Details { get; }

        public bool IsValid => Details.HasValidSignature
                               && !Details.IsExpired;

        public DecodingResult(IDictionary<string, string> properties, DecodingResultDetails details)
        {
            Properties = properties;
            Details = details;
        }
    }
}