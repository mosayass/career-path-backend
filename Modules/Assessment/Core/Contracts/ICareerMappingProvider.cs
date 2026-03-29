    using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Core.Contracts;

public interface ICareerMappingProvider
{
    Result<string> GetCareerName(int jobLabel);
}