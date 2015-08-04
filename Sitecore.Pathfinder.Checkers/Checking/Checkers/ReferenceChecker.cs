﻿using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Checking.Checkers
{
  using System.ComponentModel.Composition;
  using System.Linq;

  [Export(typeof(IChecker))]
  public class ReferenceChecker : CheckerBase
  {
    public override void Check(ICheckerContext context)
    {
      foreach (var projectItem in context.Project.Items)
      {
        foreach (var reference in projectItem.References)
        {
          if (!reference.IsValid)
          {
            context.Trace.TraceWarning("Reference not found", projectItem.Snapshots.First().SourceFile.FileName, reference.SourceAttribute?.Source?.Position ?? TextPosition.Empty, reference.TargetQualifiedName);
          }
        }
      }
    }
  }
}
