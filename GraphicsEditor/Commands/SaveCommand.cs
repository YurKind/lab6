using System;
using System.IO;
using System.Text;
using ConsoleUI;

namespace GraphicsEditor
{
    public class SaveCommand : ICommand
    {
        private Picture picture;

        public string Name => "save";
        public string Help => "Сохраняет картинку в файл";
        public string Description => "Сохраняет картинку в файл, принимает единственный аргумент" +
                                     " - имя файла. (путь к нему)";
        public string[] Synonyms => new[] {"sv"};

        public SaveCommand(Picture picture)
        {
            this.picture = picture;
        }
        
        public void Execute(params string[] parameters)
        {
            string fileName;
            try
            {
                fileName = ParameterParser.ProcessSaveOrLoadParams(this, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var sb = new StringBuilder();

            sb.AppendLine("[");
            for (var i = 0; i < picture.shapes.Count; i++)
            {
                var shape = picture.shapes[i];
                sb.AppendLine(ConvertShapeToCommand(shape));
                if (i != picture.shapes.Count - 1)
                {
                    sb.AppendLine(",");
                }
            }
            sb.AppendLine("]");
            
            using (var sw = new StreamWriter(fileName))
            {
                sw.Write(sb.ToString());
            } 
        }
        
        private string ConvertShapeToCommand(IShape shape)
        {
            string command = null;
            if (shape is Point point)
            {
                command = $"\"point {point.Coordinates.X} {point.Coordinates.Y}\"";
            }
            else if (shape is Line line)
            {
                command = $"\"line {line.StartPnt.X} {line.StartPnt.Y} {line.EndPnt.X} {line.EndPnt.Y}\"";
            }
            else if (shape is Circle circle)
            {
                command = $"\"circle {circle.Center.X} {circle.Center.Y} {circle.Radius}\"";
            }
            else if (shape is Ellipse ellipse)
            {
                command = $"\"ellipse {ellipse.Center.X} {ellipse.Center.Y} " +
                          $"{ellipse.Size.Width} {ellipse.Size.Height} {ellipse.RotateAngle}\"";
            }
            else if (shape is CompoundShape compoundShape)
            {
                var sb = new StringBuilder();
                sb.Append("[");
                
                for (var i = 0; i < compoundShape.Shapes.Count; i++)
                {
                    var child = compoundShape.Shapes[i];
                    sb.Append(ConvertShapeToCommand(child));
                    if (i != compoundShape.Shapes.Count - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.Append("]");

                command = sb.ToString();
            }

            return command;
        }
    }
}