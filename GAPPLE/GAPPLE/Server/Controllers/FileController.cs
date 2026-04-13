using System.Security.Claims;
using GAPPLE.Server.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GAPPLE.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _uploadFolder = @"C:\ZentraFiles";

        public FileController()
        {
            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);
        }

        [HttpPost("chunk/{entidad}/{subEntidad}/{fileName}/{chunkIndex:int}")]
        public async Task<IActionResult> UploadChunk(string entidad, string subEntidad, string fileName, int chunkIndex)
        {
            try
            {
                var carpetaEntidad = Path.Combine(_uploadFolder, entidad);

                if (!Directory.Exists(carpetaEntidad))
                    Directory.CreateDirectory(carpetaEntidad);

                var carpetaSubEntidad = Path.Combine(carpetaEntidad, subEntidad);

                if (!Directory.Exists(carpetaSubEntidad))
                    Directory.CreateDirectory(carpetaSubEntidad);

                string baseName = Path.Combine(carpetaSubEntidad, $"{fileName}");

                if (chunkIndex == 0)
                {
                    // borrar cualquier resto de subida previa (chunks y archivo final)
                    var oldChunks = Directory.GetFiles(_uploadFolder, $"{Path.GetFileName(baseName)}.part.*");
                    foreach (var chunk in oldChunks)
                        System.IO.File.Delete(chunk);

                    if (System.IO.File.Exists(baseName))
                        System.IO.File.Delete(baseName);
                }

                string chunkPath = $"{baseName}.part.{chunkIndex}";

                await using var fs = new FileStream(chunkPath, FileMode.Create, FileAccess.Write);
                await Request.Body.CopyToAsync(fs);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpPost("complete/{entidad}/{subEntidad}/{fileName}/{chunksTotales:int}")]
        public IActionResult CompleteUpload(string entidad, string subEntidad, string fileName, int chunksTotales )
        {
            string finalFilePath = null;
            try
            {
                string baseName = Path.Combine(_uploadFolder, entidad, subEntidad, $"{fileName}");
                finalFilePath = baseName; // archivo final

                // recolecto todos los chunk paths
                var chunkFiles = Enumerable.Range(0, chunksTotales)
                    .Select(i => $"{baseName}.part.{i}")
                    .ToList();

                // validar que estén todos
                if (!chunkFiles.All(System.IO.File.Exists))
                    throw new InvalidOperationException("Faltan chunks, no se puede completar el archivo.");

                // concatenar en orden
                using (var fsFinal = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write))
                {
                    foreach (var chunkFile in chunkFiles)
                    {
                        using var fsChunk = new FileStream(chunkFile, FileMode.Open, FileAccess.Read);
                        fsChunk.CopyTo(fsFinal);
                    }
                }

                // limpiar
                chunkFiles.ForEach(System.IO.File.Delete);

                return Ok(finalFilePath);
            }
            catch (Exception ex)
            {
                DeleteFile(finalFilePath);
                return StatusCode(500, ex.ToString());
            }
        }

        internal byte[] GetFile(string fullPath)
        {
            if (fullPath == null || !System.IO.File.Exists(fullPath))
                throw new Exception("Ha ocurrido un error en la lectura del archivo");

            return System.IO.File.ReadAllBytes(fullPath);
        }

        internal void DeleteFile(string fullPath)
        {
            if (fullPath != null && System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        [HttpPost("template/download")]
        public IActionResult GetTemplateArchivo(List<string> columns)
        {
            try
            {
                return new Export().ToExcelHeaders(columns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
