using System.Text.RegularExpressions;

namespace UnitTestingAndIntegrationTesting.UnitTests.PhoneNumbers
{
    public class PhoneNumberValidation
    {
        //https://areaphonecodes.com/

        const string strictUKNumberPattern = "^(?:0|\\+?44)(?:\\d\\s?){9,15}$";
        const string relaxedUSNumberPattern = "\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}";

        [Theory]
        [InlineData("02012345678")]
        [InlineData("442012345678")]
        [InlineData("+442012345678")]
        public void UKLandlineNumber_ReturnsMatch(string input)
        {
            // Note: Testing variants of landline in UK.
            // '+44' is country code for UK.
            string pattern = strictUKNumberPattern;

            bool actual = Regex.IsMatch(input, pattern);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("07700123456")]
        [InlineData("447700123456")]
        [InlineData("+447700123456")]
        [InlineData("07700654321")]
        [InlineData("447700654321")]
        [InlineData("+447700654321")]
        public void UKMobileNumber_ReturnsMatch(string input)
        {
            // Note: Testing variants of mobile in UK.
            // '7' is code for UK mobiles.
            string pattern = strictUKNumberPattern;

            bool actual = Regex.IsMatch(input, pattern);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("08451234567")]
        [InlineData("448451234567")]
        [InlineData("+448451234567")]
        [InlineData("08751234567")]
        [InlineData("448751234567")]
        [InlineData("+448751234567")]
        [InlineData("09851234567")]
        [InlineData("449851234567")]
        [InlineData("+449851234567")]
        public void UKPremiumLineNumber_ReturnsMatch(string input)
        {
            // Note: Testing variants of premium lines in UK.
            // '84', '87' and '98' are codes for UK premium lines.
            string pattern = strictUKNumberPattern;

            bool actual = Regex.IsMatch(input, pattern);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("2019876543")]
        [InlineData("(201)9876543")]
        [InlineData("(201)987-6543")]
        [InlineData("(201)-987-6543")]
        [InlineData("201-987-6543")]
        [InlineData("12019876543")]
        [InlineData("1(201)9876543")]
        [InlineData("1(201)987-6543")]
        [InlineData("1(201)-987-6543")]
        [InlineData("1201-987-6543")]
        [InlineData("+12019876543")]
        [InlineData("+1(201)9876543")]
        [InlineData("+1(201)987-6543")]
        [InlineData("+1(201)-987-6543")]
        [InlineData("+1201-987-6543")]
        public void USLandlinePhoneNumber_ReturnsMatch(string input)
        {
            // Note: Testing variants of landline in US.
            // '+1' is country code for US.
            string pattern = relaxedUSNumberPattern;

            bool actual = Regex.IsMatch(input, pattern);

            Assert.True(actual);
        }

        [Theory]
        [InlineData("9005551234")]
        [InlineData("(900)5551234")]
        [InlineData("(900)555-1234")]
        [InlineData("(900)-555-1234")]
        [InlineData("900-555-1234")]
        [InlineData("19005551234")]
        [InlineData("1(900)5551234")]
        [InlineData("1(900)555-1234")]
        [InlineData("1(900)-555-1234")]
        [InlineData("1900-555-1234")]
        [InlineData("1-900-555-1234")]
        [InlineData("+19005551234")]
        [InlineData("+1(900)5551234")]
        [InlineData("+1(900)555-1234")]
        [InlineData("+1(900)-555-1234")]
        [InlineData("+1900-555-1234")]
        [InlineData("+1-900-555-1234")]
        public void USPremiumLinePhoneNumber_ReturnsMatch(string input)
        {
            // Note: Testing variants of premium lines in US.
            // '(900)' is code for US premium lines
            string pattern = relaxedUSNumberPattern;

            bool actual = Regex.IsMatch(input, pattern);

            Assert.True(actual);
        }
    }
}
