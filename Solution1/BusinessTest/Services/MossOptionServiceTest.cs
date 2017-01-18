using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Business.Services;
using Business.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessTest
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