using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Document = QuestPDF.Fluent.Document;

QuestPDF.Settings.License = LicenseType.Community;

Document.Create(container =>
{
    //Page 1
    container.Page(page =>
    {
        //Declare Size first
        page.Size(PageSizes.A4);

        //Standard Header Text with Background Colour added
        page.Header()
            .Background(Colors.Blue.Lighten5)
            .Text("Hello World!")
            .SemiBold()
            .FontSize(30)
            .FontColor(Colors.Red.Darken1);

        //Demonstration of some Text and Image content within Document main body
        page.Content()
            .PaddingVertical(5, Unit.Millimetre)
            .Column(column =>
            {
                column.Item()
                    .Text(Placeholders.LoremIpsum());

                column.Item()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Image(Placeholders.Image(200, 100));

                column.Item()
                    .Background(Colors.Amber.Lighten1)
                    .PaddingVertical(1, Unit.Centimetre)
                    .Text("Sample Text Here")
                    .Bold()
                    .Italic()
                    .FontSize(15)
                    .FontColor(Colors.Green.Medium)
                    .FontFamily(Fonts.Calibri);
            });

        //Add Footer with Page Number
        page.Footer()
            .PaddingBottom(1, Unit.Centimetre)
            .AlignCenter()
            .Text(_ =>
            {
                _.Span("Page ");
                _.CurrentPageNumber();
            });
    });

    //Page 2
    container.Page(page =>
    {
        //Declare size again
        page.Size(PageSizes.A4);

        //Title
        page.Header()
            .AlignCenter()
            .Text("Table Example")
            .SemiBold().FontSize(30).FontColor(Colors.Blue.Medium);


        //Use the 'Element' function to apply styling that we have scoped to another Method.
        page.Content().Element(TableContent);

        //Our custom method, called by .Element() above
        void TableContent(IContainer container)
        {
            //Adding sample Table with generated data
            container.PaddingVertical(25).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    //Define columns
                    columns.ConstantColumn(50);
                    columns.ConstantColumn(200);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                //Define Text for Columns
                table.Header(header =>
                {
                    header.Cell().AlignCenter().Text("#");
                    header.Cell().AlignCenter().Text("Product Name");
                    header.Cell().AlignCenter().Text("Quantity");
                    header.Cell().AlignCenter().Text("Price");
                    header.Cell().AlignCenter().Text("Total Price");
                });

                //Generate random data to populate table
                foreach (var i in Enumerable.Range(1, 25))
                {
                    var quantity = Placeholders.Random.Next(1, 10);
                    var price = Placeholders.Random.NextDouble() * 100;
                    var total = quantity * price;

                    table.Cell().AlignCenter().Text(i.ToString());
                    table.Cell().AlignCenter().Text(Placeholders.Label());
                    table.Cell().AlignCenter().Text(quantity.ToString());
                    table.Cell().AlignCenter().Text($"{price:F2}");
                    table.Cell().AlignCenter().Text($"{total:F2}");
                }
            });
        }

        //Add Footer with Page Number
        page.Footer()
            .PaddingBottom(1, Unit.Centimetre)
            .AlignCenter()
            .Text(_ =>
            {
                _.Span("Page ");
                _.CurrentPageNumber();
            });
    });
}).ShowInPreviewer();

Console.ReadLine();

//For generating PDF
//document.GeneratePdf("test.pdf");