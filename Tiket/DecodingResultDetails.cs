namespace Tiket
{
    /// <summary>
    /// Gives details about the reason why a token might have been deemed to be invalid
    /// </summary>
    public class DecodingResultDetails
    {
        internal DecodingResultDetails(bool hasValidSignature, bool isExpired, bool isNotValidYet)
        {
            HasValidSignature = hasValidSignature;
            IsExpired = isExpired;
            IsNotValidYet = isNotValidYet;
        }

        /// <summary>
        /// Gets whether the token was properly signed and can be considered authentic. 
        /// IF THIS IS FALSE THE TOKEN MUST NOT BE TRUSTED AND SHOULD BE REJECTED!!
        /// </summary>
        public bool HasValidSignature { get; }

        /// <summary>
        /// Gets whether the token has expired
        /// </summary>
        public bool IsExpired { get; }

        /// <summary>
        /// Gets whether the token has not yet become valid
        /// </summary>
        public bool IsNotValidYet { get; }
    }
}