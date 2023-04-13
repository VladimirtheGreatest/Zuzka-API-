using FluentValidation;
using FluentValidation.Results;
using Zuzka.Services.Contracts;
using Zuzka.Services.DTO;

namespace Zuzka.Services
{
    public class ValidationService : IValidationService
    {
        public ValidationResult Validate<T>(T obj)
        {
            var validator = new InlineValidator<T>();
            ConfigureRules(validator);

            var result = validator.Validate(obj);
            return new ValidationResult(result.Errors);
        }

        protected virtual void ConfigureRules<T>(AbstractValidator<T> validator)
        {
            // Define default validation rules for all objects
            validator.RuleFor(x => x).NotNull();

            // Define validation rules for the DocumentRequestDto properties more can be added later
            if (typeof(T) == typeof(DocumentRequestDto))
            {
                validator.RuleFor(x => (x as DocumentRequestDto).PublishedYear).GreaterThan(1900).WithMessage(Zuzka.Data.Configuration.Constants.publishedYearValidationMessage);
                validator.RuleFor(x => (x as DocumentRequestDto).Rating).InclusiveBetween(0, 100).WithMessage(Zuzka.Data.Configuration.Constants.ratingValidationMessage);
                validator.RuleFor(x => (x as DocumentRequestDto).Tags).NotNull().WithMessage(Zuzka.Data.Configuration.Constants.tagsNotNullValidationMessage);
                validator.RuleFor(x => (x as DocumentRequestDto).Tags).Must(x => x.All(tag => !string.IsNullOrWhiteSpace(tag))).WithMessage(Zuzka.Data.Configuration.Constants.tagsFormatValidationMessage);
                validator.RuleFor(x => (x as DocumentRequestDto).Tags).Must(x => x.Length <= 100).WithMessage(Zuzka.Data.Configuration.Constants.tagsCountValidationMessage);
            }
        }
    }

    public class ValidationResult
    {
        public IEnumerable<ValidationError> Errors { get; }

        public bool IsValid => !Errors.Any();

        public ValidationResult(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }

        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.Select(failure => new ValidationError(failure.PropertyName, failure.ErrorMessage));
        }
    }

    public class ValidationError
    {
        public string PropertyName { get; }
        public string ErrorMessage { get; }

        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
