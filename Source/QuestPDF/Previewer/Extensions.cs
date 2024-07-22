using System.Linq;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Previewer;

internal static class PreviewerModelExtensions
{
    public static PreviewerCommands.UpdateDocumentStructure.DocumentHierarchyElement ExtractHierarchy(this Element container)
    {
        var layoutTree = container.ExtractElementsOfType<LayoutProxy>().Single();
        return Traverse(layoutTree);
        
        PreviewerCommands.UpdateDocumentStructure.DocumentHierarchyElement Traverse(TreeNode<LayoutProxy> node)
        {
            var layout = node.Value;
            
            if (layout.Child is Container or SnapshotRecorder or ElementProxy)
                return Traverse(node.Children.Single());
            
            var element = new PreviewerCommands.UpdateDocumentStructure.DocumentHierarchyElement
            {
                ElementType = layout.Child.GetType().Name.PrettifyName(),
                
                PageLocations = layout.Snapshots,
                
                IsSingleChildContainer = layout.Child is ContainerElement,
                Properties = layout
                    .Child
                    .GetElementConfiguration()
                    .Select(x => new PreviewerCommands.UpdateDocumentStructure.ElementProperty
                    {
                        Label = x.Property,
                        Value = x.Value
                    })
                    .ToList(),
                
                Children = node.Children.Select(Traverse).ToList()
            };

            return element;
        }
    }
}