using Microsoft.AspNetCore.Mvc;

using bifeldy_sd3_mbz_60.Models;
using bifeldy_sd3_lib_60.Services;

namespace bifeldy_sd3_mbz_60.Controllers {

    [ApiController]
    [Route("attachment")]
    public class AttachmentController : ControllerBase {

        private readonly IBerkasService _berkas;

        public AttachmentController(IBerkasService berkas) {
            _berkas = berkas;
        }

        [HttpGet("{id}")]
        public IActionResult DownloadFile(string id) {
            try {
                if (string.IsNullOrEmpty(id)) {
                    throw new Exception("Data Tidak Lengkap!");
                }

                string filePath = Path.Combine(_berkas.TempFolderPath, id);
                if (!System.IO.File.Exists(filePath)) {
                    return NotFound(new {
                        info = $"🙄 404 - {GetType().Name} :: Download Berkas 😪",
                        result = new {
                            message = "File Tidak Ditemukan!"
                        }
                    });
                }

                var stream = System.IO.File.OpenRead(filePath);
                return File(stream, "application/octet-stream", id, true);
            }
            catch (Exception ex) {
                return BadRequest(new {
                    info = $"🙄 400 - {GetType().Name} :: Download Berkas 😪",
                    result = new {
                        message = ex.Message
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] Attachment model) {
            if (model?.file == null || model.file.Length <= 0) {
                return BadRequest(new {
                    info = $"🙄 400 - {GetType().Name} :: Gagal Upload Berkas 😪",
                    result = new {
                        message = "Data Tidak Lengkap!"
                    }
                });
            }

            // Generate a unique filename for the uploaded file
            string uniqueFileName = Path.GetRandomFileName();
            string filePath = Path.Combine(_berkas.TempFolderPath, uniqueFileName);

            // Save the uploaded file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                await model.file.CopyToAsync(fileStream);
            }

            var fi = new FileInfo(filePath);
            return Ok(new {
                info = $"😅 200 - {GetType().Name} :: Berhasil Upload Berkas 🤣",
                result = new {
                    name = fi.Name,
                    size = fi.Length,
                    created = fi.CreationTime,
                    description = model.description
                }
            });
        }
    }

}
