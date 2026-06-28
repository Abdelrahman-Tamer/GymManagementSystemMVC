using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.BLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(IFormFile? file, string folderName, CancellationToken ct = default);
        bool Delete(string fileName, string folderName);
        (Stream stream, string ContentType)? GetFile(string fileName, string folderName);
    }
}
