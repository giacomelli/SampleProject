using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Globalization;
using TestSharp;

namespace SampleProject.Infrastructure.Framework.UnitTests
{
    [TestClass]
    public class GlobalizationHelperTest
    {
        [TestMethod]
        public void GetText_NullKey_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("key"), () =>
            {
                GlobalizationHelper.GetText(null);
            });
        }

        [TestMethod]
        public void GetText_KeyDoesNotExists_NotFoundText()
        {
            Assert.AreEqual("[TEXT NOT FOUND] ___DOES_NOT_EXISTS___", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___"));
        }

        [TestMethod]
        public void GetText_KeyExists_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("Name"));
        }

        [TestMethod]
        public void GetText_NullMainKey_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("key"), () =>
            {
                GlobalizationHelper.GetText(null, "Name");
            });

        }

        [TestMethod]
        public void GetText_KeyAndFallbackDoesNotExists_NotFoundText()
        {
            Assert.AreEqual("[TEXT NOT FOUND] ___DOES_NOT_EXISTS___", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___", "___DOES_NOT_EXISTS___"));
        }

        [TestMethod]
        public void GetText_KeyExistsFallbackIgnored_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("Name", "User"));
        }

        [TestMethod]
        public void GetText_KeyDoesNotExistsFallbackUsed_Translated()
        {
            Assert.AreEqual("Nome", GlobalizationHelper.GetText("___DOES_NOT_EXISTS___", "Name"));
        }

        [TestMethod]
        public void GetText_ConditionTrue_TrueKey()
        {
            Assert.AreEqual("Sim", GlobalizationHelper.GetText(true, "Yes", "No"));
        }

        [TestMethod]
        public void GetText_ConditionFalse_FalseKey()
        {
            Assert.AreEqual("Não", GlobalizationHelper.GetText(false, "Yes", "No"));
        }

        [TestMethod]
        public void ToYesNo_ConditionTrue_Yes()
        {
            Assert.AreEqual("Sim", true.ToYesNo());
        }

        [TestMethod]
        public void ToYesNo_ConditionFalse_No()
        {
            Assert.AreEqual("Não", false.ToYesNo());
        }
    }
}
