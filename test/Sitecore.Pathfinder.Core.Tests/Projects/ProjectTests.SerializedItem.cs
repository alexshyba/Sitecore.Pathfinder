﻿// © 2015 Sitecore Corporation A/S. All rights reserved.

using System.IO;
using System.Linq;
using NUnit.Framework;
using Sitecore.Pathfinder.Languages.Serialization;
using Sitecore.Pathfinder.Projects.Items;
using Sitecore.Pathfinder.Snapshots;

namespace Sitecore.Pathfinder.Projects
{
    [TestFixture]
    public partial class ProjectTests
    {
        [Test]
        public void SerializedItemTest()
        {
            var projectItem = Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/SerializedItem");
            Assert.IsNotNull(projectItem);
            Assert.AreEqual("SerializedItem", projectItem.ShortName);
            Assert.AreEqual("/sitecore/content/Home/SerializedItem", projectItem.QualifiedName);

            var item = projectItem as Item;
            Assert.IsNotNull(item);
            Assert.AreEqual("SerializedItem", item.ItemName);
            Assert.AreEqual("/sitecore/content/Home/SerializedItem", item.ItemIdOrPath);
            Assert.AreEqual("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}", item.TemplateIdOrPath);
            Assert.IsNotNull(item.ItemNameProperty.SourceTextNodes);
            Assert.IsInstanceOf<TextNode>(item.ItemNameProperty.SourceTextNode);
            Assert.IsInstanceOf<TextNode>(item.TemplateIdOrPathProperty.SourceTextNode);
            Assert.AreEqual("{76036F5E-CBCE-46D1-AF0A-4143F9B557AA}", TraceHelper.GetTextNode(item.TemplateIdOrPathProperty).Value);

            var field = item.Fields.FirstOrDefault(f => f.FieldName == "Text");
            Assert.IsNotNull(field);
            Assert.AreEqual("Pip 2", field.Value);
            Assert.IsInstanceOf<TextNode>(field.ValueProperty.SourceTextNode);
            Assert.AreEqual("Pip 2", field.ValueProperty.SourceTextNode?.Value);

            var textDocument = projectItem.Snapshots.First() as ITextSnapshot;
            Assert.IsNotNull(textDocument);
        }

        [Test]
        public void WriteSerializedItemTest()
        {
            var item = Project.Items.FirstOrDefault(i => i.QualifiedName == "/sitecore/content/Home/SerializedItem") as Item;
            Assert.IsNotNull(item);

            var writer = new StringWriter();
            item.WriteAsSerialization(writer);
            var result = writer.ToString();

            var expected = @"----item----
version: 1
id: {CEABE4B1-E915-4904-B396-BBC0C081F111}
database: master
path: /sitecore/content/Home/SerializedItem
parent: 
name: SerializedItem
master: {00000000-0000-0000-0000-000000000000}
template: {76036F5E-CBCE-46D1-AF0A-4143F9B557AA}
templatekey: Sample Item

----field----
field: 
name: __Workflow
key: __workflow
content-length: 38

{A5BC37E7-ED96-4C1E-8590-A26E64DB55EA}
----version----
language: en
version: 1
revision: 

----field----
field: 
name: Title
key: title
content-length: 5

Pip 1
----field----
field: 
name: Text
key: text
content-length: 5

Pip 2
----field----
field: 
name: __Created
key: __created
content-length: 16

20150602T111618Z
----field----
field: 
name: __Created by
key: __created by
content-length: 14

sitecore\admin
----field----
field: 
name: __Revision
key: __revision
content-length: 36

9ac1cdd2-40d5-466e-b3d6-00797bfc0e62
----field----
field: 
name: __Updated
key: __updated
content-length: 35

20150602T111625:635688405856685957Z
----field----
field: 
name: __Updated by
key: __updated by
content-length: 14

sitecore\admin
----field----
field: 
name: __Workflow state
key: __workflow state
content-length: 38

{190B1C84-F1BE-47ED-AA41-F42193D9C8FC}
";
            Assert.AreEqual(expected, result);
        }
    }
}