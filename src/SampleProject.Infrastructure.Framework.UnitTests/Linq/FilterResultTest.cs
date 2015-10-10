using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleProject.Infrastructure.Framework.Linq;

namespace SampleProject.Infrastructure.Framework.UnitTests.Linq
{
    [TestClass]
    public class FilterResultTest
    {
        [TestMethod]
        public void Constructor_Args_PropertiesDefined()
        {
            var target = new FilterResult<int>(new int[] { 1, 2, 3 }, 6);
            Assert.AreEqual(3, target.Entities.Count());
            Assert.AreEqual(6, target.TotalCount);
        }

        [TestMethod]
        public void GetEntities_NoArgs_Entities()
        {
            var target = new FilterResult<int>(new int[] { 1, 2, 3 }, 6);
            Assert.AreEqual(3, ((IFilterResult)target).GetEntities().Cast<object>().Count());
        }

        [TestMethod]
        public void SetEntities_Entities_Changed()
        {
            var target = new FilterResult<int>(new int[] { 1, 2, 3 }, 6);
            ((IFilterResult)target).SetEntities(new int[] { 1 });
            ((IFilterResult)target).TotalCount = 1;
            Assert.AreEqual(1, ((IFilterResult)target).GetEntities().Cast<object>().Count());
            Assert.AreEqual(1, ((IFilterResult)target).TotalCount);
        }

        [TestMethod]
        public void ToAnonymous_Entities_Anonymous()
        {
            var target = new FilterResult<int>(new int[] { 1, 2, 3 }, 6);
            var actual = target.ToAnonymous((i) =>
            {
                return (i * 10).ToString();
            });

            var entities = actual.Entities.ToList();
            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual(6, actual.TotalCount);
            Assert.AreEqual("10", entities[0]);
            Assert.AreEqual("20", entities[1]);
            Assert.AreEqual("30", entities[2]);
        }
    }
}
