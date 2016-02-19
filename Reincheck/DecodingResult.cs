using System.Collections.Generic;

namespace Reincheck
{
    public class DecodingResult
    {
        public string Token { get; }
        public IDictionary<string, string> Properties { get; }
        public bool IsValid { get; }

        public DecodingResult(string token, IDictionary<string, string> properties, bool isValid)
        {
            Token = token;
            Properties = properties;
            IsValid = isValid;
        }
    }
}