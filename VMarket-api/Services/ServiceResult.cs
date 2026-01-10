namespace VMarket_api.Services;

public record ServiceResult(bool Success, Object? Data = null, string[]? Errors = null);
