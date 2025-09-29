using EMS.API.Models;
using EMS.Application.Features.Employees.DTOs;
using FluentValidation;

namespace EMS.API.Validators;
public class CreateEmployeeValidator: AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(255).WithMessage("El email no puede exceder 255 caracteres");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("El puesto es requerido")
            .MaximumLength(100).WithMessage("El puesto no puede exceder 100 caracteres");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("El departamento es requerido")
            .MaximumLength(100).WithMessage("El departamento no puede exceder 100 caracteres");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("El salario debe ser mayor a 0")
            .LessThanOrEqualTo(1000000).WithMessage("El salario no puede exceder $1,000,000");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de contratación no puede ser futura");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }

}
public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID inválido");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(255).WithMessage("El email no puede exceder 255 caracteres");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("El puesto es requerido")
            .MaximumLength(100).WithMessage("El puesto no puede exceder 100 caracteres");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("El departamento es requerido")
            .MaximumLength(100).WithMessage("El departamento no puede exceder 100 caracteres");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("El salario debe ser mayor a 0")
            .LessThanOrEqualTo(1000000).WithMessage("El salario no puede exceder $1,000,000");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de contratación no puede ser futura");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
public class EmployeeQueryValidator : AbstractValidator<EmployeeQuery>
{
    public EmployeeQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("La página debe ser mayor o igual a 1");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("El tamaño de página debe estar entre 1 y 100");

        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0).WithMessage("El salario mínimo debe ser mayor o igual a 0")
            .When(x => x.SalaryMin.HasValue);

        RuleFor(x => x.SalaryMax)
            .GreaterThan(x => x.SalaryMin).WithMessage("El salario máximo debe ser mayor al mínimo")
            .When(x => x.SalaryMin.HasValue && x.SalaryMax.HasValue);

        RuleFor(x => x.OrderDirection)
            .Must(x => x.Equals("asc", StringComparison.CurrentCultureIgnoreCase) || x.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
            .WithMessage("La dirección de ordenamiento debe ser 'asc' o 'desc'");
    }
}
