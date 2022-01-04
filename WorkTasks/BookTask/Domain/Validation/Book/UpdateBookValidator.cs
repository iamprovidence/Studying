using Domain.DataTransferObjects.Book;

using FluentValidation;

namespace Domain.Validation.Book
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookDTO>
    {
        public UpdateBookValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                    .WithMessage("Book's title is required");
        }
    }
}
