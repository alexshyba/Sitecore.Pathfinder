﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Projects.Files
{
    public class ContentFile : File
    {
        public ContentFile([NotNull] IProject project, [NotNull] ISnapshot snapshot, [NotNull] string filePath) : base(project, snapshot, filePath)
        {
        }
    }
}
