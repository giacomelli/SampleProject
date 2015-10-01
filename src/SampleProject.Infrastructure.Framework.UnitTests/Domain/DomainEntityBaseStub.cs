using System;
using SampleProject.Infrastructure.Framework.Domain;
using Skahal.Infrastructure.Framework.Domain;

namespace SampleProject.Infrastructure.Framework.UnitTests.Domain
{
    public class DomainEntityBaseStub : DomainEntityBase, IAggregateRoot
    {
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
