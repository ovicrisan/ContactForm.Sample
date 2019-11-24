using FluentValidation;

namespace ContactForm.Sample.Models
{
    public class ContactModelValidator: AbstractValidator<ContactModel>
    {
        public ContactModelValidator()
        {
            RuleFor(x => x.ContactName).NotEmpty().WithMessage("Contact name required")
                .MaximumLength(50).WithMessage("Contact name too long.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email address invalid")
                .MaximumLength(50).WithMessage("Email too long.");
            RuleFor(x => x.Phone).MaximumLength(20).WithMessage("Phone too long.");
            RuleFor(x => x.Category).MaximumLength(30).WithMessage("Phone too long.");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Subject required")
                .MaximumLength(50).WithMessage("Subject too long.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message required")
                .MaximumLength(1500).WithMessage("Message too long.");
        }
    }
}
