using System;
using FluentValidation;
using DDD.Domain.Entities;
namespace DDD.Service.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
{
	public ProdutoValidator()
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

            RuleFor(c => c.Preco)
                .NotNull().WithMessage("Is necessary to inform the Price.")
                .Must(Preco => Preco >= 0).WithMessage("Price must be greater or equal to zero.");

            RuleFor(c => c.SKU)
                .NotEmpty().WithMessage("Is necessary to inform the SKU.")
                .NotNull().WithMessage("Is necessary to inform the birth SKU.");
        }
			}
}