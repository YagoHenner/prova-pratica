using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Infrastructure.Services;

public class ServicoEnvioArquivosMinio : IServicoEnvioArquivosMinio
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _options;
    private readonly ILogger<ServicoEnvioArquivosMinio> _logger;

    public ServicoEnvioArquivosMinio(
        IMinioClient minioClient,
        IOptions<MinioOptions> options,
        ILogger<ServicoEnvioArquivosMinio> logger)
    {
        _minioClient = minioClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadAsync(IFormFile arquivo, string nomeArquivoUnico)
    {
        try
        {
            var nomeBucket = _options.BucketName;

            // 1. Verifica se o bucket existe, senão cria
            var beArgs = new BucketExistsArgs().WithBucket(nomeBucket);
            bool encontrado = await _minioClient.BucketExistsAsync(beArgs);
            if (!encontrado)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(nomeBucket);
                await _minioClient.MakeBucketAsync(mbArgs);
                _logger.LogInformation("Bucket '{NomeBucket}' criado.", nomeBucket);

                // Opcional: Tornar o bucket público
                // var policy = ... (json policy)
                // var spArgs = new SetPolicyArgs().WithBucket(nomeBucket).WithPolicy(policy);
                // await _minioClient.SetPolicyAsync(spArgs);
            }

            // 2. Faz o upload
            using var stream = arquivo.OpenReadStream();
            var putArgs = new PutObjectArgs()
                .WithBucket(nomeBucket)
                .WithObject(nomeArquivoUnico)
                .WithStreamData(stream)
                .WithObjectSize(arquivo.Length)
                .WithContentType(arquivo.ContentType);

            await _minioClient.PutObjectAsync(putArgs, CancellationToken.None);

            _logger.LogInformation("Arquivo '{NomeArquivo}' salvo no bucket '{NomeBucket}'.", nomeArquivoUnico, nomeBucket);

            // 3. Retorna a URL (Endpoint + Bucket + Nome)
            // Esta URL pode não ser pública por padrão, depende da política do bucket
            return $"{_options.Endpoint}/{nomeBucket}/{nomeArquivoUnico}";
        }
        catch (MinioException e)
        {
            _logger.LogError(e, "Erro MinIO ao fazer upload do arquivo {NomeArquivo}.", nomeArquivoUnico);
            throw new InvalidOperationException("Erro no serviço de storage.", e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro inesperado ao fazer upload do arquivo {NomeArquivo}.", nomeArquivoUnico);
            throw;
        }
    }

    public async Task DeleteAsync(string nomeArquivoUnico)
    {
        try
        {
            var nomeBucket = _options.BucketName;

            var rmArgs = new RemoveObjectArgs()
                .WithBucket(nomeBucket)
                .WithObject(nomeArquivoUnico);

            await _minioClient.RemoveObjectAsync(rmArgs, CancellationToken.None);

            _logger.LogInformation("Arquivo '{NomeArquivo}' deletado do bucket '{NomeBucket}'.", nomeArquivoUnico, nomeBucket);
        }
        catch (MinioException e)
        {
            _logger.LogError(e, "Erro MinIO ao deletar o arquivo {NomeArquivo}.", nomeArquivoUnico);
            throw new InvalidOperationException("Erro no serviço de storage.", e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro inesperado ao deletar o arquivo {NomeArquivo}.", nomeArquivoUnico);
            throw;
        }
    }
}