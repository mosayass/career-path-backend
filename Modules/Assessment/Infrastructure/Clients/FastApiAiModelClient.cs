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

    public async Task<AiPredictionResponse> GetPredictionsAsync(
        AssessmentSubmissionPayload payload,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/predict/top-matches",
            payload,
            _jsonOptions,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"AI prediction service returned an error: {response.StatusCode}");
        }

        var predictionResult = await response.Content.ReadFromJsonAsync<AiPredictionResponse>(
            _jsonOptions,
            cancellationToken);

        if (predictionResult == null)
        {
            throw new InvalidOperationException("Failed to deserialize the AI model response.");
        }

        return predictionResult;
    }
}