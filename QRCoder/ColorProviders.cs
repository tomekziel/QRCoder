using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace QRCoder
{
    public static class ColorProviders
    {

        public class ColorProviderBW : ColorProvider
        {


            static Brush[] brushTable =
            {
            Brushes.White, Brushes.Black, // NONE
            Brushes.White, Brushes.Black, // QUIETPART
            Brushes.White, Brushes.Black,  // TIMING
            Brushes.White, Brushes.Black, // ALIGN,
            Brushes.White, Brushes.Black, // FINDER
            Brushes.White, Brushes.Black, // DARK
            Brushes.White, Brushes.Black, //VERSION
            Brushes.White, Brushes.Black, // FORMAT
            Brushes.White, Brushes.Black, // DATA
            Brushes.White, Brushes.Black, // SPACING
            Brushes.White, Brushes.Black // INVALID
        };

            public Brush getBrush(SourceType stype, bool module)
            {
                return brushTable[((int)stype) * 2 + (module ? 1 : 0)];
            }

        }

        public class ColorProviderDebug : ColorProvider
        {
            public ColorProviderDebug()
            {

            }

            static Brush[] brushTable =
            {
            // light dark
            ColorProviders.BrushFromHex("ff0000"),ColorProviders.BrushFromHex("ff0000"), // NONE
            Brushes.LightGray, Brushes.LightGray, // QUIETPART
            ColorProviders.BrushFromHex("afe4f8"),ColorProviders.BrushFromHex("005878"), // TIMING
            ColorProviders.BrushFromHex("e7b7cf"),ColorProviders.BrushFromHex("391326"), // ALIGN,
            ColorProviders.BrushFromHex("ffcdcd"),ColorProviders.BrushFromHex("5f0000"), // FINDER
            ColorProviders.BrushFromHex("000000"),ColorProviders.BrushFromHex("000000"), // DARK
            ColorProviders.BrushFromHex("84a9cb"),ColorProviders.BrushFromHex("04294b"), // VERSION
            ColorProviders.BrushFromHex("ffffb8"),ColorProviders.BrushFromHex("666600"),// FORMAT
            Brushes.White, Brushes.Black, // DATA
            ColorProviders.BrushFromHex("e1f5ca"),ColorProviders.BrushFromHex("e1f5ca"), // SPACING
            Brushes.OrangeRed, Brushes.DarkRed // INVALID
        };

            public Brush getBrush(SourceType stype, bool module)
            {

                return brushTable[((int)stype) * 2 + (module ? 1 : 0)];
            }
        }
        public static SolidBrush BrushFromHex(String colorcode)
        {
            int argb = Int32.Parse("ff"+colorcode, NumberStyles.HexNumber);
            Color clr = Color.FromArgb(argb);
            return new SolidBrush(clr);
            
        }

        public class ColorProviderRysowankiWyrownanie : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush gray = ColorProviders.BrushFromHex("dddddd");
            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER)
                {
                    return module ? gray : Brushes.White;
                }

                if (stype == SourceType.ALIGN)
                {
                    return debug.getBrush(stype, module);
                }
                return Brushes.White;
            }
        }

        public class ColorProviderRysowankiTimer : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush gray = ColorProviders.BrushFromHex("dddddd");
            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN)
                {
                    return module ? gray : Brushes.White;
                }

                if (stype == SourceType.TIMING)
                {
                    return debug.getBrush(stype, module);
                }
                return Brushes.White;
            }
        }


        public class ColorProviderRysowankiFormat : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush gray = ColorProviders.BrushFromHex("dddddd");
            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING)
                {
                    return module ? gray : Brushes.White;
                }

                if (stype == SourceType.FORMAT || stype == SourceType.DARK)
                {
                    return debug.getBrush(stype, module);
                }
                return Brushes.White;
            }
        }


        public class ColorProviderRysowankiWersja : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush gray = ColorProviders.BrushFromHex("dddddd");
            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING || stype == SourceType.DARK)
                {
                    return module ? gray : Brushes.White;
                }
                if (stype == SourceType.FORMAT || stype == SourceType.DARK)
                {
                    return gray;
                }

                if (stype == SourceType.VERSION)
                {
                    return debug.getBrush(stype, module);
                }
                return Brushes.White;
            }
        }



        public class ColorProviderRysowankiAllReserved: ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush gray = ColorProviders.BrushFromHex("555555");
            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING || 
                    stype == SourceType.DARK || stype == SourceType.FORMAT || stype == SourceType.DARK ||
                    stype == SourceType.VERSION || stype == SourceType.QUIETPART || stype == SourceType.SPACING)
                {
                    return gray;
                }
                return Brushes.White;
            }
        }

        public class ColorProviderRysowankiFinder : ColorProvider
        {
            ColorProviderDebug debug = new ColorProviderDebug();

            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.SPACING)
                {
                    return debug.getBrush(stype, module);
                }
                return Brushes.White;
            }
        }


        public class ColorProviderRysowankiMaska : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush grayD = ColorProviders.BrushFromHex("aaaaaa");
            Brush grayL = ColorProviders.BrushFromHex("bbbbbb");

            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING ||
                    stype == SourceType.DARK || stype == SourceType.FORMAT || stype == SourceType.DARK ||
                    stype == SourceType.VERSION || stype == SourceType.QUIETPART || stype == SourceType.SPACING)
                {
                    return module ? grayD : grayL;
                }
                return module ? Brushes.Black : Brushes.White;
            }
        }

        public class ColorProviderKolejnosc : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush grayD = ColorProviders.BrushFromHex("aaaaaa");
            Brush grayL = ColorProviders.BrushFromHex("bbbbbb");

            public Brush getBrush(SourceType stype, bool module)
            {
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING ||
                    stype == SourceType.DARK || stype == SourceType.FORMAT || stype == SourceType.DARK ||
                    stype == SourceType.VERSION || stype == SourceType.QUIETPART || stype == SourceType.SPACING)
                {
                    return module ? grayD : grayL;
                }
                if (stype == SourceType.DATA)
                {
                    return Brushes.White;
                }
                return module ? Brushes.Black : Brushes.White;
            }
        }

        public class ColorProviderRysowankiAnim : ColorProvider
        {

            ColorProviderDebug debug = new ColorProviderDebug();
            Brush grayD = ColorProviders.BrushFromHex("aaaaaa");
            Brush grayL = ColorProviders.BrushFromHex("bbbbbb");

            Brush grayH = ColorProviders.BrushFromHex("dddddd");

            public Brush getBrush(SourceType stype, bool module)
            {
                if ( stype == SourceType.QUIETPART)
                {
                    return Brushes.White;
                }
                if (stype == SourceType.FINDER || stype == SourceType.ALIGN || stype == SourceType.TIMING ||
                    stype == SourceType.DARK || stype == SourceType.FORMAT || stype == SourceType.DARK ||
                    stype == SourceType.VERSION || stype == SourceType.QUIETPART || stype == SourceType.SPACING)
                {
                    return module ? grayD : grayL;
                }
                if (stype == SourceType.NONE)
                {
                    return grayH;
                }
                return module ? Brushes.Black : Brushes.White;
            }
        }

        public class ColorProviderDebug2 : ColorProvider
        {
            public ColorProviderDebug2()
            {
            }


            static Brush[] brushTable =
            {
            // light dark
            Brushes.White, Brushes.White, // NONE
            Brushes.White, Brushes.White, // INVALID// QUIETPART
            ColorProviders.BrushFromHex("afe4f8"),ColorProviders.BrushFromHex("005878"), // TIMING
            ColorProviders.BrushFromHex("e7b7cf"),ColorProviders.BrushFromHex("391326"), // ALIGN,
            ColorProviders.BrushFromHex("ffcdcd"),ColorProviders.BrushFromHex("5f0000"), // FINDER
            ColorProviders.BrushFromHex("000000"),ColorProviders.BrushFromHex("000000"), // DARK
            ColorProviders.BrushFromHex("84a9cb"),ColorProviders.BrushFromHex("04294b"), // VERSION
            ColorProviders.BrushFromHex("ffffb8"),ColorProviders.BrushFromHex("666600"),// FORMAT
            Brushes.White, Brushes.Black, // DATA
            ColorProviders.BrushFromHex("e1f5ca"),ColorProviders.BrushFromHex("e1f5ca"), // SPACING
            Brushes.White, Brushes.White // INVALID
        };

            public Brush getBrush(SourceType stype, bool module)
            {

                return brushTable[((int)stype) * 2 + (module ? 1 : 0)];
            }

        }
    }
}
