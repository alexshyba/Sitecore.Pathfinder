﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.IO;
using System.Text.RegularExpressions;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Documents;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.Projects;

namespace Sitecore.Pathfinder.IO
{
    public static class PathHelper
    {
        [NotNull]
        public static string Combine([NotNull] string path1, [NotNull] string path2)
        {
            var f1 = NormalizeFilePath(path1);
            var f2 = NormalizeFilePath(path2);

            if (string.IsNullOrEmpty(f1) || f1 == ".")
            {
                return f2;
            }

            if (string.IsNullOrEmpty(f2) || f2 == ".")
            {
                return f1;
            }

            var path = Path.Combine(f1, f2);

            if (path.IndexOf("..", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                path = Path.GetFullPath(path);
            }

            if (f1.IndexOf(Path.VolumeSeparatorChar) < 0 && f2.IndexOf(Path.VolumeSeparatorChar) < 0)
            {
                var n = path.IndexOf(Path.VolumeSeparatorChar);
                if (n >= 0)
                {
                    path = path.Mid(n + 1);
                }
            }

            return path;
        }

        public static bool CompareFiles([NotNull] string sourceFileName, [NotNull] string destinationFileName)
        {
            var fileInfo1 = new FileInfo(sourceFileName);
            var fileInfo2 = new FileInfo(destinationFileName);

            if (!fileInfo1.Exists && !fileInfo2.Exists)
            {
                return true;
            }

            if (!fileInfo2.Exists)
            {
                return false;
            }

            if (!fileInfo1.Exists)
            {
                return false;
            }

            // var lastWrite1 = new DateTime(fileInfo1.LastWriteTimeUtc.Year, fileInfo1.LastWriteTimeUtc.Month, fileInfo1.LastWriteTimeUtc.Day, fileInfo1.LastWriteTimeUtc.Hour, fileInfo1.LastWriteTimeUtc.Minute, fileInfo1.LastWriteTimeUtc.Second);
            // var lastWrite2 = new DateTime(fileInfo2.LastWriteTimeUtc.Year, fileInfo2.LastWriteTimeUtc.Month, fileInfo2.LastWriteTimeUtc.Day, fileInfo2.LastWriteTimeUtc.Hour, fileInfo2.LastWriteTimeUtc.Minute, fileInfo2.LastWriteTimeUtc.Second);
            if (fileInfo1.LastWriteTimeUtc != fileInfo2.LastWriteTimeUtc)
            {
                return false;
            }

            if (fileInfo1.Length != fileInfo2.Length)
            {
                return false;
            }

            return true;
        }

        [NotNull]
        public static string GetDirectoryAndFileNameWithoutExtensions([NotNull] string fileName)
        {
            fileName = NormalizeFilePath(fileName);
            var n = fileName.LastIndexOf('\\');
            if (n < 0)
            {
                n = 0;
            }

            n = fileName.IndexOf('.', n);
            if (n < 0)
            {
                return fileName;
            }

            return fileName.Left(n);
        }

        [NotNull]
        public static string GetFileNameWithoutExtensions([NotNull] string fileName)
        {
            var s = NormalizeFilePath(fileName).LastIndexOf('\\');
            if (s < 0)
            {
                var e0 = fileName.IndexOf('.');
                return e0 < 0 ? fileName : fileName.Left(e0);
            }

            var e1 = fileName.IndexOf('.', s);
            return e1 < 0 ? fileName.Mid(s + 1) : fileName.Mid(s + 1, e1 - s - 1);
        }

        [NotNull]
        public static string GetFilePath([NotNull] IProject project, [NotNull] ISourceFile sourceFile)
        {
            var filePath = "/" + NormalizeItemPath(UnmapPath(project.Options.ProjectDirectory, sourceFile.FileName)).TrimStart('/');

            foreach (var pair in project.Options.RemapFileDirectories)
            {
                var n = filePath.IndexOf(pair.Key, StringComparison.OrdinalIgnoreCase);

                if (n >= 0)
                {
                    filePath = filePath.Left(n) + pair.Value + filePath.Mid(n + pair.Key.Length);
                }
            }

            return filePath;
        }

        [NotNull]
        public static string GetItemName([NotNull] ISourceFile sourceFile)
        {
            var fileName = NormalizeItemPath(sourceFile.FileName);

            var s = fileName.LastIndexOf('/') + 1;
            var e = fileName.IndexOf('.', s);

            if (e < 0)
            {
                return fileName.Mid(s);
            }

            return fileName.Mid(s, e - s);
        }

        [NotNull]
        public static string GetItemParentPath([NotNull] string itemPath)
        {
            itemPath = NormalizeItemPath(itemPath).TrimEnd('/');

            var n = itemPath.LastIndexOf('/');

            return n >= 0 ? itemPath.Left(n) : itemPath;
        }

        [NotNull]
        public static string GetItemPath([NotNull] IProject project, [NotNull] ISourceFile sourceFile)
        {
            var itemPath = UnmapPath(project.Options.ProjectDirectory, sourceFile.FileName);

            itemPath = GetDirectoryAndFileNameWithoutExtensions(itemPath);

            itemPath = "/" + NormalizeItemPath(itemPath);

            if (!itemPath.StartsWith("/sitecore/", StringComparison.OrdinalIgnoreCase))
            {
                itemPath = "/sitecore" + itemPath;
            }

            return itemPath;
        }

        public static bool IsQualifiedName([NotNull] string name)
        {
            return name.IndexOf('/') > 0;
        }

        public static bool MatchesPattern([NotNull] string fileName, [NotNull] string pattern)
        {
            var s = Path.GetFileName(fileName);

            var regex = "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";

            return Regex.IsMatch(s, regex, RegexOptions.IgnoreCase);
        }

        [NotNull]
        public static string NormalizeFilePath([NotNull] string filePath)
        {
            return filePath.Replace("/", "\\").TrimEnd('\\');
        }

        [NotNull]
        public static string NormalizeItemPath([NotNull] string filePath)
        {
            return filePath.Replace("\\", "/").TrimEnd('/');
        }

        [NotNull]
        public static string UnmapPath([NotNull] string rootPath, [NotNull] string fileName)
        {
            rootPath = NormalizeFilePath(rootPath).TrimEnd('\\');
            fileName = NormalizeFilePath(fileName);

            if (fileName.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
            {
                return fileName.Mid(rootPath.Length).TrimStart('\\');
            }

            return fileName;
        }
    }
}
