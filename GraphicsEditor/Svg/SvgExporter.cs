using System.Collections.Generic;
using System.IO;
using System.Text;
using DrawablesUI;

namespace GraphicsEditor.Svg
{
    public class SvgExporter : ISvgExporter
    {
        private string HEADER = "<svg version=\"1.1\" \n" +
                                "baseProfile=\"full\" \n" +
                                "width=\"{0}\" height=\"{1}\" \n" +
                                "xmlns=\"http://www.w3.org/2000/svg\">";

        private string CLOSING_TAG = "</svg>";
        
        public void Export(List<IShape> shapes, string fileName)
        {
            var width = DrawableGUI.GetFormWidth();
            var height = DrawableGUI.GetFormHeight();
            var actualHeader = string.Format(HEADER, width, height);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(actualHeader);

            foreach (var shape in shapes)
            {
                stringBuilder.AppendLine(shape.ToSvg());
            }

            stringBuilder.AppendLine(CLOSING_TAG);
            
            fileName = fileName.Contains(".svg") ? fileName : fileName + ".svg";
            File.WriteAllText($"{fileName}", stringBuilder.ToString());
        }
    }
}