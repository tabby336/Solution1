using System;
using System.IO;
using Business.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessTest.Services
{
    [TestClass]
    public class MossOptionServiceTest
    {

        [TestMethod]
        public void When_SendOptionIsCalled_WithNullParameters_NullArgumentExceptionIsThrown()
        {

            var moqStream = new Mock<Stream>(); 

            var businessMossOptions = new Mock<IMossOptionService>();
            
            businessMossOptions.Setup(x => x.SendOption("", "la", moqStream.Object)).Throws(new ArgumentNullException());

        }
    }
}