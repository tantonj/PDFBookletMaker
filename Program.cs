using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Ocsp;

namespace PDFBookletMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args != null && args.Length == 6) {
                if(!(args.Contains("-source") || args.Contains("-s")) || !(args.Contains("-dest") || args.Contains("-d")) || !(args.Contains("-bsize") || args.Contains("-b"))) {
                    printHelp();
                    return;
                }
                string source = "", dest = "";
                int bookletSize = 0;
                for(int i = 0; i < 5; i+=2) {
                    if(args[i].Equals("-source") || args[i].Equals("-s")) {
                        source = args[i+1];
                    }
                    if(args[i].Equals("-dest") || args[i].Equals("-d")) {
                        dest = args[i+1];
                    }
                    if(args[i].Equals("-bsize") || args[i].Equals("-b")) {
                        try {
                            bookletSize = Convert.ToInt32(args[i+1]);
                            if(bookletSize < 4 || bookletSize%4 != 0) {
                                Console.WriteLine("Booklet Size must be a positive integer and multiple of 4");
                                return;
                            }
                        }
                        catch {
                            Console.WriteLine("Booklet Size must be a positive integer and multiple of 4");
                            return;
                        }
                    }
                }
                try {
                    PdfReader reader = new PdfReader(source);
                    int booklets = reader.NumberOfPages / bookletSize;
                    int pagesNeeded = reader.NumberOfPages % bookletSize;
                    pagesNeeded = bookletSize - pagesNeeded;
                    if(pagesNeeded > 0)
                        booklets++;
                    int addToFront = 0;
                    int addToBack = 0;
                    if(pagesNeeded % 2 == 0) {
                        addToFront = pagesNeeded/2;
                        addToBack = pagesNeeded/2;
                    }
                    else {
                        addToFront = (pagesNeeded-1)/2;
                        addToBack = ((pagesNeeded-1)/2)+1;
                    }
                    Console.WriteLine($"This will create {booklets} booklets");
                    Console.WriteLine($"{pagesNeeded} pages must be added for a booklet size of {bookletSize}.");
                    char response = 't';
                    do {
                        Console.WriteLine($"Would you like to add {addToFront} blank pages to the Front and {addToBack} blank pages to the Back? (Y/N)");
                        response = Console.ReadKey().KeyChar;
                    }while(response != 'y' && response != 'n' && response != 'Y' && response != 'N');
                    if(response == 'n' || response == 'N') {
                        Console.WriteLine("\nGoodbye!");
                        return;
                    }
                    else {
                        Console.WriteLine($"\nConverting {source} to {dest} with a booklet size of {bookletSize}");
                        string tempfile = $"temp-{Guid.NewGuid().ToString()}.pdf";
                        using(var tempStream = new FileStream(tempfile, FileMode.Create)) {
                            PdfStamper stamper = new PdfStamper(reader, tempStream);
                            for(int j = 0; j < addToFront; j++) {
                                stamper.InsertPage(0, PageSize.A4);
                            }
                            for(int j = 0; j < addToFront; j++) {
                                stamper.InsertPage(reader.NumberOfPages+1, PageSize.A4);
                            }
                            stamper.Close();
                        }
                        List<int> pageOrder = new List<int>();
                        int count = 0;
                        int n = (booklets * bookletSize)-1;
                        int i = 0;
                        do {
                            pageOrder.Add(n+1);
                            pageOrder.Add(i+1);
                            n--;
                            i++;
                            pageOrder.Add(i+1);
                            pageOrder.Add(n+1);
                            n--;
                            i++;
                        }while(i < n);
                        PdfReader reader2 = new PdfReader(tempfile);
                        using(var destDocStream = new FileStream(dest, FileMode.Create)) {
                            var pdfConcat = new PdfConcatenate(destDocStream);
                            reader2.SelectPages(pageOrder);
                            pdfConcat.AddPages(reader2);
                            reader2.Close();
                            pdfConcat.Close();
                        }
                        File.Delete(tempfile);
                        Console.WriteLine($"resulting pdf can be found at {dest}.");
                        Console.WriteLine("When printing result use 2 page per side option and duplex printing on short edge");
                    }
                    reader.Close();
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else {
                printHelp();
            }
        }

        static private void printHelp() {
            Console.WriteLine("Invalid Arguments! Must have all 3 parameters");
            Console.WriteLine("-source (-s)\t\t\tsource document location");
            Console.WriteLine("-dest (-d)\t\t\tdestination document location");
            Console.WriteLine("-bsize (-b)\t\t\tbooklet size (multiple of 4)");
        }
    }
}