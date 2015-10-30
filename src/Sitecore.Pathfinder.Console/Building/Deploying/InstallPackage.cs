// � 2015 Sitecore Corporation A/S. All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;

namespace Sitecore.Pathfinder.Building.Deploying
{
    public class InstallPackage : RequestTaskBase
    {
        public InstallPackage() : base("install-package")
        {
        }

        public override void Run(IBuildContext context)
        {
            if (context.Project.HasErrors)
            {
                context.Trace.TraceInformation(Texts.Package_contains_errors_and_will_not_be_deployed);
                context.IsAborted = true;
                return;
            }

            context.Trace.TraceInformation(Texts.Installing___);

            var failed = false;

            foreach (var fileName in context.OutputFiles)
            {
                var packageId = Path.GetFileNameWithoutExtension(fileName);
                if (string.IsNullOrEmpty(packageId))
                {
                    continue;
                }

                var queryStringParameters = new Dictionary<string, string>
                {
                    ["w"] = "0",
                    ["rep"] = packageId
                };


                var url = MakeUrl(context, context.Configuration.GetString(Constants.Configuration.InstallUrl), queryStringParameters);
                if (!Request(context, url))
                {
                    failed = true;
                }
                else
                {
                    context.Trace.TraceInformation("Installed", Path.GetFileName(fileName));
                }
            }

            if (failed)
            {
                return;
            }

            foreach (var snapshot in context.Project.Items.SelectMany(i => i.Snapshots))
            {
                snapshot.SourceFile.IsModified = false;
            }
        }

        public override void WriteHelp(HelpWriter helpWriter)
        {
            helpWriter.Summary.Write("Unpacks and installs the project package (including dependencies) in the website.");
        }
    }
}