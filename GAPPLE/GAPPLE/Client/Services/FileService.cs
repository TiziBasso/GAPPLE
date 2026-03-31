using GAPPLE.Client.Entities;
using GAPPLE.Client.Tools;
using GAPPLE.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Services
{
    public class FileService
    {
        [Inject]
        private HttpClient httpClient { get; set; }
        [Inject]
        private SesionDTO sesionDTO { get; }

        [Inject]
        private IJSFunction js { get; set; }

        public FileService(HttpClient httpClient, IJSFunction js, SesionDTO sesionDTO) => (this.httpClient, this.sesionDTO, this.js) = (httpClient, sesionDTO, js);

        private const string REQUEST_URI_BASE = "/api/file";

        /// <summary>
        /// Envia un archivo por chunks y ejecuta la funcion porcentaje en cada envio
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Porcentaje"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<Response> PostChunkFile(IBrowserFile file, Action<int> Porcentaje, CancellationTokenSource cancellationToken)
        {
            Response response = new(HttpStatusCode.OK);
            long totalRead = 0;
            long totalSize = file.Size;
            int chunkSize = 1024 * 1024 * 5;//5MB por chunk

            using var stream = file.OpenReadStream(maxAllowedSize: file.Size + 1024); // o tu MaxSize
            byte[] buffer = new byte[chunkSize];
            int bytesRead;
            int chunkIndex = 0;

            while ((bytesRead = await stream.ReadAsync(buffer)) > 0 && response.IsOk)
            {
                var chunkContent = new ByteArrayContent(buffer, 0, bytesRead);

                // Endpoint que recibe los chunks
                var r = await httpClient.PostAsync(
                    $"{REQUEST_URI_BASE}/chunk/{sesionDTO.Nombre}/{file.Name}/{chunkIndex}",
                    chunkContent, cancellationToken.Token
                );
                response.HttpStatusCode = r.StatusCode;

                totalRead += bytesRead;
                chunkIndex++;

                Porcentaje?.Invoke((int)((double)totalRead / totalSize * 100));
            }

            if (response.IsOk)
                response.Data = chunkIndex--;

            return response;
        }

        /// <summary>
        /// Junta todos los chunks del archivo en uno solo y devuelve el Path en el que se encuentra el archivo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="chunksTotales"></param>
        /// <returns></returns>
        public async ValueTask<Response> PostCompleteFile(string fileName, int chunksTotales)
        {
            var response = await httpClient.PostAsync($"{REQUEST_URI_BASE}/complete/{sesionDTO.Nombre}/{fileName}/{chunksTotales}", null);
            return new(response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        public async ValueTask DownloadTemplate(List<string> columns, string nombreArchivo = "Template")
        {
            var response = await httpClient.PostAsJsonAsync($"{REQUEST_URI_BASE}/template/download", columns);
            await js.DownloadFile(await response.Content.ReadAsByteArrayAsync(), $"{nombreArchivo}.xlsx");
        }
    }
}
