﻿namespace Sitecore.Pathfinder.Projects.Files
{
  using System.Diagnostics;
  using Sitecore.Pathfinder.Diagnostics;
  using Sitecore.Pathfinder.Projects.Items;
  using Sitecore.Pathfinder.TextDocuments;

  public class MediaFile : File
  {
    public MediaFile([NotNull] IProject project, [NotNull] IDocumentSnapshot documentSnapshot, [NotNull] Item mediaItem) : base(project, documentSnapshot)
    {
      this.MediaItem = mediaItem;

      Debug.Assert(this.MediaItem.Owner == null, "Owner is already set");
      this.MediaItem.Owner = this;
    }

    [NotNull]
    public Item MediaItem { get; }
  }
}
