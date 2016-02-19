namespace Reincheck
{
    public class DecodingResultDetails
    {
        public DecodingResultDetails(bool hasValidSignature, bool isExpired)
        {
            HasValidSignature = hasValidSignature;
            IsExpired = isExpired;
        }

        public bool HasValidSignature { get; }
        public bool IsExpired { get; }
    }
}