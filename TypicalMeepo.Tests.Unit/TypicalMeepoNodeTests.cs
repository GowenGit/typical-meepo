using System;
using NUnit.Framework;
using TypicalMeepo.Tests.Unit.Core;

namespace TypicalMeepo.Tests.Unit
{
    [TestFixture]
    public class TypicalMeepoNodeTests
    {
        private FakeMeepoNode fakeMeepoNode;

        [SetUp]
        public void Initialize()
        {
            fakeMeepoNode = new FakeMeepoNode();
        }

        private TypicalMeepoNode GetSutObject()
        {
            return new TypicalMeepoNode(fakeMeepoNode);
        }

        [Test]
        public void Subscribe_WhenCalledWithNull_ShouldThrow()
        {
            var sut = GetSutObject();

            Assert.Throws<ArgumentNullException>(() => sut.Subscribe<string>(null));
        }

        [Test]
        public void Subscribe_WhenCalled_ShouldSubscribeToMeepo()
        {
            var sut = GetSutObject();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());

            sut.Subscribe<TestPackage>((id, data) => { });

            Assert.AreEqual(1, fakeMeepoNode.GetSubscriberCount());
        }

        [Test]
        public void Subscribe_WhenCalledTwice_ShouldOverride()
        {
            var sut = GetSutObject();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());

            sut.Subscribe<TestPackage>((id, data) => { });

            Assert.AreEqual(1, fakeMeepoNode.GetSubscriberCount());

            sut.Subscribe<TestPackage>((id, data) => { });

            Assert.AreEqual(1, fakeMeepoNode.GetSubscriberCount());
        }

        [Test]
        public void Unsubscribe_WhenCalledWithNoSubscribtions_ShouldDoNothing()
        {
            var sut = GetSutObject();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());

            sut.Unsubscribe<TestPackage>();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());
        }

        [Test]
        public void Unsubscribe_WhenCalledAfterSubscribe_ShouldUnsubscribe()
        {
            var sut = GetSutObject();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());

            sut.Subscribe<TestPackage>((id, data) => { });

            Assert.AreEqual(1, fakeMeepoNode.GetSubscriberCount());

            sut.Unsubscribe<TestPackage>();

            Assert.AreEqual(0, fakeMeepoNode.GetSubscriberCount());
        }
    }
}
