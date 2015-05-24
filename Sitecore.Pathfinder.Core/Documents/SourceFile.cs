﻿namespace Sitecore.Pathfinder.Documents
{
  using System;
  using System.Diagnostics;
  using System.Xml.Linq;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.IO;
  using Sitecore.Pathfinder.Projects;

  [DebuggerDisplay("{GetType().Name}: FileName={FileName}")]
  public class SourceFile : ISourceFile
  {
    public SourceFile([NotNull] IFileSystemService fileSystem, [NotNull] string fileName)
    {
      this.FileSystem = fileSystem;
      this.FileName = fileName;

      this.LastWriteTimeUtc = this.FileSystem.GetLastWriteTimeUtc(this.FileName);
    }

    [NotNull]
    public static ISourceFile Empty { get; } = new EmptySourceFile();

    public string FileName { get; }

    public bool IsModified { get; set; }

    public DateTime LastWriteTimeUtc { get; }

    [NotNull]
    protected IFileSystemService FileSystem { get; }

    public string GetFileNameWithoutExtensions()
    {
      return PathHelper.GetDirectoryAndFileNameWithoutExtensions(this.FileName);
    }

    public string GetProjectPath(IProject project)
    {
      return PathHelper.UnmapPath(project.Options.ProjectDirectory, this.FileName);
    }

    public string[] ReadAsLines()
    {
      return this.FileSystem.ReadAllLines(this.FileName);
    }

    public string ReadAsText()
    {
      var contents = this.FileSystem.ReadAllText(this.FileName);
      return contents;
    }

    public XElement ReadAsXml()
    {
      var contents = this.ReadAsText();

      XDocument doc;
      try
      {
        doc = XDocument.Parse(contents, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
      }
      catch
      {
        return null;
      }

      return doc.Root;
    }
  }
}