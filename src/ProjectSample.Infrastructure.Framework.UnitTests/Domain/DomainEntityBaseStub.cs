using System;
using ProjectSample.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Domain;

namespace ProjectSample.Infrastructure.Framework.UnitTests.Domain
{
    public class DomainEntityBaseStub : DomainEntityBase, IAggregateRoot
    {
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
