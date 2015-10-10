using System;
using SampleProject.Infrastructure.Framework.Globalization;

namespace SampleProject.Infrastructure.Framework.Commons
{
    /// <summary>
    /// Represents a range of values.
    /// </summary>
    /// <typeparam name="T">The data type the values will have.</typeparam>
    public struct RangeValue<T> where T : struct, IComparable<T>
    {
        private T? startValue;
        private T? endValue;

        /// <summary>
        /// Obtém um valor que indica se o valor inicial ou o valor final não foram informados.
        /// </summary>
        /// <value>
        ///   <c>true</c> se esta instância está vazia; caso contrário, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return !this.startValue.HasValue || !this.endValue.HasValue;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se esta instância está incompleta.
        /// </summary>
        /// <value>
        /// <c>true</c> se esta instância está incompleta; caso contrário, <c>false</c>.
        /// </value>
        public bool IsIncomplete
        {
            get
            {
                return (this.startValue.HasValue && !this.endValue.HasValue) || (this.endValue.HasValue && !this.startValue.HasValue);
            }
        }

        /// <summary>
        /// Obtém ou define o valor inicial.
        /// </summary>
        /// <value>
        /// O valor inicial.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">Caso o valor inicial for maior do que o final.</exception>
        public T? StartValue
        {
            get
            {
                return this.startValue;
            }

            set
            {
                if (this.endValue.HasValue && value.HasValue && this.endValue.Value.CompareTo(value.Value) < 0)
                {
                    throw new ArgumentOutOfRangeException(Texts.Value);
                }

                this.startValue = value;
            }
        }

        /// <summary>
        /// Obtém ou define o valor final.
        /// </summary>
        /// <value>
        /// O valor final. Deve ser maior ou igual ao valor inicial.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">Caso o valor final seja menor do que o inicial.</exception>
        public T? EndValue
        {
            get
            {
                return this.endValue;
            }

            set
            {
                if (this.startValue.HasValue && value.HasValue && this.startValue.Value.CompareTo(value.Value) > 0)
                {
                    throw new ArgumentOutOfRangeException(Texts.Value);
                }

                this.endValue = value;
            }
        }

        /// <summary>
        /// Implementa o operador ==.
        /// </summary>
        /// <param name="left">Instância à esquerda.</param>
        /// <param name="right">Instância à direita</param>
        /// <returns>
        /// <c>true</c> se as instâncias forem iguais; caso contrário <c>false</c>.
        /// </returns>
        public static bool operator ==(RangeValue<T> left, RangeValue<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implementa o operador !=.
        /// </summary>
        /// <param name="left">A instância à esquerda.</param>
        /// <param name="right">A instância à direita.</param>
        /// <returns>
        /// <c>true</c> se as instâncias forem diferentes; caso contrário <c>false</c>.
        /// </returns>
        public static bool operator !=(RangeValue<T> left, RangeValue<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is RangeValue<T>))
            {
                return false;
            }

            var other = (RangeValue<T>)obj;

            return this.startValue.GetValueOrDefault().CompareTo(other.startValue.GetValueOrDefault()) == 0 &&
                this.endValue.GetValueOrDefault().CompareTo(other.endValue.GetValueOrDefault()) == 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.startValue.GetHashCode() ^ this.endValue.GetHashCode();
        }
    }
}
