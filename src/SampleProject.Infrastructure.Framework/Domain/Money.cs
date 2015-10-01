using System;
using System.Globalization;
using HelperSharp;
using SampleProject.Infrastructure.Framework.Globalization;

namespace SampleProject.Infrastructure.Framework.Domain
{
    /// <summary>
    /// Representa um valor monetário.
    /// </summary>    
    public class Money
    {
        #region Fields
        private CultureInfo m_cultureInfo;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Money"/>.
        /// </summary>
        public Money()
            : this(0, "R$")
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Money" />.
        /// </summary>
        /// <param name="amount">A quantia.</param>
        public Money(decimal amount)
            : this(amount, "R$")
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="Money"/>.
        /// </summary>
        /// <param name="amount">A quantia.</param>
        /// <param name="currency">A moeda.</param>
        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Obtém ou define a quantidade.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Obtém ou define a moeda.
        /// </summary>
        public string Currency { get; set; }

        private CultureInfo CultureInfo
        {
            get
            {
                if (m_cultureInfo == null)
                {
                    m_cultureInfo = CultureInfoHelper.GetCultureInfoByCurrency(Currency);
                }

                return m_cultureInfo;
            }
        }
        #endregion

        #region Operators

        /// <summary>
        /// Soma dois valores da mesma moeda.
        /// </summary>
        /// <param name="left">Primeiro valor.</param>
        /// <param name="right">Segundo valor.</param>
        /// <returns>A soma dos valores, na mesma moeda.</returns>
        public static Money operator +(Money left, Money right)
        {
            ExceptionHelper.ThrowIfNull("left", left);
            ExceptionHelper.ThrowIfNull("right", right);

            EnsureSameCurrency(left, right);

            return Money.SameCurrency(left, Math.Round(left.Amount + right.Amount, 2));
        }

        /// <summary>
        /// Subtrai um valor de outro da mesma moeda.
        /// </summary>
        /// <param name="left">Primeiro valor.</param>
        /// <param name="right">Segundo valor.</param>
        /// <returns>O resultado da subtração dos valores, na mesma moeda.</returns>
        public static Money operator -(Money left, Money right)
        {
            ExceptionHelper.ThrowIfNull("left", left);
            ExceptionHelper.ThrowIfNull("right", right);

            EnsureSameCurrency(left, right);

            return Money.SameCurrency(left, Math.Round(left.Amount - right.Amount, 2));
        }

        /// <summary>
        /// Multiplica um valor de moeda.
        /// </summary>
        /// <param name="left">O valor.</param>
        /// <param name="right">O multiplicador.</param>
        /// <returns>O valor multiplicado, na mesma moeda.</returns>
        public static Money operator *(Money left, int right)
        {
            ExceptionHelper.ThrowIfNull("left", left);

            return Money.SameCurrency(left, Math.Round(left.Amount * right, 2));
        }

        /// <summary>
        /// Multiplica um valor de moeda.
        /// </summary>
        /// <param name="left">O valor.</param>
        /// <param name="right">O multiplicador.</param>
        /// <returns>O valor multiplicado, na mesma moeda.</returns>
        public static Money operator *(Money left, decimal right)
        {
            ExceptionHelper.ThrowIfNull("left", left);

            return Money.SameCurrency(left, Math.Round(left.Amount * right, 2));
        }

        /// <summary>
        /// Dividi um valor de moeda.
        /// </summary>
        /// <param name="left">O valor.</param>
        /// <param name="right">O divisor.</param>
        /// <returns>O valor dividido, na mesma moeda.</returns>
        public static Money operator /(Money left, decimal right)
        {
            ExceptionHelper.ThrowIfNull("left", left);

            return Money.SameCurrency(left, Math.Round(left.Amount / right, 2));
        }

        /// <summary>
        /// Compara duas instâncias de Money.
        /// </summary>
        /// <param name="left">O primeiro valor.</param>
        /// <param name="right">O segundo valor.</param>
        /// <returns>True caso os valores sejam iguais, False caso contrário.</returns>
        public static bool operator ==(Money left, Money right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (null == (object)left || null == (object)right)
            {
                return false;
            }

            return left.Amount == right.Amount && left.Currency == right.Currency;
        }

        /// <summary>
        /// Compara duas instâncias de Money.
        /// </summary>
        /// <param name="left">O primeiro valor.</param>
        /// <param name="right">O segundo valor.</param>
        /// <returns>True caso os valores sejam diferentes, False caso contrário.</returns>
        public static bool operator !=(Money left, Money right)
        {
            if (ReferenceEquals(left, right))
            {
                return false;
            }

            if (null == (object)left || null == (object)right)
            {
                return true;
            }

            return left.Amount != right.Amount || left.Currency != right.Currency;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cria uma instância de Money utilizando a moeda Reais (R$).
        /// </summary>
        /// <param name="amount">A quantidade na moeda.</param>
        /// <returns>O dinheiro em R$.</returns>
        public static Money Reais(decimal amount)
        {
            return new Money(amount, "BRL");
        }

        /// <summary>
        /// Cria uma instância de Money utilizando a moeda Dólares (US$).
        /// </summary>
        /// <param name="amount">A quantidade na moeda.</param>
        /// <returns>O dinheiro em US$.</returns>
        public static Money USDollars(decimal amount)
        {
            return new Money(amount, "USD");
        }

        /// <summary>
        /// Cria uma instância de Money utilizando a mesma moeda que outra instância.
        /// </summary>
        /// <param name="money">Money contendo a moeda a ser mantida.</param>
        /// <param name="amount">A quantidade na moeda.</param>
        /// <returns>O dinheiro na mesma moeda informada.</returns>
        public static Money SameCurrency(Money money, decimal amount)
        {
            return new Money(amount, money.Currency);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo, "{0:c2}", Amount);
        }

        /// <summary>
        /// Determina se a instância informada é igual a esta instância.
        /// </summary>
        /// <param name="obj">A instância a comparar com esta instância.</param>
        /// <returns>True caso ambas instâncias sejam iguais, False caso contrário.</returns>
        public override bool Equals(object obj)
        {
            Money money = obj as Money;
            if (null == (object)money)
            {
                return false;
            }

            return money.Amount == this.Amount && money.Currency == this.Currency;
        }

        /// <summary>
        /// Retorna um hash code para esta instância.
        /// </summary>
        /// <returns>Um hash code.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Soma dois valores da mesma moeda.
        /// </summary>
        /// <param name="right">Segundo valor.</param>
        /// <returns>A soma dos valores, na mesma moeda.</returns>
        public Money Add(Money right)
        {
            return this + right;
        }

        /// <summary>
        /// Subtrai um valor de outro da mesma moeda.
        /// </summary>
        /// <param name="right">Segundo valor.</param>
        /// <returns>O resultado da subtração dos valores, na mesma moeda.</returns>
        public Money Subtract(Money right)
        {
            return this - right;
        }

        /// <summary>
        /// Multiplica um valor de moeda.
        /// </summary>
        /// <param name="multiplier">O multiplicador.</param>
        /// <returns>O valor multiplicado, na mesma moeda.</returns>
        public Money Multiply(int multiplier)
        {
            return this * multiplier;
        }

        /// <summary>
        /// Multiplica um valor de moeda.
        /// </summary>
        /// <param name="multiplier">O multiplicador.</param>
        /// <returns>O valor multiplicado, na mesma moeda.</returns>
        public Money Multiply(decimal multiplier)
        {
            return this * multiplier;
        }

        /// <summary>
        /// Divide um valor de moeda.
        /// </summary>
        /// <param name="divider">O divisor.</param>
        /// <returns>O valor dividido, na mesma moeda.</returns>
        public Money Divide(decimal divider)
        {
            return this / divider;
        }

        private static void EnsureSameCurrency(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw new InvalidOperationException(Texts.InvalidMoneyOperationDifferentCurrencies);
            }
        }

        #endregion
    }
}
