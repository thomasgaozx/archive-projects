using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStasher.Control.Core;
using static TaskStasher.ZConsole.ConsoleSyntax.ZConsoleSyntax;

namespace TaskStasher.ZConsole.UnitTests
{
    [TestFixture]
    public class ZCommandProcessorUnitTests
    {
        private ZTaskManager manager;
        private ZCommandProcessor processor;

        [SetUp]
        public void SetUpTest()
        {
            manager = new ZTaskManager();
            processor = new ZCommandProcessor(manager);
        }

        [Test]
        public void ZCommandProcessor_QuitProgram()
        {
            // Act
            bool quit = processor.ProcessCommand(new string[] { Quit });
            

            // Assert
            Assert.That(quit, Is.True);
        }
        
    }
}
