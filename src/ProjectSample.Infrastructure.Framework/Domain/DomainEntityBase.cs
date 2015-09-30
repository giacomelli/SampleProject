using System;
using HelperSharp;
using Skahal.Infrastructure.Framework.Domain;

namespace ProjectSample.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Classe base para entidade de domínio.
    /// </summary>
    public abstract class DomainEntityBase : EntityWithIdBase<int>, IEntity, ICloneable
    {
        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DomainEntityBase"/>.
        /// </summary>
        protected DomainEntityBase()
        {
            Created = new ActionStamp();
            Modified = new ActionStamp();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém um valor que indica se é uma nova entidade ou se já existia.
        /// </summary>        
        public bool IsNew
        {
            get
            {
                return Id == 0;
            }
        }

        /// <summary>
        /// Obtém o carimbo da ação de criação.
        /// </summary>        
        public ActionStamp Created { get; internal set; }

        /// <summary>
        /// Obtém o carimbo da ação de alteração.
        /// </summary>
        public ActionStamp Modified { get; internal set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>        
        public virtual object Clone()
        {
            var cloned = ObjectHelper.CreateShallowCopy(this) as DomainEntityBase;
            cloned.Id = 0;

            return cloned;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var result = base.Equals(obj);

            return result
                && (obj.GetType().IsAssignableFrom(GetType()) || GetType().IsAssignableFrom(obj.GetType())); // Necessário em razão das Proxies do EF.
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var name = GetType().Name;
            var entityProxyNameIndex = name.IndexOf('_');

            name = entityProxyNameIndex > -1 ? name.Substring(0, entityProxyNameIndex) : name;

            return "{0}_{1}".With(name, Id).GetHashCode();
        }
        #endregion
    }
}
