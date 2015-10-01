using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Extensions methods para Money.
    /// </summary>
    public static class MoneyExtensions
    {
        /// <summary>
        /// Realiza a soma dos Money projetados.
        /// </summary>
        /// <typeparam name="TSource">O tipo da lista.</typeparam>
        /// <param name="source">A lista.</param>
        /// <param name="selector">O seletor do tipo Money.</param>
        /// <returns>O Money com o valor somado.</returns>
        public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
        {
            if (source.Count() == 0)
            {
                return Money.Reais(0);
            }

            var first = selector(source.First());

            return Money.SameCurrency(first ?? Money.Reais(0), source.Sum(s => selector(s) == null ? 0 : selector(s).Amount));
        }

        /// <summary>
        /// Realiza a méida dos Money projetados.
        /// </summary>
        /// <typeparam name="TSource">O tipo da lista.</typeparam>
        /// <param name="source">A lista.</param>
        /// <param name="selector">O seletor do tipo Money.</param>
        /// <returns>O Money com o valor médio.</returns>
        public static Money Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
        {
            if (source.Count() == 0)
            {
                return Money.Reais(0);
            }

            var first = selector(source.First());

            return Money.SameCurrency(first ?? Money.Reais(0), source.Average(s => selector(s) == null ? 0 : selector(s).Amount));
        }
    }
}
