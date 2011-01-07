using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TaskHandler.BusinessLogic;
using TaskHandler.BusinessLogic.Impl;
using TaskHandler.Commons;

namespace TaskHandler.Tests
{
    // todo: write some tests
    [TestFixture]
    public class StartTaskFixture
    {
        [Test]
        public void LoadTasksTest()
        {
            var triggerLoader = IoC.Resolve<ITriggerLoader>();

            var triggers = triggerLoader.LoadTriggers();

            Assert.AreEqual(1, triggers.Count);
        }

        [Test]
        [Ignore("Is not testing anything")]
        public void StartProcessingTest()
        {
            var handler = new MainTaskProcessor(IoC.Resolve<ITriggerLoader>());

            handler.Start();
        }
    }
}
