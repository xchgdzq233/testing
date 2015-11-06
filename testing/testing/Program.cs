using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace testing
{
    class Program
    {
        private static Thread thread1;
        private static Thread thread2;
        private static CancellationToken cancelToken;

        //[STAThread]
        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            //testing merging tiff
            //List<string> lstImages = new List<string>();
            //lstImages.Add(@"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\100110.1");
            //lstImages.Add(@"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\100110.2");
            //string strDestinationFileName = @"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\merged.tif";

            //ImageCodecInfo codec = null;

            //foreach (ImageCodecInfo cCodec in ImageCodecInfo.GetImageEncoders())
            //{
            //    if (cCodec.CodecName == "Built-in TIFF Codec")
            //    {
            //        codec = cCodec;
            //        break;
            //    }
            //}

            //using (EncoderParameters imagePararms = new EncoderParameters(1))
            //{
            //    imagePararms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);

            //    using (System.Drawing.Image destinationImage = (System.Drawing.Image)(new Bitmap(lstImages[0])))
            //    {
            //        destinationImage.Save(strDestinationFileName, codec, imagePararms);
            //        imagePararms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);

            //        for (int i = 1; i < lstImages.Count; i++)
            //        {
            //            using (System.Drawing.Image img = (System.Drawing.Image)(new Bitmap(lstImages[i])))
            //            {
            //                destinationImage.SaveAdd(img, imagePararms);
            //            }
            //        }

            //        imagePararms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
            //        destinationImage.SaveAdd(imagePararms);
            //    }
            //}


            //testing corrupted tiff
            //try
            //{
            //    Tiff.SetErrorHandler(new MyTiffErrorHandler());
            //    using (Tiff image = Tiff.Open(@"C:\Users\janetxue\Downloads\JIT\0409201.DuplicatedImages\20150407.000268\20150407.000268.14.tif", "r"))
            //    {
            //        int numberOfDirectories = image.NumberOfDirectories();
            //        for (int i = 0; i < numberOfDirectories; ++i)
            //        {
            //            image.SetDirectory((short)i);

            //            int width = image.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            //            int height = image.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

            //            int imageSize = height * width;
            //            int[] raster = new int[imageSize];

            //            if (!image.ReadRGBAImage(width, height, raster, true))
            //            {
            //                Console.WriteLine("Page " + i + " is corrupted");
            //                continue;
            //            }
            //        }
            //    }
            //    Console.WriteLine("Passed");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Oops");
            //}

            //Console.ReadKey();

            //testing corrupted pdf
            //string path1 = @"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\100110.1";
            //string path2 = @"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\100110.2";
            //string path3 = @"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\merged.pdf";
            //string path4 = @"C:\Users\janetxue\Downloads\Migration\testing\Corrupted PDF testing\corrupted.pdf";

            //try
            //{
            //    //FileStream fs = new FileStream(path3, FileMode.Create, FileAccess.Write, FileShare.None);
            //    //Document doc = new Document(PageSize.LETTER, 0, 0, 0, 0);
            //    //PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            //    //doc.Open();

            //    //iTextSharp.text.Image tiff = iTextSharp.text.Image.GetInstance(path1);
            //    //tiff.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
            //    //doc.Add(tiff);

            //    //doc.NewPage();
            //    //tiff = iTextSharp.text.Image.GetInstance(path2);
            //    //tiff.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
            //    //doc.Add(tiff);

            //    //doc.Close();
            //    //fs.Close();

            //    using (FileStream fs = new FileStream(path3, FileMode.Create, FileAccess.Write, FileShare.None))
            //    using (Document doc = new Document(PageSize.LETTER, 0, 0, 0, 0))
            //    {
            //        PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            //        doc.Open();

            //        iTextSharp.text.Image tiff = iTextSharp.text.Image.GetInstance(path1);
            //        tiff.ScaleToFit(doc.PageSize.Width, doc.PageSize.Height);
            //        doc.Add(tiff);
            //        doc.NewPage();
            //    }


            //    PdfReader pdfReader = new PdfReader(path4);
            //    int num = pdfReader.NumberOfPages;
            //    Console.WriteLine(num);
            //    Console.ReadKey();
            //}
            //catch (PdfException e1)
            //{
            //    Console.WriteLine("can't");
            //    Console.ReadKey();
            //}
            //catch (Exception e2)
            //{
            //    Console.WriteLine("gotch ya");
            //    Console.ReadKey();
            //}

            //testing threading
            //AppDomain currentDomain = AppDomain.CurrentDomain;
            //currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            //try
            //{
            //    thread1 = new Thread(new ThreadStart(() => initDatabase("1st thread")));
            //    thread2 = new Thread(new ThreadStart(() => initDatabase("2nd thread")));
            //    thread1.Start();
            //    thread2.Start();
            //    Console.WriteLine(thread1.ThreadState);
            //    thread1.Join();
            //    thread2.Join();
            //    //if (thread1.ThreadState.HasFlag(ThreadState.Aborted) || thread2.ThreadState.HasFlag(ThreadState.Aborted))
            //    //{
            //    //    Console.WriteLine("gotcha!");
            //    //}
            //    //else
            //    //{
            //    //    Console.WriteLine("didn't catch it");
            //    //}
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine("catch it");
            //}
            //Console.ReadKey();

            //testing custom exception
            //try
            //{
            //    throw new PageNumMismatchException();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //Console.ReadKey();

            //testing LINQ
            //Func<int, bool> myFunc = x => x == 5;
            //Console.WriteLine(myFunc(5));

            //int[] numbers = { 4, 5, 1, 3, 9, 8, 6, 7, 2, 0 };
            //IEnumerable<int> firstNumbersLessThan6 = numbers.TakeWhile(n => n < 6);

            //foreach (int i in firstNumbersLessThan6)
            //    Console.WriteLine(i);

            //testing miscellaneous
            //IEnumerable<int> squares = Enumerable.Range(1, 10).Select(x => x * x);
            //foreach (int i in squares)
            //    Console.WriteLine(i);

            //testing XML
            //XDocument test = new XDocument(
            //    new XComment("commenting"),
            //    new XElement("Root",
            //        new XElement("child1", "data1"),
            //        new XElement("child2", "data2"),
            //        new XElement("child3",
            //            new XElement("grandChild31", "data31")),
            //        new XElement("child4", "data4")));
            //test.Save(@"C:\Users\janetxue\Downloads\Others\testing\testing.xml");

            //Console.WriteLine(typeof(MyClass));
            //string path = @"C:\Users\janetxue\Downloads\Others\testing\testing.xml";
            //XDocument test2 = new XDocument();
            //test2 = XDocument.Load(path);
            //Console.WriteLine(test2);

            //Console.WriteLine();
            //Console.WriteLine("-------------------START HERE-------------------");
            //Console.WriteLine();

            ////foreach(XElement el in test2.Descendants("child1"))
            ////{
            ////    Console.WriteLine(el.Value + " " + el.Name);
            ////}

            ////Console.WriteLine();
            ////Console.WriteLine("-------------------START HERE-------------------");
            ////Console.WriteLine();
            ////Console.WriteLine(Path.GetDirectoryName(path));

            ////Console.WriteLine(test2.Root.Elements().Any());
            ////Console.WriteLine(test2.Descendants("child2").Descendants().Any());
            ////Console.WriteLine(test2.Descendants("child3").Descendants().Any());
            //Console.WriteLine(test2.Descendants("grandChild31"));

            //testing task
            //CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            //cancelToken = cancelTokenSource.Token;
            //Task t1 = new Task(() => task1());
            //Task t2 = new Task(() => task2());
            //t1.Start();
            //t2.Start();
            ////cancelTokenSource.Cancel();
            //t1.Wait();
            //t2.Wait();
            //Console.WriteLine("ALL COMPLETE");

            //var list = new ConcurrentBag<string>();
            //string[] dirNames = { ".", ".." };
            //List<Task> tasks = new List<Task>();df
            //foreach (var dirName in dirNames)
            //{
            //    Task t = Task.Run(() =>
            //    {
            //        foreach (var path in Directory.GetFiles(dirName))
            //            list.Add(path);
            //    });
            //    tasks.Add(t);
            //}
            //Task.WaitAll(tasks.ToArray());
            //foreach (Task t in tasks)
            //    Console.WriteLine("Task {0} Status: {1}", t.Id, t.Status);

            //Console.WriteLine("Number of files read: {0}", list.Count);

            //testing Regex
            //String tester = @"Root\1/15/2001 (not complete)\Repealed Sections\10-1-1";
            //Console.WriteLine(tester + Environment.NewLine);
            //Console.WriteLine(tester.Replace(@"([\w])", "(in work)") + Environment.NewLine);
            //Console.WriteLine(Regex.Replace(tester, @"\((.*)\)", "(in work)") + Environment.NewLine);


            //string pattern = @"\p{Sc}*(\s?\d+[.,]?\d*)\p{Sc}*";
            //string replacement = "$";
            //string input = "$16.32 12.19 £16.29 €18.29  €18,29";
            //string result = Regex.Replace(input, pattern, replacement);
            //Console.WriteLine(result);

            //testing compare
            //String a = "01".Substring(2);
            //Console.WriteLine(a.Count());
            //Console.WriteLine("0".Substring(1));
            //Console.WriteLine('1'.CompareTo('2'));
            //Console.WriteLine('2'.CompareTo('1'));
            //Console.WriteLine('a'.CompareTo('b'));
            //Console.WriteLine('b'.CompareTo('a'));
            //Console.WriteLine('a'.CompareTo('a'));
            //Console.WriteLine('1'.CompareTo('1'));
            //Console.WriteLine('1'.CompareTo('z'));
            //Console.WriteLine('A'.CompareTo('a'));
            //Console.WriteLine('-'.CompareTo('-'));
            //Console.WriteLine(''.CompareTo(''));
            //Console.WriteLine(''.CompareTo(''));
            //Console.WriteLine(''.CompareTo(''));

            //testing stuff
            //while (true)
            //{
            //    Console.Write("Enter here: ");
            //    string result = Console.ReadLine();
            //    if (string.IsNullOrEmpty(result) || string.IsNullOrWhiteSpace(result))
            //        Console.WriteLine("BINGO!!");
            //    else
            //        Console.WriteLine();
            //}

            //testing consol.title
            //Console.Title = "OhYeah";
            //Console.ReadKey();
            //Console.Title = "haha";

            //testing lambda
            //String strFolderRoot = @"C:\Users\janetxue\Dropbox\Fairfax Learning files";
            ////List<String> lstDocFolders = Directory.GetDirectories(strFolderRoot).ToList();
            //List<String> lstDocFolders = Directory.GetDirectories(strFolderRoot).Select(docName => Path.GetFileName(docName)).ToList();
            //foreach (String doc in lstDocFolders)
            //{
            //    Console.WriteLine(doc);
            //}

            //testing path.combine
            //String root = @"c:\fairfax\";
            //int num = 0;
            //int doc = 12345;
            //String path = Path.Combine(root, num.ToString(), doc.ToString());
            //Console.WriteLine(path);

            //testing cut/paste
            //String sourceRoot = @"C:\Users\janetxue\Downloads\Migration\Files\testing\Unmatch Docs\SourceTiff";
            //String targetRoot = @"C:\Users\janetxue\Downloads\Migration\Files\testing\Unmatch Docs\TargetPath";
            //List<String> lstSourceDocs = Directory.GetDirectories(sourceRoot).ToList();
            //foreach (String strSourceDoc in lstSourceDocs)
            //{
            //    String strTargetDocPre = strSourceDoc.Replace("SourceTiff", "TargetPath");
            //    int i = 0;
            //    String strTargetDoc = strTargetDocPre + "." + i;
            //    while (Directory.Exists(strTargetDoc))
            //    {
            //        strTargetDoc = strTargetDocPre + "." + (++i);
            //    }
            //    Directory.CreateDirectory(strTargetDoc);
            //    List<String> strSourceFiles = Directory.GetFiles(strSourceDoc).ToList();
            //    foreach(String strSourceFile in strSourceFiles)
            //    {
            //        FileInfo fi = new FileInfo(strSourceFile);
            //        fi.MoveTo(Path.Combine(strTargetDoc, Path.GetFileName(strSourceFile)));
            //    }
            //    Directory.Delete(strSourceDoc);
            //}

            //testing remove
            //String strgroupids = "1234567890";
            //Console.WriteLine(strgroupids);
            //strgroupids = strgroupids.Remove(strgroupids.Length - 2);
            //for (int i = 0; i < 20; i++)
            //{
            //    Console.WriteLine(i % 10);
            //}

            //tool sorting
            Boolean bExit = false;
            while (!bExit)
            {
                Console.WriteLine("Input Strings: ");
                String sInput = Console.ReadLine();
                List<String> lInput = sInput.Split(new[] {", "}, StringSplitOptions.None).ToList();
                lInput.Sort();
                Console.WriteLine();
                Console.WriteLine("Sorted Strings: ");
                foreach (String input in lInput)
                {
                    Console.Write(input + ", ");
                }
                Console.WriteLine();
                Console.WriteLine("\nDo you want to exit?");
                sInput = Console.ReadLine();
                bExit = (sInput == "y" || sInput == "Y") ? true : false;
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static void task1()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("hello hahaha " + Task.CurrentId + " " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.Name);
                Task.Delay(80000);
            }

        }

        private static void task2()
        {
            for (int i = 0; i < 60; i++)
            {
                Console.WriteLine("byebye hahaha " + Task.CurrentId + " " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.Name);
                if (cancelToken.IsCancellationRequested)
                    break;
            }
        }

        delegate TResult Func<TArg0, TResult>(TArg0 arg0);

        static void MyHandler(Exception e)
        {
            Console.WriteLine("handler");
        }

        static void initDatabase(string text)
        {
            try
            {
                Thread.CurrentThread.Abort();
                throw new Exception();
            }
            catch (Exception e)
            {
                Console.WriteLine("catch " + text);
            }

        }

    }
}
