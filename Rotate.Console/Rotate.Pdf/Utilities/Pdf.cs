using System;
using System.IO;
using EO.Pdf;
using EO.Pdf.Drawing;

namespace RDI.Web.Utilities
{
    public static class Pdf
    {
        public enum Rotationtype{ Clock90 = -1, Count90 = 1, Half180 = 2}
        public static Stream Rotate(Rotationtype rtype, Stream docstream)
        {
           
            var pdf = new PdfDocument(docstream);
            var pdfPage = pdf.Pages[0];
            float r, tX, tY;
            switch (rtype)
            {
                case Rotationtype.Clock90:
                    pdfPage.Size = GetReorientedPage(pdfPage);
                    tX = (72*(float) rtype)*pdfPage.Size.Height;
                    tY = 0;
                    r = 90*(float) rtype;
                    break;
                case Rotationtype.Count90:
                    pdfPage.Size = GetReorientedPage(pdfPage);
                    tX = 0;
                    tY = -72 * pdfPage.Size.Width;
                    r = 90 * (float)rtype;
                    break;
                case Rotationtype.Half180:
                    tX = -72 * pdfPage.Size.Width;
                    tY = -72 * pdfPage.Size.Height;
                    r = -90 * (float)rtype;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("rtype");
            }
            

            var matrix = new PdfMatrix();
            matrix.Rotate(r);
            matrix.Translate(tX, tY);
           
            pdfPage.Transform(matrix);
            var stream = new MemoryStream();
            pdf.Save(docstream);
            return docstream;
        }
        /// <summary>
        /// Switch page orientation from landscape to portrait or visa versa
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static PdfSize  GetReorientedPage(PdfPage page)
        {
            return new PdfSize(page.Size.Height, page.Size.Width);
        }      
        /// <summary>
        /// Generate a stream to view the pdf as an image in the browser
        /// </summary>
        /// <param name="docstream"></param>
        /// <returns></returns>
        //public Stream GetImagePreview(Stream docstream)
        //{
        //    return new Stream();
        //}
    }
 
}
