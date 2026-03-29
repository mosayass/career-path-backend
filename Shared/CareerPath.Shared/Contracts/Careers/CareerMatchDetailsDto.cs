namespace CareerPath.Shared.Contracts.Careers;

public record CareerMatchDetailsDto(
    int CareerId,
    string Title,
    string Sector,
    string Description,
    string EducationLevel,
    IReadOnlyList<string> CoreSkills,
    IReadOnlyList<AlternativeCareerDto> AlternativeMatches
);