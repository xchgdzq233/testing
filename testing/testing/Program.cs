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
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace testing
{
    class Program
    {
        private static Thread thread1;
        private static Thread thread2;
        private static CancellationToken cancelToken;
        static CancellationTokenSource cancelTokenSource;

        //[STAThread]
        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main(string[] args)
        {
            #region
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
            #endregion

            #region "testing XML"
            ////XDocument test = new XDocument(
            ////    new XComment("commenting"),
            ////    new XElement("Root",
            ////        new XElement("child1", "data1"),
            ////        new XElement("child2", "data2"),
            ////        new XElement("child3",
            ////            new XElement("grandChild31", "data31")),
            ////        new XElement("child4", "data4")));
            ////test.Save(@"C:\Users\janetxue\Downloads\Others\testing\testing.xml");
            //string path = @"C:\Users\Cui\Documents\GitHub\testing\testing\testing\testingXML.xml";

            ////shiporderType shiporder;
            ////XmlSerializer ser = new XmlSerializer(typeof(shiporderType));

            ////XmlDocument xml = new XmlDocument();

            ////using (StreamReader reader = new StreamReader(path))
            ////{
            ////    xml.Load(reader);
            ////    printXML(xml.DocumentElement);
            ////}

            //XmlReader reader = XmlReader.Create(path);
            //XmlReader reader1 = XmlReader.Create(path);
            //XmlSchemaSet schemaSet = new XmlSchemaSet();
            //XmlSchemaInference inference = new XmlSchemaInference();
            //schemaSet = inference.InferSchema(reader);

            //// Display the inferred schema.
            //Console.WriteLine("Original schema:\n");
            //foreach (XmlSchema schema in schemaSet.Schemas())
            //{
            //    schema.Write(Console.Out);
            //}

            //// Use the additional data in item2.xml to refine the original schema.
            //schemaSet = inference.InferSchema(reader1, schemaSet);

            //// Display the refined schema.
            //Console.WriteLine("\n\nRefined schema:\n");
            //foreach (XmlSchema schema in schemaSet.Schemas())
            //{
            //    schema.Write(Console.Out);
            //}


            ////Console.WriteLine(typeof(MyClass));
            ////string path = @"C:\Users\janetxue\Downloads\Others\testing\testing.xml";
            ////XDocument test2 = new XDocument();
            ////test2 = XDocument.Load(path);
            ////Console.WriteLine(test2);

            ////Console.WriteLine();
            ////Console.WriteLine("-------------------START HERE-------------------");
            ////Console.WriteLine();

            //////foreach(XElement el in test2.Descendants("child1"))
            //////{
            //////    Console.WriteLine(el.Value + " " + el.Name);
            //////}

            //////Console.WriteLine();
            //////Console.WriteLine("-------------------START HERE-------------------");
            //////Console.WriteLine();
            //////Console.WriteLine(Path.GetDirectoryName(path));

            //////Console.WriteLine(test2.Root.Elements().Any());
            //////Console.WriteLine(test2.Descendants("child2").Descendants().Any());
            //////Console.WriteLine(test2.Descendants("child3").Descendants().Any());
            ////Console.WriteLine(test2.Descendants("grandChild31"));
            #endregion "testing XML"

            #region "testing Task"

            //cancelTokenSource = new CancellationTokenSource();
            //cancelToken = cancelTokenSource.Token;

            //log4net.GlobalContext.Properties["LogName"] = "test";
            //log = LogUtility.GetLogger();

            //Task t1 = null, t2 = null;

            //t1 = new Task(() => task1());
            //t2 = new Task(() => task2());

            //if (!Object.ReferenceEquals(t1, null))
            //    t1.Start();
            //if (!Object.ReferenceEquals(t2, null))
            //    t2.Start();

            ////cancelTokenSource.Cancel();

            //if (!Object.ReferenceEquals(t1, null))
            //    t1.Wait();
            //if (!Object.ReferenceEquals(t2, null))
            //    t2.Wait();

            //Console.WriteLine("ALL COMPLETE");
            //Console.ReadKey();

            ////var list = new ConcurrentBag<string>();
            ////string[] dirNames = { ".", ".." };
            ////List<Task> tasks = new List<Task>();
            ////foreach (var dirName in dirNames)
            ////{
            ////    Task t = Task.Run(() =>
            ////    {
            ////        foreach (var path in Directory.GetFiles(dirName))
            ////            list.Add(path);
            ////    });
            ////    tasks.Add(t);
            ////}
            ////Task.WaitAll(tasks.ToArray());
            ////foreach (Task t in tasks)
            ////    Console.WriteLine("Task {0} Status: {1}", t.Id, t.Status);

            ////Console.WriteLine("Number of files read: {0}", list.Count);
            ////foreach (var x in list)
            ////    Console.WriteLine(x);

            #endregion "testing Task"

            #region
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
            //Boolean bExit = false;
            //while (!bExit)
            //{
            //    Console.WriteLine("Input Strings: ");
            //    String sInput = Console.ReadLine();
            //    List<String> lInput = sInput.Split(new[] {", "}, StringSplitOptions.None).ToList();
            //    lInput.Sort();
            //    Console.WriteLine();
            //    Console.WriteLine("Sorted Strings: ");
            //    foreach (String input in lInput)
            //    {
            //        Console.Write(input + ", ");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine("\nDo you want to exit?");
            //    sInput = Console.ReadLine();
            //    bExit = (sInput == "y" || sInput == "Y") ? true : false;
            //    Console.WriteLine();
            //}

            //98-361
            //int[] bubbleSort = new int[] { 3, 4, 2, 5, 1, 7, 6 };

            //for (int i = 0; i < bubbleSort.Length; i++)
            //    for (int j = i + 1; j < bubbleSort.Length; j++)
            //        if (bubbleSort[i] > bubbleSort[j])
            //        {
            //            int holder = bubbleSort[i];
            //            bubbleSort[i] = bubbleSort[j];
            //            bubbleSort[j] = holder;
            //        }

            //foreach(int x in bubbleSort)
            //{
            //    Console.Write(x.ToString() + " ");
            //}

            //Console.WriteLine();
            //Console.WriteLine("Press any keys to quit.");
            //Console.ReadKey();
            #endregion

            #region "log4net"

            //log4net.GlobalContext.Properties["abc"] = "testing";
            ////log4net.Config.XmlConfigurator.Configure();
            //log = LogHelper.GetLogger();
            //log.Info("testing");
            //Environment.Exit(0);

            //log.Debug("Developer: Tutorial was run");
            //log.Info("Maintenance: water pump turned on");
            //log.Warn("Maintenance: the water pump is getting hot");

            //var i = 0;

            //try
            //{
            //    var x = 10 / i;
            //}
            //catch(DivideByZeroException ex)
            //{
            //    log.Error("Developer: we tried to divide by zero again");
            //}

            //log.Fatal("Maintenance: water pump exploded");
            //Console.ReadKey();

            #endregion

            #region "XML Schema"
            //XmlSchema schema = new XmlSchema();

            //// <xs:element name="cat" type="xs:string"/>
            //XmlSchemaElement elementCat = new XmlSchemaElement();
            //schema.Items.Add(elementCat);
            //elementCat.Name = "cat";
            //elementCat.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");

            //// <xs:element name="dog" type="xs:string"/>
            //XmlSchemaElement elementDog = new XmlSchemaElement();
            //schema.Items.Add(elementDog);
            //elementDog.Name = "dog";
            //elementDog.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");

            //// <xs:element name="redDog" substitutionGroup="dog" />
            //XmlSchemaElement elementRedDog = new XmlSchemaElement();
            //schema.Items.Add(elementRedDog);
            //elementRedDog.Name = "redDog";
            //elementRedDog.SubstitutionGroup = new XmlQualifiedName("dog");

            //// <xs:element name="brownDog" substitutionGroup ="dog" />
            //XmlSchemaElement elementBrownDog = new XmlSchemaElement();
            //schema.Items.Add(elementBrownDog);
            //elementBrownDog.Name = "brownDog";
            //elementBrownDog.SubstitutionGroup = new XmlQualifiedName("dog");


            //// <xs:element name="pets">
            //XmlSchemaElement elementPets = new XmlSchemaElement();
            //schema.Items.Add(elementPets);
            //elementPets.Name = "pets";

            //// <xs:complexType>
            //XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            //elementPets.SchemaType = complexType;

            //// <xs:choice minOccurs="0" maxOccurs="unbounded">
            //XmlSchemaChoice choice = new XmlSchemaChoice();
            //complexType.Particle = choice;
            //choice.MinOccurs = 0;
            //choice.MaxOccursString = "unbounded";

            //// <xs:element ref="cat"/>
            //XmlSchemaElement catRef = new XmlSchemaElement();
            //choice.Items.Add(catRef);
            //catRef.RefName = new XmlQualifiedName("cat");

            //// <xs:element ref="dog"/>
            //XmlSchemaElement dogRef = new XmlSchemaElement();
            //choice.Items.Add(dogRef);
            //dogRef.RefName = new XmlQualifiedName("dog");

            //XmlSchemaSet schemaSet = new XmlSchemaSet();
            //schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
            //schemaSet.Add(schema);
            //schemaSet.Compile();

            //XmlSchema compiledSchema = null;

            //foreach (XmlSchema schema1 in schemaSet.Schemas())
            //{
            //    compiledSchema = schema1;
            //}

            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            //nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            //compiledSchema.Write(Console.Out, nsmgr);



            // Create the FirstName and LastName elements.
            XmlSchemaElement firstNameElement = new XmlSchemaElement();
            firstNameElement.Name = "FirstName";
            XmlSchemaElement lastNameElement = new XmlSchemaElement();
            lastNameElement.Name = "LastName";

            // Create CustomerId attribute.
            XmlSchemaAttribute idAttribute = new XmlSchemaAttribute();
            idAttribute.Name = "CustomerId";
            idAttribute.Use = XmlSchemaUse.Required;

            // Create the simple type for the LastName element.
            XmlSchemaSimpleType lastNameType = new XmlSchemaSimpleType();
            lastNameType.Name = "LastNameType";
            XmlSchemaSimpleTypeRestriction lastNameRestriction =
                new XmlSchemaSimpleTypeRestriction();
            lastNameRestriction.BaseTypeName =
                new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            XmlSchemaMaxLengthFacet maxLength = new XmlSchemaMaxLengthFacet();
            maxLength.Value = "20";
            lastNameRestriction.Facets.Add(maxLength);
            lastNameType.Content = lastNameRestriction;

            // Associate the elements and attributes with their types.
            // Built-in type.
            firstNameElement.SchemaTypeName =
                new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            // User-defined type.
            lastNameElement.SchemaTypeName =
                new XmlQualifiedName("LastNameType", "http://www.tempuri.org");
            // Built-in type.
            idAttribute.SchemaTypeName = new XmlQualifiedName("positiveInteger",
                "http://www.w3.org/2001/XMLSchema");

            // Create the top-level Customer element.
            XmlSchemaElement customerElement = new XmlSchemaElement();
            customerElement.Name = "Customer";
            //customerElement.SchemaTypeName = new XmlQualifiedName("ssf");

            // Create an anonymous complex type for the Customer element.
            XmlSchemaComplexType customerType = new XmlSchemaComplexType();
            XmlSchemaSequence sequence = new XmlSchemaSequence();
            sequence.Items.Add(firstNameElement);
            sequence.Items.Add(lastNameElement);
            customerType.Particle = sequence;

            // Add the CustomerId attribute to the complex type.
            customerType.Attributes.Add(idAttribute);

            // Set the SchemaType of the Customer element to
            // the anonymous complex type created above.
            customerElement.SchemaType = customerType;

            // Create an empty schema.
            XmlSchema customerSchema = new XmlSchema();
            //customerSchema.TargetNamespace = "http://www.tempuri.org";

            // Add all top-level element and types to the schema
            customerSchema.Items.Add(customerElement);
            customerSchema.Items.Add(lastNameType);

            // Create an XmlSchemaSet to compile the customer schema.
            //XmlSchemaSet schemaSet = new XmlSchemaSet();
            //schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            //schemaSet.Add(customerSchema);
            //schemaSet.Compile();

            //foreach (XmlSchema schema in schemaSet.Schemas())
            //{
            //    customerSchema = schema;
            //}

            // Write the complete schema to the Console.
            //customerSchema.Write(Console.Out);

            XmlWriter writer = XmlWriter.Create(@"C:\Users\janetxue\Downloads\Others\testing\eReg_XML\result.xsd");
            customerSchema.Write(writer);

            #endregion "XML Shcema"
        }

        public static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            Console.WriteLine(args.Message);
        }

        private static log4net.ILog log;

        private static void printXML(XmlNode node, int depth = 0)
        {
            for (int i = 0; i < depth; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine(node.Name);
            if (node.HasChildNodes)
                foreach (XmlNode childNote in node)
                    if (childNote is XmlElement)
                        printXML(childNote, depth + 1);
        }

        //private static readonly Object _syncObject = new Object();

        //public static void LogIt(String LogMessage, TextWriter writer)
        //{
        //    lock(_syncObject)
        //    {
        //        writer.WriteLine(LogMessage);
        //        writer.Flush();
        //    }
        //}

        private static void task1()
        {
            String logPath = @"C:\Users\janetxue\Downloads\Others\testing\task1.txt";
            TextWriter writer = new StreamWriter(logPath, true);
            for (int i = 0; i < 5; i++)
            {
                LogUtility.LogIt(String.Format("task1 - TaskID: {0}, ThreadID: {1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId), writer);
                //Console.WriteLine("hello hahaha " + Task.CurrentId + " " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.Name);
                //Console.WriteLine("task1");
                //Task.Delay(300);
            }

        }

        private static void task2()
        {
            String logPath = @"C:\Users\janetxue\Downloads\Others\testing\task2.txt";
            TextWriter writer = new StreamWriter(logPath, true);
            for (int i = 0; i < 10; i++)
            {
                LogUtility.LogIt(String.Format("task2 - TaskID: {0}, ThreadID: {1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId), writer);
                //Console.WriteLine("byebye hahaha " + Task.CurrentId + " " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.Name);
                //Console.WriteLine("task2");
                //log.Info("task2");
                Task.Delay(50);
                if (cancelToken.IsCancellationRequested)
                {
                    Console.WriteLine("break it baby");
                    break;
                }
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

    class TestXMLSchema : XmlNamespaceManager
    {
        private String defaultNS;

        public TestXMLSchema(NameTable nameTable)
        {
        }

        public override string DefaultNamespace
        {
            get
            {
                return base.DefaultNamespace;
            }
            set
            {
                base.DefaultNamespace = 
            }
        }
    }
    
    class LogUtility
    {
        private static readonly Object _syncObject = new Object();

        public static log4net.ILog GetLogger([CallerFilePath]String filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }

        public static void LogIt (String logMessage, TextWriter writer)
        {
            lock(_syncObject)
            {
                writer.WriteLine(logMessage);
                writer.Flush();
            }
        }
    }
}
