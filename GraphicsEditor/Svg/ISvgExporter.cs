using System.Collections.Generic;

namespace GraphicsEditor.Svg
{
    public interface ISvgExporter
    {
        void Export(List<IShape> shapes, string fileName);
    }
}