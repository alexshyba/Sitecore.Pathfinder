﻿namespace Sitecore.Pathfinder.Projects.Files
{
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.TextDocuments;

  public class ContentFile : File
  {
    public ContentFile([NotNull] IProject project, [NotNull] IDocumentSnapshot documentSnapshot) : base(project, documentSnapshot)
    {
    }
  }
}