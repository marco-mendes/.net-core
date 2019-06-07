using System;
using FluentValidation;
using DDD.Domain.Entities;
namespace DDD.Service.Validators
{
    public class FabricanteValidator : AbstractValidator<Fabricante>
{
	public FabricanteValidator()
        {
	    RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Can't found the object.");
                });
		
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Is necessary to inform the Name.")
                .NotNull().WithMessage("Is necessary to inform the Name.");

            RuleFor(c => c.Codigo)
                .NotEmpty().WithMessage("Is necessary to inform the Codigo.")
                .NotNull().WithMessage("Is necessary to inform the Codigo.");

        }
			}
}