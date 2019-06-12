using System;
using FluentValidation;
using DDD.Domain.Entities;
namespace DDD.Service.Validators
{
    public class CategoriaValidator : AbstractValidator<Categoria>
{
	public CategoriaValidator()
        {
	    RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Can't found the object.");
                });
		
        }
			}
}