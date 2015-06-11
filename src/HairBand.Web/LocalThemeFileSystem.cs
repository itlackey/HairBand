using DotLiquid;
using DotLiquid.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HairBand.Web
{
    public class LocalThemeFileSystem : DotLiquid.FileSystems.IFileSystem
    {

        public string Root { get; set; }

        public LocalThemeFileSystem(string root)
        {
            Root = root;
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            string templatePath = (string)context[templateName];

            string fullPath = FullPath(templatePath);

            if (!File.Exists(fullPath))
                throw new FileSystemException(GetTemplateNotFoundMessage(), templatePath);

            return File.ReadAllText(fullPath);
        }


        public string FullPath(string templatePath)
        {
            if (templatePath == null || !Regex.IsMatch(templatePath, @"^[^.\/][a-zA-Z0-9_\/]+$"))
                throw new FileSystemException(GetIllegalTemplateNameMessage(), templatePath);

            string fullPath = templatePath.Contains("/")
                ? Path.Combine(Path.Combine(Root, Path.GetDirectoryName(templatePath)), string.Format("_{0}.liquid", Path.GetFileName(templatePath)))
                : Path.Combine(Root, string.Format("_{0}.liquid", templatePath));


            //if the liquid file is not there, try looking for an html file
            if (!File.Exists(fullPath))
            {
                fullPath = templatePath.Contains("/")
                    ? Path.Combine(Path.Combine(Root, Path.GetDirectoryName(templatePath)), string.Format("{0}.html", Path.GetFileName(templatePath)))
                    : Path.Combine(Root, string.Format("{0}.html", templatePath));

            }

            string escapedPath = Root.Replace(@"\", @"\\").Replace("(", @"\(").Replace(")", @"\)");

            if (!Regex.IsMatch(Path.GetFullPath(fullPath), string.Format("^{0}", escapedPath)))
                throw new FileSystemException(GetIllegalTemplatePathMessage(), Path.GetFullPath(fullPath));

            return fullPath;
        }


        private static string GetTemplateNotFoundMessage()
        {
            return "Template not found";
            //return Liquid.ResourceManager.GetString("LocalFileSystemTemplateNotFoundException");
        }

        private static string GetIllegalTemplatePathMessage()
        {
            //return Liquid.ResourceManager.GetString("LocalFileSystemIllegalTemplatePathException");
            return "Invalid template path";
        }

        private static string GetIllegalTemplateNameMessage()
        {
            //return Liquid.ResourceManager.GetString("LocalFileSystemIllegalTemplateNameException");
            return "Invalid template name";
        }
    }
}
