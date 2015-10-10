using System.Collections.Generic;

namespace SampleProject.Domain.Accounts
{
    public class UserFilter
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string ExternalId { get; set; }

        public string Regional { get; set; }

        public string Subregional { get; set; }

        public IEnumerable<int> RoleIds { get; set; }

        public IEnumerable<bool> UserActiveStatus { get; set; }

        #region Methods

        public void Sanitize()
        {
            UserName = Sanitize(UserName);
            FullName = Sanitize(FullName);
            ExternalId = Sanitize(ExternalId);
            Regional = Sanitize(Regional);
            Subregional = Sanitize(Subregional);
            RoleIds = RoleIds ?? new List<int>();
            UserActiveStatus = UserActiveStatus ?? new List<bool>();
        }

        private static string Sanitize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
