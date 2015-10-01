using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Runtime;

namespace SampleProject.Infrastructure.Framework.UnitTests.Runtime
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
