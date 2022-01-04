using Core.DataTransferObjects.Project;

using FluentValidation;

namespace WebApi.Validators.Project
{
    public sealed class UpdateProjectDTOValidator : AbstractValidator<UpdateProjectDTO>
    {
        public UpdateProjectDTOValidator()
        {
            RuleFor(p => p.Deadline)
                .GreaterThan(System.DateTime.Now)
                    .WithMessage("You can not set deadline to passed date");
        }
    }
}
