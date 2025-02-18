using FluentValidation;
using Golub.Requests;

namespace Golub.Endpoints.Validators
{
    /// <summary>
    /// Validator for <see cref="SendEmailRequest"/>
    /// </summary>
    public class SendEmailRequestValidator : AbstractValidator<SendEmailRequest>
    {
        public SendEmailRequestValidator()
        {
            RuleFor(x => x.Tos).NotEmpty()
                .WithMessage("To addresses cannot be empty.")
                .Must(emails => emails.All(IsValidEmail))
                .WithMessage("At least one 'To' address is invalid.");

            RuleFor(x => x.Ccs)
                .Must(emails => !emails.Any() || emails.All(IsValidEmail))
                .WithMessage("At least one 'Cc' address is invalid.");

            RuleFor(x => x.Bcc)
                .Must(emails => !emails.Any() || emails.All(IsValidEmail))
                .WithMessage("At least one 'Bcc' address is invalid.");

            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Subject cannot be empty.");

            RuleFor(x => x.PlainTextContent)
                .NotEmpty()
                .WithMessage("Plain text content cannot be empty.")
                .When(x => string.IsNullOrEmpty(x.InnerHtml));

            RuleFor(x => x.InnerHtml)
                .NotEmpty()
                .WithMessage("HTML content cannot be empty.")
                .When(x => string.IsNullOrEmpty(x.PlainTextContent));

            RuleFor(x => x.From)
                .Must(IsValidEmail)
                .WithMessage("From address is not valid");
        }

        /// <summary>
        /// Checks if email address is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false; // if email fails to parse it is not valid and we are returning false
            }
        }
    }
}