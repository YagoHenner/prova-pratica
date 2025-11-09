using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IServicoEnvioArquivosMinio
{
    /// <summary>
    /// Faz upload de um arquivo para o storage (ex: MinIO, S3).
    /// </summary>
    /// <param name="arquivo">O arquivo (IFormFile) recebido.</param>
    /// <param name="nomeArquivoUnico">O nome do arquivo a ser salvo (ex: guid.jpg).</param>
    /// <returns>A URL pública ou de acesso do arquivo salvo.</returns>
    Task<string> UploadAsync(IFormFile arquivo, string nomeArquivoUnico);

    /// <summary>
    /// Exclui um arquivo do storage.
    /// </summary>
    /// <param name="nomeArquivoUnico">O nome do arquivo a ser deletado (ex: guid.jpg).</param>
    Task DeleteAsync(string nomeArquivoUnico);
}