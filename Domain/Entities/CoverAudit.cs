﻿using Domain.Entities.Contracts;

namespace Domain.Entities;

public class CoverAudit : IAudit
{
    public int Id { get; set; }

    public string? CoverId { get; set; }

    public DateTime Created { get; set; }

    public string? HttpRequestType { get; set; }
}
