using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ThothCbz.Extensions
{
    internal static class GraphicsExtension
    {
        internal static Graphics SetDefaultQuality(
                this Graphics graphics,
                Color? backgroundColor = null
            )
        {
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.SmoothingMode = SmoothingMode.HighQuality;

            graphics.Clear(backgroundColor ?? Color.White);

            return graphics;
        }

    }
}
