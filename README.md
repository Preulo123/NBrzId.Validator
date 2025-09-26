# Brazilian Identifiers Validator Library

This library provides validation methos for Brazilian identifiers in .NET applications.

## Table of Contents
- [Overview](#overview)
- [Getting Started](#getting-started)
- [Installation](#installation)
- [Usage](#usage)
- [Compatibility](#compatibility)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)

## Overview

This library provides a set of tools and interfaces for validating Brazilian identifiers. The main components are:
- **`IBrzIdentifierValidator<T>`**: Defines the contract for validators of Brazilian identifiers. Implementations of this interface are expected to apply validation logic tailored to types that implement the `NBrzId.Common.IBrzIdentifier` interface.
- **`IBrzValidator`**: This interface provides methods that simplify the validation of identifiers without requiring direct instantiation of specific validator classes. It includes specialized methods for common identifier types.
- **`BrzValidations`**: This static helper class provides direct validation methods for common identifier types, making it convenient to validate these identifiers without requiring dependency injection or service configuration.

## Getting Started

### Installation

To include this library in your .NET project, add it as a dependency via NuGet:

```sh
dotnet add package NBrzId.Validator --version 1.0.0
```

## Usage

The simplest way to validate Brazilian identifier is through the `BrzValidations` static class:

```csharp
Console.WriteLine("Enter a Brazilian identifier: ");

var identifierValue = Console.ReadKey();

var isValidCnpj = BrzValidations.CheckCnpj(identifierValue);
var isValidCpf = BrzValidations.CheckCpf(identifierValue, removeFormatters: false, pad: true);

Console.WriteLine($"Valid CNPJ? {isValidCnpj}");
Console.WriteLine($"Valid CPF? {isValidCpf}");
```

You can also leverage the validators with dependency injection. For this, add a reference to the package `NBrzId.Validator.DependencyInjection`.

```csharp
//Dependency injection setup
private static void ConfigureServices(IServiceCollection serviceDescriptors)
{
    //Registers pre-built identifier types and their validators
    serviceDescriptors.UseBrzValidators();
}

//Actual injection and usage of the CPF validator
[ApiController]
[Route("api/{controller}")]
public class MyApiController : ControllerBase
{
    private readonly IBrzIdentifierValidator<Cpf> _cpfValidator;

    public MyApiController(IBrzIdentifierValidator<Cpf> cpfValidator)
    {
        _cpfValidator = cpfValidator;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
    {
        // Applied validation here!
        if (_cpfValidator.ApplyValidation(updatedCustomer.Cpf, removeFormatters: false, pad: true))
        {
            return BadRequest("Invalid CPF was provided");
        }

        //[...]

        return Ok("Updated");
    }
}

//Injection and usage of IBrzValidator, which contains operation to validate multiple identifier types
public sealed class MyService
{
    private readonly IBrzValidator _validator;

    public MyService(IBrzIdentifierValidator<Cpf> validator)
    {
        _validator = validator;
    }

    public void CheckUserType(User user)
    {
        var userDocument = user.DocumentNumber;

        if (_validator.ValidateCnpj(userDocument))
        {
            user.Type = "Company";
        }
        else if (_validator.ValidateCpf(userDocument))
        {
            user.Type = "Individual";
        }
        else
        {
            user.Type = "Unknown";
        }
    }
}
```

## Compatibility

This library targets .NET Standard 2.0, ensuring compatibility with .NET Core, .NET 5/6/7/8, and .NET Framework 4.6.1+.

## Roadmap

This library is in active development. Planned features for future releases include additional Brazilian identifiers validations as needed

## Contributing

Contributions are welcome! Please open an issue to report bugs or discuss improvements.

## License

This project is licensed under the MIT License. See the LICENSE.md file for details.

