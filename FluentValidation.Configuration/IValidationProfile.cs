namespace FluentValidation.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValidationProfile
    {
        /// <summary>
        /// Gets instance of configuration which contains validators.
        /// </summary>
        IValidationConfiguration Configuration { get; }
    }
}
