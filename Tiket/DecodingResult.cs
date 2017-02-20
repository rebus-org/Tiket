using System.Collections.Generic;
using System.Security;

namespace Tiket
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
        public bool IsValid => Details.IsCorrectFormat
                               && Details.HasValidSignature
                               && !Details.IsNotValidYet
                               && !Details.IsExpired;

        /// <summary>
        /// Checks that the result is valid. If it is not valid, a <see cref="SecurityException"/> is thrown with some hints on why the key was not valid.
        /// </summary>
        public void ThrowIfInvalid()
        {
            if (IsValid) return;

            throw new SecurityException($"The key is not valid! (has valid signature: {Details.HasValidSignature}, is not valid yet: {Details.IsNotValidYet}, is expired: {Details.IsExpired})");
        }
    }
}