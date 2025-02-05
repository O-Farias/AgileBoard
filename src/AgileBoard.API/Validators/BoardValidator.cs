using FluentValidation;
using AgileBoard.API.Models;

namespace AgileBoard.API.Validators
{
    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MinimumLength(3).WithMessage("O nome deve ter no mínimo 3 caracteres")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}