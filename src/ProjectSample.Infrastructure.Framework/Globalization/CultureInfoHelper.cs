using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProjectSample.Infrastructure.Framework.Globalization
{
    /// <summary>
    /// Utilitários para questões envolvendo CurrencyInfo.
    /// </summary>
    public static class CultureInfoHelper
    {
        #region Fields
        private static Dictionary<string, CultureInfo> s_cultureInfoCache = new Dictionary<string, CultureInfo>();
        #endregion

        #region Methods
        /// <summary>
        /// Obtém uma CultureInfo através de seu símbolo de moeda.
        /// </summary>
        /// <param name="isoCurrencySymbol">O símbolo da moeda no padrão ISO. <example>BRL e USD.</example></param>
        /// <returns>A cultura.</returns>
        public static CultureInfo GetCultureInfoByCurrency(string isoCurrencySymbol)
        {
            lock (s_cultureInfoCache)
            {
                if (!s_cultureInfoCache.ContainsKey(isoCurrencySymbol))
                {
                    var cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                        .OrderBy(query => query.ToString().Length)
                        .FirstOrDefault(c => new RegionInfo(c.LCID).ISOCurrencySymbol.Equals(isoCurrencySymbol, StringComparison.OrdinalIgnoreCase));

                    s_cultureInfoCache.Add(isoCurrencySymbol, cultureInfo);
                }

                return s_cultureInfoCache[isoCurrencySymbol];
            }
        }
        #endregion
    }
}
