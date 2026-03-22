using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.DTOs;
using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Infrastructure.Clients;

public class FastApiAiModelClient : IAiModelClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public FastApiAiModelClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    public async Task<Result<AiPredictionResponse>> GetPredictionsAsync(
        AssessmentSubmissionPayload payload,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/predict/top-matches",
                payload,
                _jsonOptions,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                // In a production scenario, you would log the exact response.StatusCode here
                return Result<AiPredictionResponse>.Failure($"AI prediction service returned an error: {response.StatusCode}");
            }

            var predictionResult = await response.Content.ReadFromJsonAsync<AiPredictionResponse>(
                _jsonOptions,
                cancellationToken);

            if (predictionResult == null)
            {
                return Result<AiPredictionResponse>.Failure("Failed to deserialize the AI model response.");
            }

            return Result<AiPredictionResponse>.Success(predictionResult);
        }
        catch (HttpRequestException ex)
        {
            // Catches network-level issues (e.g., container down, connection refused)
            return Result<AiPredictionResponse>.Failure($"Network error communicating with AI service: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Catches other unexpected issues
            return Result<AiPredictionResponse>.Failure($"An unexpected error occurred during AI prediction: {ex.Message}");
        }
    }
}