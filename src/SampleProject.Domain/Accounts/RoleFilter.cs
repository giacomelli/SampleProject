namespace SampleProject.Domain.Accounts
{
    public class RoleFilter
    {
        #region Filters
        public string Description { get; set; }

        public string Name { get; set; }
        #endregion

        #region Methods
        public void Sanitize()
        {
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.ToLowerInvariant().Trim();
            Name = string.IsNullOrWhiteSpace(Name) ? null : Name.ToLowerInvariant().Trim();
        }
        #endregion
    }
}
