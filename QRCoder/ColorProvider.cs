using System.Drawing;

namespace QRCoder
{
    public interface ColorProvider
    {
        Brush getBrush(SourceType stype, bool module);
    }
}
