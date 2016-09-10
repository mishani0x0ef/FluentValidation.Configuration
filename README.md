# FluentValidation.Configuration
Configuration functionality for validators based on FluentValidation.

[NuGet Gallery | FluentValidation.Configuration](https://www.nuget.org/packages/FluentValidation.Configuration)

This library represent simple way to manage validators that you have created with the great [FluentValidation] (https://github.com/JeremySkinner/FluentValidation). 
The main idea is to provide you with possibility to register and resolve validators based on type you want to validate.

## Example

Lets assume you have created few validators with FluentValidation:

```c#
using FluentValidation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name).Length(10, 50);
        RuleFor(c => c.Address).NotEmpty();
    }
}

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(c => c.Number).NotEmpty();
    }
}
```

With FluentValidation.Configuration you can easily register all your validators. You also can create new validator using fluent syntax without creating separate class. Just create your own implementation of IValidationProfile (use _AbstractValidationProfile_ which taking care of trivial things and will let you concentrate your attention on validators registration):

```c#
using FluentValidation.Configuration;

public class ValidationProfile : AbstractValidationProfile
{
    public ValidationProfile()
    {
        // Register existing validators.
        Configuration.Register(new CustomerValidator());
        Configuration.Register(new OrderValidator());

        // Specify validation rules for User using fluent syntax.
        Configuration.Register<User>()
            .RuleFor(u => u.Name, rule => rule.Length(5, 50))
            .RuleFor(u => u.Email, rule => rule.EmailAddress());
    }
}
```

When you want to get validator for some type - it could be resolved from validation configuration:

```c#
User user = new User();

IValidationProfile profile = new ValidationProfile();
IValidationConfiguration config = profile.Configuration;
IValidator<User> validator = config.GetValidator<User>();

validator.Validate(user);
```
