using System;

namespace FluentValidation.Configuration
{
    public abstract class AbstractValidationProfile : IValidationProfile
    {
        public IValidationConfiguration Configuration { get; protected set; }

        protected AbstractValidationProfile()
        {
            Configuration = new ValidationConfiguration();
        }

        protected AbstractValidationProfile(IValidationConfiguration config)
        {
            if(config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Configuration = config;
        }
    }
}
