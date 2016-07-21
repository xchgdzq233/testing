using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.util;
using System.Xml;

namespace Test_iText
{
    public class TextExtractionStrategy_Font : iTextSharp.text.pdf.parser.ITextExtractionStrategy
    {
        //HTML buffer
        private StringBuilder result = new StringBuilder();

        //Store last used properties
        private Vector lastBaseLine;
        private string lastFont;
        private float lastFontSize;

        private enum TextRenderMode
        {
            FillText = 0,
            StrokeText = 1,
            FillThenStrokeText = 2,
            Invisible = 3,
            FillTextAndAddToPathForClipping = 4,
            StrokeTextAndAddToPathForClipping = 5,
            FillThenStrokeTextAndAddToPathForClipping = 6,
            AddTextToPaddForClipping = 7
        }

        public void RenderText(iTextSharp.text.pdf.parser.TextRenderInfo renderInfo)
        {
            string curFont = renderInfo.GetFont().PostscriptFontName;
            bool bBold = false;
            if(curFont.Contains("Bold"))
            {
                bBold = true;
                curFont = curFont.Replace("-Bold", "");
            }
            //Check if faux bold is used
            if ((renderInfo.GetTextRenderMode() == (int)TextRenderMode.FillThenStrokeText))
            {
                bBold = true;
            }

            //This code assumes that if the baseline changes then we're on a newline
            Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
            Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(curBaseline[Vector.I1], curBaseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
            Single curFontSize = rect.Height;

            //See if something has changed, either the baseline, the font or the font size
            if ((this.lastBaseLine == null) || (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]) || (curFontSize != lastFontSize) || (curFont != lastFont))
            {
                //if we've put down at least one span tag close it
                if ((this.lastBaseLine != null))
                {
                    this.result.AppendLine("</span></div>");
                }
                //If the baseline has changed then insert a line break
                if ((this.lastBaseLine != null) && curBaseline[Vector.I2] != lastBaseLine[Vector.I2])
                {
                    this.result.AppendLine("<br />");
                }
                //Create an HTML tag with appropriate styles
                this.result.AppendFormat("<div style=\"position:absolute;top:{0};left:{1}\">", rect.Top, rect.Left);
                this.result.AppendFormat("<span style=\"font-style:normal;font-weight:{0};font-size:{1}px;font-family:{2};color:#000000;\">", bBold ? "bold" : "normal", rect.Height * 2 - 1, curFont);
            }

            //Append the current text
            this.result.Append(renderInfo.GetText());

            //Set currently used properties
            this.lastBaseLine = curBaseline;
            this.lastFontSize = curFontSize;
            this.lastFont = curFont;
        }

        public string GetResultantText()
        {
            //If we wrote anything then we'll always have a missing closing tag so close it here
            if (result.Length > 0)
            {
                result.Append("</span>");
            }
            return result.ToString();
        }

        //Not needed
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) { }
    }
    
    public class LocationTextExtractionStrategy_Line : LocationTextExtractionStrategy
    {
        private List<LocationTextExtractionStrategy_Line.ExtendedTextChunk> m_DocChunks = new List<ExtendedTextChunk>();
        private List<LocationTextExtractionStrategy_Line.LineInfo> m_LinesTextInfo = new List<LineInfo>();
        public List<SearchResult> m_SearchResultsList = new List<SearchResult>();
        private String m_SearchText;
        public const float PDF_PX_TO_MM = 0.3528f;
        public float m_PageSizeY;

        public LocationTextExtractionStrategy_Line(string m_SearchText, float fPageSizeY)
            : base()
        {
            this.m_SearchText = m_SearchText;
            this.m_PageSizeY = fPageSizeY;
        }

        private void searchText()
        {
            foreach (LineInfo aLineInfo in m_LinesTextInfo)
            {
                int iIndex = aLineInfo.m_Text.IndexOf(m_SearchText);
                if (iIndex != -1)
                {
                    TextRenderInfo aFirstLetter = aLineInfo.m_LineCharsList.ElementAt(iIndex);
                    SearchResult aSearchResult = new SearchResult(aFirstLetter, m_PageSizeY);
                    this.m_SearchResultsList.Add(aSearchResult);
                }
            }
        }

        private void groupChunksbyLine()
        {
            LocationTextExtractionStrategy_Line.ExtendedTextChunk textChunk1 = null;
            LocationTextExtractionStrategy_Line.LineInfo textInfo = null;
            foreach (LocationTextExtractionStrategy_Line.ExtendedTextChunk textChunk2 in this.m_DocChunks)
            {
                if (textChunk1 == null)
                {
                    textInfo = new LocationTextExtractionStrategy_Line.LineInfo(textChunk2);
                    this.m_LinesTextInfo.Add(textInfo);
                }
                else if (textChunk2.sameLine(textChunk1))
                {
                    textInfo.appendText(textChunk2);
                }
                else
                {
                    textInfo = new LocationTextExtractionStrategy_Line.LineInfo(textChunk2);
                    this.m_LinesTextInfo.Add(textInfo);
                }
                textChunk1 = textChunk2;
            }
        }

        public override string GetResultantText()
        {
            groupChunksbyLine();
            searchText();
            //In this case the return value is not useful
            return "";
        }

        public override void RenderText(TextRenderInfo renderInfo)
        {
            LineSegment baseline = renderInfo.GetBaseline();
            //Create ExtendedChunk
            ExtendedTextChunk aExtendedChunk = new ExtendedTextChunk(renderInfo.GetText(), baseline.GetStartPoint(), baseline.GetEndPoint(), renderInfo.GetSingleSpaceWidth(), renderInfo.GetCharacterRenderInfos().ToList());
            this.m_DocChunks.Add(aExtendedChunk);
        }

        public class ExtendedTextChunk
        {
            public string m_text;
            private Vector m_startLocation;
            private Vector m_endLocation;
            private Vector m_orientationVector;
            private int m_orientationMagnitude;
            private int m_distPerpendicular;
            private float m_charSpaceWidth;
            public List<TextRenderInfo> m_ChunkChars;


            public ExtendedTextChunk(string txt, Vector startLoc, Vector endLoc, float charSpaceWidth, List<TextRenderInfo> chunkChars)
            {
                this.m_text = txt;
                this.m_startLocation = startLoc;
                this.m_endLocation = endLoc;
                this.m_charSpaceWidth = charSpaceWidth;
                this.m_orientationVector = this.m_endLocation.Subtract(this.m_startLocation).Normalize();
                this.m_orientationMagnitude = (int)(Math.Atan2((double)this.m_orientationVector[1], (double)this.m_orientationVector[0]) * 1000.0);
                this.m_distPerpendicular = (int)this.m_startLocation.Subtract(new Vector(0.0f, 0.0f, 1f)).Cross(this.m_orientationVector)[2];
                this.m_ChunkChars = chunkChars;

            }


            public bool sameLine(LocationTextExtractionStrategy_Line.ExtendedTextChunk textChunkToCompare)
            {
                return this.m_orientationMagnitude == textChunkToCompare.m_orientationMagnitude && this.m_distPerpendicular == textChunkToCompare.m_distPerpendicular;
            }


        }

        public class SearchResult
        {
            public int iPosX;
            public int iPosY;

            public SearchResult(TextRenderInfo aCharcter, float fPageSizeY)
            {
                //Get position of upperLeft coordinate
                Vector vTopLeft = aCharcter.GetAscentLine().GetStartPoint();
                //PosX
                float fPosX = vTopLeft[Vector.I1];
                //PosY
                float fPosY = vTopLeft[Vector.I2];
                //Transform to mm and get y from top of page
                iPosX = Convert.ToInt32(fPosX * PDF_PX_TO_MM);
                iPosY = Convert.ToInt32((fPageSizeY - fPosY) * PDF_PX_TO_MM);
            }
        }

        public class LineInfo
        {
            public string m_Text;
            public List<TextRenderInfo> m_LineCharsList;

            public LineInfo(LocationTextExtractionStrategy_Line.ExtendedTextChunk initialTextChunk)
            {
                this.m_Text = initialTextChunk.m_text;
                this.m_LineCharsList = initialTextChunk.m_ChunkChars;
            }

            public void appendText(LocationTextExtractionStrategy_Line.ExtendedTextChunk additionalTextChunk)
            {
                m_LineCharsList.AddRange(additionalTextChunk.m_ChunkChars);
                this.m_Text += additionalTextChunk.m_text;
            }
        }
    }

    public class MyTextRenderListener : IRenderListener
    {
        protected StreamWriter sw;
        protected float fPageSize;
        protected float fCurPageHeight;

        public MyTextRenderListener(float fPageSize, StreamWriter sw, float fCurPageHeight)
        {
            this.sw = sw;
            this.fPageSize = fPageSize;
            this.fCurPageHeight = fCurPageHeight;
        }

        public void BeginTextBlock()
        {
        }

        public void EndTextBlock()
        {

        }
        public void RenderImage(ImageRenderInfo renderInfo)
        {
        }
        public void RenderText(TextRenderInfo renderInfo)
        {
            float fExpandSize = 1.77164021f;
            string sFont = renderInfo.GetFont().PostscriptFontName.Replace("-Bold", "");
            bool bBold = renderInfo.GetFont().PostscriptFontName.Contains("Bold") ? true : false;
            RectangleJ rect = renderInfo.GetBaseline().GetBoundingRectange();
            float width = renderInfo.GetAscentLine().GetStartPoint()[Vector.I2] - renderInfo.GetDescentLine().GetEndPoint()[Vector.I2];

            sw.WriteLine("<div style=\"position:absolute;top:{0}px;left:{1}px;\">", Convert.ToInt32((fPageSize - rect.Y) * fExpandSize + fCurPageHeight), Convert.ToInt32(rect.X * fExpandSize));
            sw.WriteLine("<span style=\"font-style:normal;font-weight:{0};font-family:{1};color:#000000;font-size:{2}px\">", bBold ? "bold" : "normal", sFont, Convert.ToInt32(width * fExpandSize));
            sw.WriteLine(renderInfo.GetText());
            sw.WriteLine("</span></div>");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string sFile3 = @"C:\FFX_Projects\NY_Legislative\Samples\Bill and LBD Overhead.pdf";
            string sFile4 = @"C:\FFX_Projects\NY_Legislative\Samples\12037-01-5.pdf";
            string sFile5 = @"C:\FFX_Projects\NY_Legislative\Samples\Test.pdf";

            PdfReader reader = new PdfReader(new RandomAccessFileOrArray(sFile4), null);

            TextExtractionStrategy_Font s = new TextExtractionStrategy_Font();
            LocationTextExtractionStrategy_Line l = new LocationTextExtractionStrategy_Line("SENATE", reader.GetPageSize(1).Height);
            using (StreamWriter sw = new StreamWriter(@"C:\FFX_Projects\NY_Legislative\Samples\result.html"))
            {
                sw.WriteLine("<html><body><div style=\"padding-left:5px\">");
                //for (int i = 1; i <= Math.Min(reader.NumberOfPages, 15); i++ )
                //sw.WriteLine(PdfTextExtractor.GetTextFromPage(reader, 1, s));
                //sw.WriteLine(PdfTextExtractor.GetTextFromPage(reader, 1, l));
                PdfDictionary pageDic = reader.GetPageN(1);
                PdfDictionary resourceDic = pageDic.GetAsDict(PdfName.RESOURCES);
                float fCurPageHeight = 0f;
                for (int i = 1; i <= Math.Min(3, reader.NumberOfPages); i++)
                {
                    IRenderListener listener = new MyTextRenderListener(reader.GetPageSize(i).Height, sw, fCurPageHeight);
                    PdfContentStreamProcessor processor = new PdfContentStreamProcessor(listener);
                    processor.ProcessContent(ContentByteUtils.GetContentBytesForPage(reader, i), resourceDic);
                    sw.Flush();
                    fCurPageHeight += reader.GetPageSize(i).Height * 1.77164021f;
                }
                sw.WriteLine("</div></body></html>");
            }

            //PdfReaderContentParser parser = new PdfReaderContentParser(reader);
            //PdfStamper stamper = new PdfStamper(reader, new FileStream(@"C:\FFX_Projects\NY_Legislative\Samples\result.pdf", FileMode.Create));
            //TextMarginFinder finder = parser.ProcessContent(1, new TextMarginFinder());
            //PdfContentByte cb = stamper.GetOverContent(1);
            //cb.Rectangle(finder.GetLlx(), finder.GetLly(), finder.GetWidth(), finder.GetHeight());
            //cb.Stroke();
            //stamper.Close();



            //string sFolder = @"C:\FFX_Projects\NY_Legislative\Samples";
            //DirectoryInfo d = new DirectoryInfo(sFolder);

            //foreach (FileInfo file in d.GetFiles("*.pdf"))
            //{
            //    PdfReader doc = new PdfReader(file.FullName);
            //    Console.WriteLine("******\n");
            //    Console.WriteLine("Found a file at {0}, file size: {1}KB, ", file.FullName, doc.FileLength / 1024);

            //    //foreach (string key in doc.Info.Keys)
            //    //{
            //    //    Console.WriteLine("<{0}>: {1}", key, doc.Info[key]);
            //    //}

            //    //try
            //    //{
            //    //    XmlDocument xml = new XmlDocument();
            //    //    xml.Load(new MemoryStream(doc.Metadata));
            //    //    xml.Save(Path.Combine(sFolder, file.Name + "_result.xml"));

            //    //}
            //    //catch(Exception)
            //    //{
            //    //    Console.WriteLine("Cannot read metadata.");
            //    //}

            //    //byte[] b = File.ReadAllBytes(file.FullName)

            //    Console.WriteLine(Encoding.ASCII.GetString(File.ReadAllBytes(file.FullName), 0, 10));

            //}
        }
    }
}
