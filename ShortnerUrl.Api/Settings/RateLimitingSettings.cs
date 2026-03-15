namespace ShortnerUrl.Api.Settings;

public class RateLimitingSettings
{
    public LoginRateLimitSettings Login { get; set; } = new();
    public RefreshTokenRateLimitSettings RefreshToken { get; set; } = new();
    public ApiRateLimitSettings Api { get; set; } = new();
} 

public class LoginRateLimitSettings
{
    /// <summary>
    /// Numero maximo de tentativas de login permitidas na janela de tempo
    /// </summary>
    public int PermitLimit { get; set; } = 5;
    
    /// <summary>
    /// Janela de tempo em segundos
    /// </summary>
    public int WindowSeconds { get; set; } = 60;
    
    /// <summary>
    /// Numero de segmentos na janela deslizante
    /// </summary>
    public int SegmentsPerWindow { get; set; } = 2;
}

public class RefreshTokenRateLimitSettings
{
    public int PermitLimit { get; set; } = 10;
    public int WindowSeconds { get; set; } = 60;
    public int SegmentsPerWindow { get; set; } = 2;
}

public class ApiRateLimitSettings
{
    /// <summary>
    /// Limite maximo de tokens no bucket
    /// </summary>
    public int TokenLimit { get; set; } = 100;
    
    /// <summary>
    /// Tokens adicionados por periodo
    /// </summary>
    public int TokensPerPeriod { get; set; } = 20;
    
    /// <summary>
    /// Periodo de reposicao em segundos
    /// </summary>
    public int ReplenishmentPeriodSeconds { get; set; } = 10;
    
    /// <summary>
    /// Limite da fila de espera
    /// </summary>
    public int QueueLimit { get; set; } = 2;
}   