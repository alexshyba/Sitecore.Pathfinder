﻿namespace Sitecore.Pathfinder.Parsing
{
  using System.ComponentModel.Composition;
  using Microsoft.Framework.ConfigurationModel;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.IO;
  using Sitecore.Pathfinder.Projects;

  public interface IParseContext
  {
    [NotNull]
    ICompositionService CompositionService { get; }

    [NotNull]
    IConfiguration Configuration { get; }

    [NotNull]
    string DatabaseName { get; }

    [NotNull]
    string ItemName { get; }

    [NotNull]
    string ItemPath { get; }

    [NotNull]
    IProject Project { get; }

    [NotNull]
    ISourceFile SourceFile { get; }

    [NotNull]
    string GetRelativeFileName([NotNull] ISourceFile sourceFile);

    [NotNull]
    IParseContext Load([NotNull] IProject project, [NotNull] ISourceFile sourceFile);

    [NotNull]
    string ReplaceTokens([NotNull] string text);
  }
}
