﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using Sitecore.Pathfinder.Diagnostics;

namespace Sitecore.Pathfinder.Documents
{
    public interface ISnapshot
    {
        bool IsModified { get; set; }

        [NotNull]
        ISourceFile SourceFile { get; }

        void SaveChanges();
    }
}
