using System;
using KissSpecifications;
using SampleProject.Domain.Accounts;
using SampleProject.Infrastructure.Repositories.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skahal.Infrastructure.Framework.Repositories;
using TestSharp;

namespace SampleProject.Domain.UnitTests.Accounts
{
    [TestClass]
    public class PermissionServiceTest
    {
        #region Fields
        private PermissionService m_target;
        private MemoryUnitOfWork m_unitOfWork;
        private MemoryPermissionRepository m_repository;
        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            m_unitOfWork = new MemoryUnitOfWork();
            m_repository = new MemoryPermissionRepository(m_unitOfWork);

            for (int i = 1; i <= 5; i++)
            {
                m_repository.Add(new Permission() { ActionName = "Action " + i, ControllerName = "Controller" });
            }

            m_unitOfWork.Commit();

            m_target = new PermissionService(m_repository, m_unitOfWork);
        }

        [TestMethod]
        public void Save_PermissionNull_Exception()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("entity"), () =>
            {
                m_target.Save(null);
            });
        }

        [TestMethod]
        public void Save_PermissionInvalid_Exception()
        {
            var permission = new Permission() { ActionName = "", ControllerName = "" };

            ExceptionAssert.IsThrowing(typeof(SpecificationNotSatisfiedException), () =>
            {
                m_target.Save(permission);
            });

            Assert.AreEqual(0, permission.Id);
            Assert.AreEqual(5, m_repository.CountAll());
        }

        [TestMethod]
        public void Save_NewPermissionValid_Created()
        {
            var permission = new Permission() { ActionName = "nome", ControllerName = "controller" };

            m_target.Save(permission);
            m_unitOfWork.Commit();

            Assert.AreEqual(6, permission.Id);
            Assert.AreEqual(6, m_repository.CountAll());
        }

        [TestMethod]
        public void Save_NewPermissionValidWithExistingControllerNameAndActionName_IdInstanceUpdated()
        {
            var permission = new Permission() { ActionName = "Action 2", ControllerName = "controller" };

            m_target.Save(permission);
            m_unitOfWork.Commit();

            Assert.AreEqual(2, permission.Id);
            Assert.AreEqual(5, m_repository.CountAll());
        }

        [TestMethod]
        public void Save_OldPermissionValid_Updated()
        {
            var permission = new Permission() { Id = 3, ActionName = "Permission 4 - UPDATED", ControllerName = "Descrição 4 - UPDATED" };

            m_target.Save(permission);
            m_unitOfWork.Commit();

            Assert.AreEqual(3, permission.Id);
            Assert.AreEqual(5, m_repository.CountAll());
            Assert.AreEqual("Permission 4 - UPDATED", m_target.GetById(3).ActionName);
            Assert.AreEqual("Descrição 4 - UPDATED", m_target.GetById(3).ControllerName);
        }

        [TestMethod]
        public void GetAllPermissions_NoArgs_AllPermissions()
        {
            var actual = m_target.Get(0, int.MaxValue, f => true);
            Assert.AreEqual(5, actual.TotalCount);
        }

        [TestMethod]
        public void RemovePermission_NotExistId_Exception()
        {
            ExceptionAssert.IsThrowing(typeof(SpecificationNotSatisfiedException), () =>
            {
                m_target.Remove(6);
            });
        }

        [TestMethod]
        public void RemovePermission_ExistsId_Deleted()
        {
            m_target.Remove(2);
            m_target.Remove(4);
            m_unitOfWork.Commit();

            Assert.IsNull(m_target.GetById(2));
            Assert.IsNull(m_target.GetById(4));
        }
    }
}
