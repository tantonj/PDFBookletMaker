This program is designed to allow anyone to easily modify an existing pdf of any number of pages to pdf ready to print to a booklet for book binding projects.

Provided in releases is the Linux and Windows builds. 3 parameters are required:<br />
```-source``` (```-s```) the location of the source pdf on your machine<br />
```-dest``` (```-d```) the output location of the finished result<br />
```-bsize``` (```-b```) the number of pages you desire for each booklet. This MUST be a multiple of 4 and a postive integer.<br />

e.g.,<br /> 
Linux: ```./PDFBookletMaker -s "sourcefile.pdf" -d "output.pdf" -b 12``` will process sourcefile.pdf int a 12 page booklet pdf named output.pdf<br />
Windows ```PDFBookletMaker -s "sourcefile.pdf" -d "output.pdf" -b 12``` will process sourcefile.pdf int a 12 page booklet pdf named output.pdf<br />

Once you run this command the application will process the pdf and determine if extra pages are needed. It will prompt you if the needed pages added to front and pack is acceptable. 

For example a 70 page document using a booklet size of 12 pages would require 6 booklets. However 6*12=72. So the application would ask if you would like to add 1 blank page to the front and 1 to the back. If you respond with Y then the application will add those pages and rearrange the page order appropriately. If you say no the program finishes with no action.

Resulting PDF can then be printed using a duplex printer. Choose 2 pages per side/sheet and use Short-edge binding when printing for a booklet. I recommend printing the first 4 pages (1st folio) to test that your booklets are printing correctly.

To run this on any platform, install dotnet and use the command:<br/>
```dotnet PDFBookletMaker.dll -s "sourcefile.pdf" -d "output.pdf" -b 12``` with the appropriate parameters modified.

I created this application for my own purposes very quickly. Excellent Object Oriented principles were not followed. Feel free to modify as you see fit.

This was developed and built in .net 6 because I was too lazy to update. Code changes may be necessary if you are trying to build with a newer version of .net.

Good luck Sewing!
