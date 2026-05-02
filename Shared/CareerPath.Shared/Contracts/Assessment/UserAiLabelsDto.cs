namespace CareerPath.Shared.Contracts.Assessment;

public record UserAiLabelsDto(
    int PrimaryAiLabelId,
    List<int> SecondaryAiLabelIds
);