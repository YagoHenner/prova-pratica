namespace Infrastructure.Services;

/// <summary>
/// Classe usada para fazer bind (mapear) as configurações
/// da seção "Minio" do appsettings.json.
/// </summary>
public class MinioOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
}