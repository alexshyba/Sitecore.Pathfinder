﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System.Diagnostics;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Projects.References
{
    [DebuggerDisplay("{GetType().Name,nq}: {ReferenceText}")]
    public class Reference : IReference
    {
        private bool _isValid;

        public Reference([NotNull] IProjectItem owner, [NotNull] SourceProperty<string> sourceProperty, [NotNull] string referenceText)
        {
            // the reference text might be different from the source property value. 
            // e.g. the source property value might be a list of guids while the reference text is a single Guid.
            Owner = owner;
            SourceProperty = sourceProperty;
            TextNode = sourceProperty.SourceTextNode ?? Snapshots.TextNode.Empty;
            ReferenceText = referenceText;
        }

        public Reference([NotNull] IProjectItem owner, [NotNull] ITextNode textNode, [NotNull] string referenceText)
        {
            // the reference text might be different from the source property value. 
            // e.g. the source property value might be a list of guids while the reference text is a single Guid.
            Owner = owner;
            SourceProperty = null;
            TextNode = textNode;
            ReferenceText = referenceText;
        }

        public bool IsResolved { get; set; }

        public bool IsValid
        {
            get
            {
                if (!IsResolved)
                {
                    Resolve();
                }

                return _isValid;
            }

            protected set
            {
                _isValid = value;
            }
        }

        public IProjectItem Owner { get; }

        public SourceProperty<string> SourceProperty { get; }

        public ITextNode TextNode { get; }

        public string ReferenceText { get; }

        [CanBeNull]
        protected IProjectItemUri ResolvedUri { get; set; }

        public void Invalidate()
        {
            IsResolved = false;
            IsValid = false;
            ResolvedUri = null;
        }

        public virtual IProjectItem Resolve()
        {
            if (IsResolved && ResolvedUri != null)
            {
                if (!IsValid)
                {
                    return null;
                }

                var result = Owner.Project.FindQualifiedItem<IProjectItem>(ResolvedUri);
                if (result == null)
                {
                    IsValid = false;
                    ResolvedUri = null;
                }

                return result;
            }

            IsResolved = true;

            var projectItem = Owner.Project.FindQualifiedItem<IProjectItem>(ReferenceText);
            if (projectItem == null)
            {
                IsValid = false;
                ResolvedUri = null;
                return null;
            }

            ResolvedUri = projectItem.Uri;
            IsValid = true;

            return projectItem;
        }
    }
}
