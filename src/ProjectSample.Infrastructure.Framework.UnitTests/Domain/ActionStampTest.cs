﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Infrastructure.Framework.Domain;
using ProjectSample.Infrastructure.Framework.Globalization;
using ProjectSample.Infrastructure.Framework.Runtime;
using TestSharp;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Domain
{
    [TestClass]
    public class ActionStampTest
    {
        [TestMethod]
        public void Stamp_NoUser_Exception()
        {
            var target = new ActionStamp();

            ExceptionAssert.IsThrowing(new ArgumentException(Texts.CanStampRecordWithNoUser), () =>
            {
                target.Stamp(0);
            });
        }

        [TestMethod]
        public void Stamp_NoArgs_DateAndUserUpdated()
        {
            RuntimeContext.Current = new MemoryRuntimeContext()
            {
                User = new MemoryRuntimeUser()
                {
                    Id = 1
                }
            };

            var target = new ActionStamp();
            target.Stamp();

            Assert.AreEqual(DateTime.UtcNow.Date, target.Date.Date);
            Assert.AreEqual(RuntimeContext.Current.User.Id, target.UserId);
        }
    }
}
