using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectSample.Infrastructure.Framework.Runtime;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Runtime
{
    [TestClass]
    public class MemoryRuntimeContextTest
    {
        [TestMethod]
        public void Country_DefaultAndSet_Value()
        {
            var target = new MemoryRuntimeContext();
            Assert.AreEqual("pt", target.Culture.Parent.Name);
        }
    }
}
