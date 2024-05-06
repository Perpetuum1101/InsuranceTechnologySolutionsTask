using Application.DTOs;
using FluentValidation;

namespace Application.Validation;

public class CoverValidator : AbstractValidator<CoverDTO>
{
    public CoverValidator()
    {
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage(ErrorMessages.START_DATE_MUST_BE_IN_THE_FUTURE);
        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage(ErrorMessages.START_DATE_CANNOT_BE_LATER_THAN_END_DATE);
        RuleFor(x => x.EndDate)
            .Must((cover, endDate) =>
            {
                var result = cover.EndDate.Subtract(cover.StartDate).TotalDays <= 365;
                return result;
            })
            .WithMessage(ErrorMessages.DATE_RANGE_CANNOT_BE_LONGER_THAN_YEAR);
    }
}
