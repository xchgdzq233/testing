﻿using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEditor.Utilities
{
    public class FFXPdfRenderListener : IRenderListener
    {
        private FFXPdfPage page;

        public FFXPdfRenderListener(FFXPdfPage page)
        {
            this.page = page;
        }

        public void RenderText(TextRenderInfo renderInfo)
        {
            page.AddNewTokenToPage(new FFXPdfToken(renderInfo, page.iPageHeight));
        }

        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo) { }
    }
}
