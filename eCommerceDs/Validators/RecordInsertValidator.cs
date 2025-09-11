using eCommerceDs.DTOs;
using FluentValidation;

namespace eCommerceDs.Validators
{
    public class RecordInsertValidator : AbstractValidator<RecordInsertDTO>
    {
        public RecordInsertValidator()
        {
            // TitleRecord validation
            RuleFor(x => x.TitleRecord)
                .NotEmpty().WithMessage("The title is required")
                .Length(2, 100).WithMessage("The title must be between 2 and 100 characters");
            
            // YearOfPublication validation
            RuleFor(x => x.YearOfPublication)
                .InclusiveBetween(1900, 2100).WithMessage("The publication year must be between 1900 and 2100.");
            
            // ImageUrl validation
            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("The image URL is required")
                .Must(BeAValidImgurUrl).WithMessage("The URL must be from Imgur (e.g.: https://i.imgur.com/example.jpg)");
            
            // Price validation
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("The price must be greater than zero");
            
            // Stock validation
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
            
            // GroupId validation
            RuleFor(x => x.GroupId)
                .GreaterThan(0).WithMessage("The group ID is required");
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
