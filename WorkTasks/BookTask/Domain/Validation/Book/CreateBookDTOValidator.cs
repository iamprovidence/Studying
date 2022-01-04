using Domain.DataTransferObjects.Book;

using FluentValidation;

namespace Domain.Validation.Book
{
    public class CreateBookDTOValidator : AbstractValidator<CreateBookDTO>
    {
        public CreateBookDTOValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                    .WithMessage("Book's title is required");
        }
    }
}
