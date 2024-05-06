using Domain.Types;

namespace Application.DTOs;

public record ClaimDTO(
              Guid Id,
              Guid CoverId,
              DateTime Created,
              string Name,
              ClaimType Type,
              decimal DamageCost);