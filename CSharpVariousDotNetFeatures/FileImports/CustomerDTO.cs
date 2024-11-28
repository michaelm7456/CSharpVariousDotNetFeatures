using System.Text.Json.Serialization;

namespace FileImports
{
    public class CustomerDTO
    {
        [JsonPropertyName("id")]
        public required long Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("age")]
        public required int Age { get; set; }

        [JsonPropertyName("address")]
        public required AddressDTO Address { get; set; }

        [JsonPropertyName("is_active")]
        public required bool IsActive { get; set; }

        [JsonPropertyName("signup_date")]
        public required DateTime SignupDate { get; set; }

        [JsonPropertyName("last_login")]
        public DateTime? LastLogin { get; set; }

        [JsonPropertyName("subscription_level")]
        public string? SubscriptionLevel { get; set; }

        [JsonPropertyName("preferred_language")]
        public string? PreferredLanguage { get; set; }

        [JsonPropertyName("contact_numbers")]
        public required List<ContactNumberDTO> ContactNumbers { get; set; }
    }

    public class AddressDTO
    {
        [JsonPropertyName("street")]
        public required string Street { get; set; }

        [JsonPropertyName("city")]
        public required string City { get; set; }

        [JsonPropertyName("postal_code")]
        public required string PostalCode { get; set; }
    }

    public class ContactNumberDTO
    {
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("number")]
        public required string Number { get; set; }
    }
}
