namespace UnitTestingAndIntegrationTesting.UnitTests.Queue
{
    public class QueueTests
    {
        public class EmptyQueue
        {
            readonly Queue<int> queue;

            public EmptyQueue()
            {
                queue = new Queue<int>();
            }

            [Fact]
            public void Count_ShouldReturnZero()
            {
                int count = queue.Count;

                Assert.Equal(0, count);
            }

            [Fact]
            public void Contains_ShouldReturnFalse()
            {
                bool contains = queue.Contains(10);

                Assert.False(contains);
            }

            [Fact]
            public void Dequeue_ShouldThrowInvalidOperationException()
            {
                Exception exception = Record.Exception(() => queue.Dequeue());

                Assert.IsType<InvalidOperationException>(exception);
            }

            [Fact]
            public void Peek_ShouldThrowInvalidOperationException()
            {
                Exception exception = Record.Exception(() => queue.Peek());

                Assert.IsType<InvalidOperationException>(exception);
            }
        }

        public class QueueWithOneElement
        {
            readonly Queue<int> queue;
            const int queuedValue = 10;

            public QueueWithOneElement()
            {
                queue = new Queue<int>();
                queue.Enqueue(queuedValue);
            }

            [Fact]
            public void Count_ShouldBeOne()
            {
                int count = queue.Count;

                Assert.Equal(1, count);
            }

            [Fact]
            public void Dequeue_CountShouldBeZero()
            {
                queue.Dequeue();

                int count = queue.Count;

                Assert.Equal(0, count);
            }

            [Fact]
            public void Peek_CountShouldBeOne()
            {
                queue.Peek();

                int count = queue.Count;

                Assert.Equal(1, count);
            }

            [Fact]
            public void Dequeue_ShouldReturnQueuedValue()
            {
                int actual = queue.Dequeue();

                Assert.Equal(queuedValue, actual);
            }

            [Fact]
            public void Peek_ShouldReturnQueuedValue()
            {
                int actual = queue.Peek();

                Assert.Equal(queuedValue, actual);
            }
        }

        public class QueueWithMultipleValues
        {
            readonly Queue<int> queue;
            const int firstQueuedValue = 10;
            const int secondQueuedValue = 20;
            const int thirdQueuedValue = 30;

            public QueueWithMultipleValues()
            {
                queue = new Queue<int>();
                queue.Enqueue(firstQueuedValue);
                queue.Enqueue(secondQueuedValue);
                queue.Enqueue(thirdQueuedValue);
            }

            [Fact]
            public void Count_ShouldBeThree()
            {
                int count = queue.Count;

                Assert.Equal(3, count);
            }

            [Fact]
            public void Dequeue_VerifyFifoOrder()
            {
                Assert.Equal(firstQueuedValue, queue.Dequeue());
                Assert.Equal(secondQueuedValue, queue.Dequeue());
                Assert.Equal(thirdQueuedValue, queue.Dequeue());
            }

            [Fact]
            public void Peek_ReturnsFirstQueuedValue()
            {
                int actual = queue.Peek();

                Assert.Equal(firstQueuedValue, actual);
            }

            [Fact]
            public void Contains_ReturnsTrue()
            {
                bool contains = queue.Contains(secondQueuedValue);

                Assert.True(contains);
            }
        }

        public class QueueWithStrings
        {
            [Fact]
            public void Dequeue_ShouldReturnQueuedValue()
            {
                Queue<string> queue = new();
                queue.Enqueue("Help");

                string actual = queue.Dequeue();

                Assert.Equal("Help", actual);
            }
        }
    }
}
