using HelperSharp;

namespace ProjectSample.Infrastructure.Framework.Globalization
{
    /// <summary>
    /// Clase com métodos auxiliares a Globalização
    /// </summary>
    public static class GlobalizationHelper
    {
        /// <summary>
        /// Obtém o texto globalizado para a cultura corrente da chave informada.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>O texto globalizado.</returns>
        public static string GetText(object key)
        {
            return GetText(key, true);
        }

        /// <summary>
        /// Obtém o texto globalizado para a cultura corrente da chave informada.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="markNotFound">Se deve marcar uma chave não encontrada com o texto [TEXT NOT FOUND].</param>
        /// <returns>O texto globalizado.</returns>
        public static string GetText(object key, bool markNotFound)
        {
            ExceptionHelper.ThrowIfNull("key", key);

            Texts.ResourceManager.IgnoreCase = true;
            var result = Texts.ResourceManager.GetString(key.ToString());

            if (markNotFound && result == null)
            {
                result = "[TEXT NOT FOUND] {0}".With(key);
            }

            return result;
        }

        /// <summary>
        /// Obtém o texto globalizado para a cultura corrente da chave informada.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="fallbackKey">A chave de globalização reserva no caso de não achar a chave informada.</param>
        /// <returns>O texto globalizado.</returns>
        public static string GetText(object key, object fallbackKey)
        {
            var text = GetText(key, false);

            if (text == null)
            {
                text = GetText(fallbackKey);
            }

            return text;
        }

        /// <summary>
        /// Obtém o texto globalizado para a cultura conforme a condição.
        /// </summary>
        /// <param name="condition">A condição para decidir qual chave será utilizada.</param>
        /// <param name="trueKey">A chave de globlaização para quando a condição for verdadeira.</param>
        /// <param name="falseKey">A chave de globlaização para quando a condição for falsa.</param>
        /// <param name="markNotFound">Se deve marcar uma chave não encontrada com o texto [TEXT NOT FOUND].</param>
        /// <returns>O texto globalizado.</returns>
        public static string GetText(bool condition, object trueKey, object falseKey, bool markNotFound = true)
        {
            return condition ? GetText(trueKey, markNotFound) : GetText(falseKey, markNotFound);
        }

        /// <summary>
        /// Obtém o texto globalizado para a cultura conforme a condição para Yes ou No.
        /// </summary>
        /// <param name="condition">A condição para decidir qual chave será utilizada.</param>
        /// <returns>O texto globalizado.</returns>
        public static string ToYesNo(this bool condition)
        {
            return condition ? Texts.Yes : Texts.No;
        }
    }
}