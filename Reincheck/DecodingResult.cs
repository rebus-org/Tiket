using System.Collections.Generic;

namespace Reincheck
{
    /// <summary>
    /// Gets the result of decoding a token
    /// </summary>
    public class DecodingResult
    {
        internal DecodingResult(IDictionary<string, string> properties, DecodingResultDetails details)
        {
            Properties = properties;
            Details = details;
        }

        /// <summary>
        /// Gets the decoded claims found in the token
        /// </summary>
        public IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Gets details about which aspects of the token could/coult not be validated
        /// </summary>
        public DecodingResultDetails Details { get; }

        /// <summary>
        /// Gets whether the token is valid. Composed of multiple checks of the details from <see cref="Details"/>.
        /// </summary>
        public bool IsValid => Details.HasValidSignature
                               && !Details.IsNotValidYet
                               && !Details.IsExpired;
    }
}