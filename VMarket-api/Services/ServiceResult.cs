namespace VMarket_api.Services;

public record ServiceResult(bool Success, string? Information = null, string[]? Errors = null);
