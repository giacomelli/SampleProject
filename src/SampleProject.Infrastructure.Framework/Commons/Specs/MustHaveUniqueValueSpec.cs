using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using HelperSharp;
using KissSpecifications;

namespace SampleProject.Infrastructure.Framework.Commons.Specs
{
    /// <summary>
    ///  Target must have a unique value for the specified property.
    /// </summary>
    /// <typeparam name="TTarget">The target.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class MustHaveUniqueValueSpec<TTarget, TValue> : SpecificationBase<TTarget>
    {
        #region Constants
        /// <summary>
        /// The default text for not satisfied reason.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public const string NotSatisfiedReasonText = "There is already a {0} with the value '{1}'";
        #endregion

        #region Fields
        private Expression<Func<TTarget, TValue>> m_getProperty;
        private Expression<Func<TValue, TTarget>> m_getByName;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MustHaveUniqueValueSpec{TTarget, TValue}" />.
        /// </summary>
        /// <param name="getProperty">The text property that must have a unique value.</param>
        /// <param name="getByName">The function used to find other target with the same text property value.</param>
        public MustHaveUniqueValueSpec(Expression<Func<TTarget, TValue>> getProperty, Expression<Func<TValue, TTarget>> getByName)
        {
            m_getProperty = getProperty;
            m_getByName = getByName;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the target object satisfies the specification.
        /// </summary>
        /// <param name="target">The target object to be validated.</param>
        /// <returns>
        ///   <c>true</c> if target object satisfies the specification; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override bool IsSatisfiedBy(TTarget target)
        {
            var value = m_getProperty.Compile()(target);

            var otherEntityWithSameName = m_getByName.Compile()(value);

            if (otherEntityWithSameName != null && !otherEntityWithSameName.Equals(target))
            {
                var globalizationResolver = KissSpecificationsConfig.GlobalizationResolver;

                object formattedValue = value;

                if (typeof(TValue).IsEnum)
                {
                    formattedValue = globalizationResolver.GetText(value.ToString());
                }

                NotSatisfiedReason = globalizationResolver
                    .GetText(NotSatisfiedReasonText)
                    .With(globalizationResolver.GetText(typeof(TTarget).Name).ToLowerInvariant(), formattedValue);

                return false;
            }

            return true;
        }
        #endregion
    }
}