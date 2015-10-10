using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Runtime;

namespace SampleProject.Infrastructure.Framework.UnitTests.Runtime
{
    [TestClass]
    public class RuntimeContextTest
    {
        [TestMethod]
        public void Current_NotDefined_MemoryContext()
        {
            Assert.IsInstanceOfType(RuntimeContext.Current, typeof(MemoryRuntimeContext));
        }

        [TestMethod]
        public void Environment_CurrentCompilation()
        {
            Assert.AreNotEqual(-1, RuntimeContext.Environment);
        }
    }
}
