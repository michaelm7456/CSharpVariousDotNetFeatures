namespace RateLimiting.Configuration.Keys
{
    public class RateLimitingPolicies
    {
        public const string RateLimitingPoliciesSection = "RateLimitingPolicies";

        public const string GenericConcurrencyLimiter = "GenericConcurrencyLimiter";

        public const string GenericConcurrencyPermitLimit = "PermitLimit";

        public const string GenericConcurrencyQueueLimit = "QueueLimit";
    }
}
