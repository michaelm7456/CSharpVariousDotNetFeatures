namespace UnitTestingAndIntegrationTesting.UnitTests.Stack
{
    public class StackTests
    {
        public class EmptyStack
        {
            readonly Stack<int> stack;

            public EmptyStack()
            {
                stack = new Stack<int>();
            }

            [Fact]
            public void Count_ShouldReturnZero()
            {
                int count = stack.Count;

                Assert.Equal(0, count);
            }

            [Fact]
            public void Contains_ShouldReturnFalse()
            {
                bool contains = stack.Contains(10);

                Assert.False(contains);
            }

            [Fact]
            public void Pop_ShouldThrowInvalidOperationException()
            {
                Exception exception = Record.Exception(() => stack.Pop());

                Assert.IsType<InvalidOperationException>(exception);
            }

            [Fact]
            public void Peek_ShouldThrowInvalidOperationException()
            {
                Exception exception = Record.Exception(() => stack.Peek());

                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class StackWithOneElement
        {
            readonly Stack<int> stack;
            const int pushedValue = 42;

            public StackWithOneElement()
            {
                stack = new Stack<int>();
                stack.Push(pushedValue);
            }

            [Fact]
            public void Count_ShouldBeOne()
            {
                int count = stack.Count;

                Assert.Equal(1, count);
            }

            [Fact]
            public void Pop_CountShouldBeZero()
            {
                stack.Pop();

                int count = stack.Count;

                Assert.Equal(0, count);
            }

            [Fact]
            public void Peek_CountShouldBeOne()
            {
                stack.Peek();

                int count = stack.Count;

                Assert.Equal(1, count);
            }

            [Fact]
            public void Pop_ShouldReturnPushedValue()
            {
                int actual = stack.Pop();

                Assert.Equal(pushedValue, actual);
            }

            [Fact]
            public void Peek_ShouldReturnPushedValue()
            {
                int actual = stack.Peek();

                Assert.Equal(pushedValue, actual);
            }
        }

        public class StackWithMultipleValues
        {
            readonly Stack<int> stack;
            const int firstPushedValue = 10;
            const int secondPushedValue = 20;
            const int thirdPushedValue = 30;

            public StackWithMultipleValues()
            {
                stack = new Stack<int>();
                stack.Push(firstPushedValue);
                stack.Push(secondPushedValue);
                stack.Push(thirdPushedValue);
            }

            [Fact]
            public void Count_ShouldBeThree()
            {
                int count = stack.Count;

                Assert.Equal(3, count);
            }

            [Fact]
            public void Pop_VerifyLifoOrder()
            {
                Assert.Equal(thirdPushedValue, stack.Pop());
                Assert.Equal(secondPushedValue, stack.Pop());
                Assert.Equal(firstPushedValue, stack.Pop());
            }

            [Fact]
            public void Peek_ReturnsLastPushedValue()
            {
                int actual = stack.Peek();

                Assert.Equal(thirdPushedValue, actual);
            }

            [Fact]
            public void Contains_ReturnsTrue()
            {
                bool contains = stack.Contains(secondPushedValue);

                Assert.True(contains);
            }
        }

        public class StackWithStrings
        {
            [Fact]
            public void Pop_ShouldReturnPushedValue()
            {
                Stack<string> stack = new();
                stack.Push("Help");

                string actual = stack.Pop();

                Assert.Equal("Help", actual);
            }
        }
    }
}