using eCommerceDs.DTOs;
using FluentValidation;

namespace eCommerceDs.Validators
{
    /// <summary>
    /// Validator for GroupInsertDTO
    /// </summary>
    public class GroupInsertValidator : AbstractValidator<GroupInsertDTO>
    {
        public GroupInsertValidator()
        {
            // Validate NameGroup
            RuleFor(x => x.NameGroup)
                .NotEmpty().WithMessage("The group name is required.")
                .Length(2, 100).WithMessage("The group name must be between 2 and 100 characters.");

            // Validate ImageUrl
            When(x => !string.IsNullOrEmpty(x.ImageUrl), () => 
            {
                RuleFor(x => x.ImageUrl)
                    .Must(BeAValidImgurUrl).WithMessage("The URL must be from Imgur (e.g.: https://i.imgur.com/example.jpg)");
            });

            // Validate MusicGenreId
            RuleFor(x => x.MusicGenreId)
                .GreaterThan(0).WithMessage("The music genre ID is required.");
        }

        private bool BeAValidImgurUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;
                
            // Validate Imgur URL
            string pattern = @"^https?://(i\.)?imgur\.com/.*\.(jpg|jpeg|png|gif)$";
            return System.Text.RegularExpressions.Regex.IsMatch(url, pattern, 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}
