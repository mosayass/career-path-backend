using System;

namespace CareerPath.Shared.Contracts.Careers;

public record CareerSummaryDto(
    Guid CareerId,
    string Title,
    string Sector);