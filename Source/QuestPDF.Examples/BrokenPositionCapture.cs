using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using QuestPDF.Elements;
using QuestPDF.Examples.Engine;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Examples
{
    public class ShowCapturedDimensions : IDynamicComponent
    {
        private readonly string Location;

        public ShowCapturedDimensions(string location)
        {
            Location = location;
        }

        public DynamicComponentComposeResult Compose(DynamicContext context)
        {
            var location = context.GetElementCapturedLocations(Location).FirstOrDefault();

            if (location == null)
            {
                return new DynamicComponentComposeResult
                {
                    Content = context.CreateElement(container => container.Text("Location not found").FontSize(12).FontColor(Colors.Red.Medium)),
                    HasMoreContent = false
                };
            }

            var content = context.CreateElement(container =>
            {
                container
                    .Text($"{Location}: {location.Width}x{location.Height}");
            });

            return new DynamicComponentComposeResult
            {
                Content = content,
                HasMoreContent = false
            };
        }
    }

    public static class BrokenPositionCaptureExample
    {
        [Test]
        public static void Test()
        {
            RenderingTest
                .Create()
                .PageSize(PageSizes.A4)
                .ShowResults()
                .ProducePdf()
                .Render(container =>
                {
                    container
                        .Background(Colors.White)
                        .Padding(25)
                        .Width(250)
                        .Column(column =>
                        {
                            column.Item().PaddingBottom(10).Text("Testing elements:").FontSize(20);
                            column.Item().CaptureLocation("ok1").Width(10).Height(10).Border(1);
                            column.Item().CaptureLocation("ok2").ExtendHorizontal().Height(10).Border(1);
                            column.Item().CaptureLocation("brokenBox").Height(10).Border(1);
                            column.Item().CaptureLocation("brokenText").Text("Hello, World!").FontSize(12);

                            column.Item().Height(20);

                            column.Item().PaddingBottom(10).Text("Captured locations:").FontSize(20);
                            column.Item().Dynamic(new ShowCapturedDimensions("ok1"));
                            column.Item().Dynamic(new ShowCapturedDimensions("ok2"));
                            column.Item().Dynamic(new ShowCapturedDimensions("brokenBox"));
                            column.Item().Dynamic(new ShowCapturedDimensions("brokenText"));
                        });
                });
        }
    }
}