using Application.DTOs;
using Application.Repository.Contracts;
using FluentValidation;

namespace Application.Validation;

public class ClaimsValidator : AbstractValidator<ClaimDTO>
{
    public ClaimsValidator(ICoverRepository coverRepository)
    {
        RuleFor(x => x.DamageCost)
            .LessThan(100000)
            .WithMessage(ErrorMessages.DAMAGE_COST_EXCEEDS_100000);
        RuleFor(x => x.Created)
            .MustAsync(async (claim, created, _) =>
            {
                var cover = await coverRepository.Get(claim.CoverId);
                if (cover == null)
                {
                    return false;
                }

                var withingDateRange = created >= cover.StartDate && created <= cover.EndDate;
                return withingDateRange;
            })
            .WithMessage(ErrorMessages.CREATE_DATE_NOT_IN_COVER_DATE_RANGR);
    }
}
