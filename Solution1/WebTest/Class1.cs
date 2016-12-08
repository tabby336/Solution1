﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTest
{
    [TestClass]
    public class Class1
    {
        private string test;

        [TestInitialize]
        public void SetUp()
        {
            test = "TEST";
        }

        [TestCleanup]
        public void TearDown()
        {
            test = null;
        }

        [TestMethod]
        public void When_TestIsTest_Then_ShouldBeTest()
        {
            test.Should().Be("TEST");
        }
    }
}
