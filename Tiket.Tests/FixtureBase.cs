using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Tiket.Tests
{
    public abstract class FixtureBase
    {
        readonly ConcurrentStack<IDisposable> _stuffToDispose = new ConcurrentStack<IDisposable>();

        protected const string ValidKey = @"bBm0bF8SWdkaBNLcb5gA0yUksVodZkkKRObwnAvsxZk=|v2Fg9JhLX+ZTkjBT9LOvwA==";

        [SetUp]
        public void InternalSetUp()
        {
            SetUp();
        }

        protected virtual void SetUp()
        {
        }

        [TearDown]
        public void InternalTearDown()
        {
            IDisposable disposable;

            while (_stuffToDispose.TryPop(out disposable))
            {
                Console.WriteLine($"Disposing {disposable}");
                disposable.Dispose();
            }
        }

        protected TDisposable Using<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
        {
            _stuffToDispose.Push(disposable);
            return disposable;
        }
    }
}
