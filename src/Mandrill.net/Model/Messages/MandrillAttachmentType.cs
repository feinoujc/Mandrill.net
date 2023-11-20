using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Mandrill.Model
{

    public enum MandrillAttachmentType
    {
        Jpg,
        Jpeg,
        Png,
        Gif,
        Bmp,
        Tiff,
        Ico,
        Pdf,
        Doc,
        Docx,
        Xls,
        Xlsx,
        Ppt,
        Pptx,
        Txt,
        Html,
        Css,
        Js,
        Xml,
        Json,
        Zip,
        Rar,
        Tar,
        Gz,
        _7z,
        Mp3,
        Wav,
        Mp4,
        Avi,
        // Agrega más extensiones según sea necesario
    }
    public class MandrillAttachmentMime
    {
        public static string GetMimeType(MandrillAttachmentType mime) => _mimeTypes[mime.ToString()];
        static Dictionary<string, string> _mimeTypes = new Dictionary<string, string>{
            { "Jpg", "image/jpeg" },
            { "Jpeg", "image/jpeg" },
            { "Png", "image/png" },
            { "Gif", "image/gif" },
            { "Bmp", "image/bmp" },
            { "Tiff", "image/tiff" },
            { "Ico", "image/x-icon" },
            { "Pdf", "application/pdf" },
            { "Doc", "application/msword" },
            { "Docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "Xls", "application/vnd.ms-excel" },
            { "Xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "Ppt", "application/vnd.ms-powerpoint" },
            { "Pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "Txt", "text/plain" },
            { "Html", "text/html" },
            { "Css", "text/css" },
            { "Js", "application/javascript" },
            { "Xml", "application/xml" },
            { "Json", "application/json" },
            { "Zip", "application/zip" },
            { "Rar", "application/x-rar-compressed" },
            { "Tar", "application/x-tar" },
            { "Gz", "application/gzip" },
            { "_7z", "application/x-7z-compressed" },
            { "Mp3", "audio/mpeg" },
            { "Wav", "audio/wav" },
            { "Mp4", "video/mp4" },
            { "Avi", "video/x-msvideo" }
      };

    }
}
