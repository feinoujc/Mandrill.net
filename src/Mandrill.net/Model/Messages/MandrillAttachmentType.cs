using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Mandrill.Model
{

    public enum MandrillAttachmentType
    {
        jpg,
        jpeg,
        png,
        gif,
        bmp,
        tiff,
        ico,
        pdf,
        doc,
        docx,
        xls,
        xlsx,
        ppt,
        pptx,
        txt,
        html,
        css,
        js,
        xml,
        json,
        zip,
        rar,
        tar,
        gz,
        _7z,
        mp3,
        wav,
        mp4,
        avi,
        // Agrega más extensiones según sea necesario
    }
    public class MandrillAttachmentMime
    {
        public static string GetMimeType(MandrillAttachmentType mime) => _mimeTypes[mime.ToString()];
         static Dictionary<string, string> _mimeTypes = new Dictionary<string, string>{
            { "jpg", "image/jpeg" },
            { "jpeg", "image/jpeg" },
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "bmp", "image/bmp" },
            { "tiff", "image/tiff" },
            { "ico", "image/x-icon" },
            { "pdf", "application/pdf" },
            { "doc", "application/msword" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "xls", "application/vnd.ms-excel" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "ppt", "application/vnd.ms-powerpoint" },
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "txt", "text/plain" },
            { "html", "text/html" },
            { "css", "text/css" },
            { "js", "application/javascript" },
            { "xml", "application/xml" },
            { "json", "application/json" },
            { "zip", "application/zip" },
            { "rar", "application/x-rar-compressed" },
            { "tar", "application/x-tar" },
            { "gz", "application/gzip" },
            { "7z", "application/x-7z-compressed" },
            { "mp3", "audio/mpeg" },
            { "wav", "audio/wav" },
            { "mp4", "video/mp4" },
            { "avi", "video/x-msvideo" }
            };
        
    }
}
