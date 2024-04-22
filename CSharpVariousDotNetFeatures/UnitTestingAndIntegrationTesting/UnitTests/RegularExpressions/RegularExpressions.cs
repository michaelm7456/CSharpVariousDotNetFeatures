using System.Text.RegularExpressions;

namespace UnitTestingAndIntegrationTesting.UnitTests.RegularExpressions
{
    public class RegularExpressions
    {
        //https://www.regular-expressions.info/refcapture.html
        //https://regexlib.com/(X(1)A(6KJlahbgIjgl7oWEmu1FbdX_3ydBJS1Gxp-y65T1_zW8c6Z9vEkKoClcQYXSUqg045bOppQZq33E_Jj3OWo0ph5h-AI2-fOaqSmOpVKoVxusVPZc122YzwgNPN9Dp0gL_ytwEQL5kA2tUN1ky57ktv-SUYyAtHMMLLJ-ZpYgTiIlExaZd2vs-_vFdEYX1A1X0))/CheatSheet.aspx?AspxAutoDetectCookieSupport=1

        // ^	    Start of a string.
        // $	    End of a string.
        // .	    Any character(except \n newline)
        // |	    Alternation.
        // {...}    Explicit quantifier notation.
        // [...]    Explicit range of characters to match.
        // (...)    Logical grouping of part of an expression.
        // *	    0 or more of previous expression.
        // +	    1 or more of previous expression.
        // ?	    0 or 1 of previous expression; also forces minimal matching when an expression might match several strings within a search string.
        // \	    Preceding one of the above, it makes it a literal instead of a special character.Preceding a special matching character, see below.

        public class LiteralCharacterMatching
        {
            [Theory]
            [InlineData("abc")]
            [InlineData("abcd")]
            [InlineData("aabcd")]
            [InlineData("abcabc")]
            public void LiteralCharacters_ReturnsMatch(string input)
            {
                // Note: Simple to start off, pattern appears explicitly within each test data record.
                string pattern = "abc";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }

        public class QuantifierMatching
        {
            [Theory]
            [InlineData("ac")]
            [InlineData("abc")]
            [InlineData("abbc")]
            public void PreviousElementHasZeroOrMoreMatches_ReturnsMatch(string input)
            {
                // Note: The '*' character means preceding character of 'b' is optional, but 'a' and 'c' are mandatory.
                string pattern = "ab*c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abc")]
            [InlineData("abbc")]
            [InlineData("abbbc")]
            public void PreviousElementHasOneOrMoreOccurrences_ReturnsMatch(string input)
            {
                // Note: The '+' character means preceding character of 'b' is mandatory and must appear at least once, and 'b' can have multiple occurrences, while 'a' and 'c' are singular.
                string pattern = "ab+c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("ac")]
            [InlineData("abc")]
            public void PreviousElementHasZeroOrOneOccurrences_ReturnsMatch(string input)
            {
                // Note: The '?' character means preceding character of 'b' is optional and 'a' and 'c' are mandatory, however 'b' is only permitted once at most. (Unlike '+' operator which allows multiple occurrences.)
                string pattern = "ab?c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abbc")]
            public void PreviousElementHasXOccurrences_ReturnsMatch(string input)
            {
                // Note: '{2}' means preceding character must appear 2 times, no more no less.
                string pattern = "ab{2}c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abbc")]
            [InlineData("abbbc")]
            [InlineData("abbbbc")]
            public void PreviousElementHasAtLeastXOccurrences_ReturnsMatch(string input)
            {
                // Note: '{2,}' means preceding character must appear at least 2 times, no limit specified.
                string pattern = "ab{2,}c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abbc")]
            [InlineData("abbbc")]
            [InlineData("abbbbc")]
            public void PreviousElementHasAtLeastXOccurrencesAndAtMostYOccurrences_ReturnsMatch(string input)
            {
                // Note: '{2,4}' means preceding character must appear at least 2 times, and at most 4 times.
                string pattern = "ab{2,4}c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }

        public class AnchorMatching
        {
            [Theory]
            [InlineData("abc")]
            [InlineData("abcdef")]
            [InlineData("abc123")]
            public void StringBeginsWithTestData_ReturnsMatch(string input)
            {
                // Note: '^' means string must start with the characters 'abc'.
                string pattern = "^abc";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abc")]
            [InlineData("defabc")]
            [InlineData("123abc")]
            public void StringEndsWithTestData_ReturnsMatch(string input)
            {
                // Note: '$' means string must end with the characters 'abc'.
                string pattern = "abc$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }

        public class CharacterMatching
        {
            [Theory]
            [InlineData("abc")]
            [InlineData("aBc")]
            public void AnySingleCharacterFoundWithinBrackets_ReturnsMatch(string input)
            {
                // Note '[]' means that any character within these brackets must appear in the string.
                string pattern = "a[bB]c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("aac")]
            [InlineData("acc")]
            [InlineData("adc")]
            public void AnySingleCharacterNotFoundWithinBrackets_ReturnsMatch(string input)
            {
                // Note '[^ ]' means that any character within these brackets must not appear in the string.
                string pattern = "a[^bB]c";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("a")]
            [InlineData("abc")]
            [InlineData("abcdef")]
            public void StringContainsLowerCaseLettersOnly_ReturnsMatch(string input)
            {
                // Note: We're only matching on lower case letters for this example, hence a-z and square brackets.
                // '^' and '$' to denote start and end respectively.
                // '+' to indicate one or more letters.
                string pattern = "^[a-z]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("A")]
            [InlineData("ABC")]
            [InlineData("ABCDEF")]
            public void StringContainsUpperCaseLettersOnly_ReturnsMatch(string input)
            {
                // Note: We're only matching on upper case letters for this example, hence A-Z and square brackets.
                // '^' and '$' to denote start and end respectively.
                // '+' to indicate one or more letters.
                string pattern = "^[A-Z]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("123")]
            [InlineData("0123456789")]
            [InlineData("0022446688")]
            public void StringContainsNonUnicodeNumbers_ReturnsMatch(string input)
            {
                // Note: We're only matching on numbers for this example, hence 0-9 and square brackets.
                // '^' and '$' to denote start and end respectively.
                // '+' to indicate one or more numbers.
                // Non-Unicode variant for simple number matching
                string pattern = "^[0-9]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("Mil")]
            [InlineData("M_l")]
            public void StringIsThreeCharactersWithGivenStartAndEndCharacter_ReturnsMatch(string input)
            {
                // ^M.l$ indicates string starts with M, ends with l, and has any character inbetween due to '.' character.
                string pattern = "^M.l$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("Mail")]
            [InlineData("Mill")]
            public void StringIsFourCharactersWithGivenStartAndEndCharacter_ReturnsMatch(string input)
            {
                // ^M.l$ indicates string starts with M, ends with l, and has any 2 characters inbetween due to '.' character.
                string pattern = "^M..l$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("Hello")]
            [InlineData("Bonjourno")]

            public void StringIsASetSize_ReturnsMatch(string input)
            {
                string pattern = @"^.{5,10}$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("123")]
            [InlineData("0123456789")]
            [InlineData("٠١٢٣٤٥٦٧٨٩")]
            public void StringContainsUnicodeNumbers_ReturnsMatch(string input)
            {
                // Note: We're matching on Unicode, hence \d and square brackets.
                // '^' and '$' to denote start and end respectively.
                // '+' to indicate one or more numbers.
                // Unicode variant for more complex number matching, using East Arabic numerals as per above test record.
                string pattern = "^[\\d]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("abc")]
            [InlineData("ABC")]
            [InlineData("abc_ABC")]
            [InlineData("abc_123")]
            [InlineData("123")]
            [InlineData("0123456789")]
            [InlineData("٠١٢٣٤٥٦٧٨٩")]
            public void StringContainsWords_ReturnsMatch(string input)
            {
                // Note: We're matching on Words, hence \w and square brackets.
                // Words being an alphanumeric character plus an underscore.
                // Supports Unicode
                string pattern = "^[\\w]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("-")]
            [InlineData("+")]
            [InlineData("*")]
            [InlineData("/")]
            [InlineData("'")]
            [InlineData("\"")]
            [InlineData("\\")]
            public void StringContainsNoWords_ReturnsMatch(string input)
            {
                // Note: We're matching on Nonwords, hence \W and square brackets.
                // Words being an alphanumeric character plus an underscore.
                string pattern = "^[\\W]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData(" ")]
            [InlineData("  ")]
            [InlineData("   ")]
            public void StringContainsWhitespaceOnly_ReturnsMatch(string input)
            {
                // Note: We're matching on white space, hence \s and square brackets.
                string pattern = "^[\\s]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }

            [Theory]
            [InlineData("a")]
            [InlineData(" ")]
            [InlineData("a 10")]
            public void StringContainsWordsAndSpaces_ReturnsMatch(string input)
            {
                // Note: We're matching on both lower and upper case letters, numbers and spaces
                // '+' to indicate one or more occurrences of the characters specified within the brackets.
                string pattern = "^[a-zA-Z0-9\\s]+$";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }

        public class GroupingAndCapturing
        {
            [Theory]
            [InlineData("abcabc")]
            [InlineData("abcabcabc")]
            [InlineData("abcabcabcabc")]
            public void GroupingOccursWithinString_ReturnsMatch(string input)
            {
                // Note: '(abc){2}' means preceding grouping must appear at least 2 times.
                string pattern = "(abc){2}";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }

        public class Alternation
        {
            [Theory]
            [InlineData("abc")]
            [InlineData("def")]
            [InlineData("abcdef")]
            [InlineData("abc123")]
            [InlineData("def456")]
            public void StringContainsOneOfMultipleRegularExpressions_ReturnsMatch(string input)
            {
                // Note: '|' works as an 'OR' statement, if an expression on either side of the pipe is found within the string, a match is returned.
                string pattern = "abc|def";

                bool actual = Regex.IsMatch(input, pattern);

                Assert.True(actual);
            }
        }
    }
}