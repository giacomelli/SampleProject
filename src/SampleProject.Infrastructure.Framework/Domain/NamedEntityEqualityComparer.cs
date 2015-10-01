using System;
using System.Collections.Generic;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Comparador para INamedEntity.
    /// </summary>
    public class NamedEntityEqualityComparer : IEqualityComparer<INamedEntity>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of to compare.</param>
        /// <param name="y">The second object of to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(INamedEntity x, INamedEntity y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Name.Equals(y.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(INamedEntity obj)
        {
            if (obj == null || obj.Name == null)
            {
                return 0;
            }

            return obj.Name.GetHashCode();
        }
    }
}
