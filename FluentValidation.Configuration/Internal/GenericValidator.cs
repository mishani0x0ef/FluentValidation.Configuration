namespace FluentValidation.Configuration.Internal
{
    /// <summary>
    /// Generic validator without references to specific types.
    /// Could be used for build general validation rules.
    /// </summary>
    /// <typeparam name="T">Type that would be validated by validator.</typeparam>
    internal class GenericValidator<T> : AbstractValidator<T>
    {
    }
}
