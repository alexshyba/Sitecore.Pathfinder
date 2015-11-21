﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System;
using System.IO;
using System.Linq;
using Sitecore.Pathfinder.Building;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Projects;
using Sitecore.Pathfinder.Projects.Items;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.T4.CodeGeneration
{
    public class T4IFileCodeGenerator : T4GeneratorBase
    {
        public override void Generate(IBuildContext context, IProject project)
        {
            var itmNameToken = context.Configuration.Get(Constants.Configuration.GenerateCodeNameToken);

            foreach (var fileName in context.FileSystem.GetFiles(context.ProjectDirectory, "*.tt", SearchOption.AllDirectories))
            {
                if (fileName.EndsWith(".project.tt", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (Ignore(fileName))
                {
                    continue;
                }

                var itemType = fileName.Left(fileName.Length - 3);
                var n = itemType.LastIndexOf('.');
                if (n < 0)
                {
                    continue;
                }

                itemType = itemType.Mid(n + 1).ToLowerInvariant();

                var typeName = context.Configuration.GetString("generate-code:items:" + itemType);
                if (string.IsNullOrEmpty(typeName))
                {
                    context.Trace.TraceWarning("T4 item type not found in the config setting 'generate-code:items'", PathHelper.UnmapPath(context.ProjectDirectory, fileName));
                    continue;
                }

                n = typeName.IndexOf(',');
                if (n < 0)
                {
                    context.Trace.TraceWarning("T4 item type must include assembly name", typeName);
                    continue;
                }

                var assemblyName = typeName.Mid(n + 1).Trim();
                typeName = typeName.Left(n).Trim();

                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => string.Equals(Path.GetFileName(a.Location), assemblyName, StringComparison.OrdinalIgnoreCase));
                if (assembly == null)
                {
                    context.Trace.TraceInformation("Assembly not found in T4 type name in the config setting 'generate-code:items'", PathHelper.UnmapPath(context.ProjectDirectory, typeName));
                    continue;
                }

                var type = assembly.GetType(typeName, false, true);
                if (type == null)              
                {
                    context.Trace.TraceInformation("T4 type name not found in the config setting 'generate-code:items'", PathHelper.UnmapPath(context.ProjectDirectory, typeName));
                    continue;
                }

                context.Trace.TraceInformation("Generating code", PathHelper.UnmapPath(context.ProjectDirectory, fileName));

                foreach (var projectItem in project.ProjectItems.Where(i => i.GetType() == type || i.GetType().IsSubclassOf(type)))
                {
                    var itemBase = projectItem as ItemBase;
                    if (itemBase != null && itemBase.IsExtern)
                    {
                        continue;
                    }

                    var host = GetHost(context, project);

                    host.Item = projectItem;

                    var targetFileName = fileName.Left(fileName.Length - 3 - itemType.Length - 1);
                    targetFileName = targetFileName.Replace(itmNameToken, projectItem.ShortName);

                    try
                    {
                        if (!host.ProcessTemplate(fileName, targetFileName))
                        {
                            TraceErrors(context, host, fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Trace.TraceError(ex.Message, fileName, TextSpan.Empty);
                    }
                }
            }
        }
    }
}
