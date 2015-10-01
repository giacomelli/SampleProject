using System;
using System.Collections.Generic;
using HelperSharp;
using KissSpecifications;
using SampleProject.Domain.Accounts;
using SampleProject.Infrastructure.Framework.Globalization;
using SampleProject.Infrastructure.Repositories.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Skahal.Infrastructure.Framework.Repositories;
using TestSharp;
using System.Linq.Expressions;
using SampleProject.Infrastructure.Framework.Runtime;

namespace SampleProject.Domain.UnitTests.Accounts
{
    [TestClass]
    public class RoleServiceTest
    {
        #region Fields
        private RoleService m_target;
        private IRepository<Role> m_roleRepository;
        private IUnitOfWork m_unitOfWork;
        #endregion

        #region Initialize
        [TestInitialize]
        public void InitializeTest()
        {
            m_unitOfWork = new MemoryUnitOfWork();
            m_roleRepository = new MemoryRoleRepository(m_unitOfWork);

            var permissionService = MockRepository.GenerateMock<IPermissionService>();

            m_unitOfWork.Commit();

            m_target = new RoleService(m_roleRepository, m_unitOfWork, permissionService);
        }
        #endregion

        #region Tests
        [TestMethod]
        public void Save_NullRole_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("entity"), () =>
            {
                m_target.Save(null);
            });
        }

        [TestMethod]
        public void Save_New_Created()
        {
            m_target.Save(new Role() { Name = "TESTE", Description = "4d5sf4ad32423%$@", Permissions = new List<Permission>() { new Permission() } });
            Assert.AreEqual(0, m_roleRepository.CountAll());

            m_unitOfWork.Commit();
            Assert.AreEqual(1, m_roleRepository.CountAll());
        }

        [TestMethod]
        public void Save_ExistsWithSameName_Exception()
        {
            m_target.Save(new Role() { Name = "TESTE", Description = "4d5sf4ad32423%$@", Permissions = new List<Permission>() { new Permission() } });
            m_unitOfWork.Commit();

            ExceptionAssert.IsThrowing(new SpecificationNotSatisfiedException(Texts.ThereIsOtherEntityWithSameName.With("papel", "TESTE")), () =>
            {
                m_target.Save(new Role() { Name = "TESTE", Description = "4d5sf4dsfasdfsdad32423%$@", Permissions = new List<Permission>() { new Permission() } });
            });
        }

        [TestMethod]
        public void Save_Old_Updated()
        {
            m_target.Save(new Role() { Name = "TESTE", Description = "4d5sf4ad32423%$@", Permissions = new List<Permission>() { new Permission() } });
            m_unitOfWork.Commit();
            Assert.AreEqual(1, m_roleRepository.CountAll());

            var role = m_roleRepository.FindFirst() as Role;

            var toUpdated = role.Clone() as Role;
            toUpdated.Id = role.Id;
            toUpdated.Name = "Updated";

            m_target.Save(toUpdated);
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_roleRepository.CountAll());

            var updatedRole = m_roleRepository.FindFirst();

            Assert.AreEqual("Updated", updatedRole.Name);
        }

        [TestMethod]
        public void Get_NoArgs_AllRoles()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_roleRepository.Add(new Role());
            m_unitOfWork.Commit();

            Assert.AreEqual(1, m_target.Get(0, int.MaxValue, f => true).TotalCount);
        }

        [TestMethod]
        public void GetById_Id_Role()
        {
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, f => true).TotalCount);
            m_roleRepository.Add(new Role() { Name = "TEST1" });
            m_roleRepository.Add(new Role() { Name = "TEST2" });
            m_unitOfWork.Commit();

            var expected = m_roleRepository.FindFirst(u => u.Name.Equals("TEST2"));

            Assert.AreEqual(expected, m_target.GetById(expected.Id));
        }

        [TestMethod]
        public void RemoveRole_Role_Removed()
        {
            m_roleRepository.Add(new Role() { Name = "TEST1" });
            m_roleRepository.Add(new Role() { Name = "TEST2" });
            m_unitOfWork.Commit();

            m_target.Remove(2);
            m_unitOfWork.Commit();


            Assert.IsNotNull(m_roleRepository.FindBy(1));
            Assert.IsNull(m_roleRepository.FindBy(2));

        }

        [TestMethod]
        public void GetByName_Name_Filtered()
        {
            m_roleRepository.Add(new Role() { Name = "TEST1" });
            m_roleRepository.Add(new Role() { Name = "TEST2" });
            m_unitOfWork.Commit();

            Assert.AreEqual(2, m_target.GetByName(0, int.MaxValue, "TEST").TotalCount);
            Assert.AreEqual(1, m_target.GetByName(0, int.MaxValue, "TEST1").TotalCount);
            Assert.AreEqual("TEST1", m_target.GetByName("TEST1").Name);
        }

        [TestMethod]
        public void GetByFilter_OffsetLimitAndFilter_FilterResult()
        {
            m_roleRepository.Add(new Role() { Name = "TEST1", Description = "DESCR1" });
            m_roleRepository.Add(new Role() { Name = "TEST2", Description = "DESCR2" });
            m_roleRepository.Add(new Role() { Name = "TEST3", Description = "DESCR3" });
            m_unitOfWork.Commit();

            Expression<Func<Role, bool>> query = null;
            RoleFilter filter = new RoleFilter();

            query = GetRoleFilterQuery(filter);
            Assert.AreEqual(3, m_target.Get(0, int.MaxValue, query).TotalCount);

            filter = new RoleFilter()
            {
                Name = "1"
            };
            
            filter.Sanitize();
            
            query = GetRoleFilterQuery(filter);
            Assert.AreEqual(1, m_target.Get(0, int.MaxValue, query).TotalCount);

            filter = new RoleFilter() 
            {
                Description = "3"
            };
            
            filter.Sanitize();

            query = GetRoleFilterQuery(filter);
            Assert.AreEqual(1, m_target.Get(0, int.MaxValue, query).TotalCount);

            filter = new RoleFilter()
            {
                Name = "1",
                Description = "2"
            };

            filter.Sanitize();
            
            query = GetRoleFilterQuery(filter);
            Assert.AreEqual(0, m_target.Get(0, int.MaxValue, query).TotalCount);
        }    
        #endregion

        #region Helpers
        private Expression<Func<Role, bool>> GetRoleFilterQuery(RoleFilter filter)
        {
            Expression<Func<Role, bool>> query = (f) => (filter.Description == null || f.Description.ToLower().Contains(filter.Description)) &&
                (filter.Name == null || f.Name.ToLower().Contains(filter.Name));

            return query;
        }
        #endregion
    }
}
