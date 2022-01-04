using Core.DataTransferObjects.Project;

using FluentValidation;

namespace WebApi.Validators.Project
{
    public sealed class CreateProjectDTOValidator : AbstractValidator<CreateProjectDTO>
    {
        public CreateProjectDTOValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Project should have a name")
                .MinimumLength(3)
                    .WithMessage("Project's name should be minimum 3 chars long")
                .MaximumLength(15)
                    .WithMessage("Project's name should be maximum 15 chars long");

            RuleFor(p => p.Deadline)
                .GreaterThan(System.DateTime.Now)
                    .WithMessage("You can not set deadline to passed date");
        }
    }
}
