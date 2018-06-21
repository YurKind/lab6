using System;
using System.Drawing;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor
{
    public class TranslateCommand : BaseEdit
    {
        public override string Name => "translate";

        public override string Help => "Переместить фигуры";

        public override string[] Synonyms => new string[] { };

        public override string Description => "Введите: <X> <Y> <index>  чтобы переместить фигуру на X и Y";

        public TranslateCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            try
            {
                var shapeIndex = 2;

                float[] args;

                if (shapeIndex < parameters.Length - 1 || shapeIndex > parameters.Length)
                {
                    throw new ArgumentException("Неверное число параметров. Помощь по команде:\n" +
                                                Description);
                }

                args = ParameterParser.ParseToFloat(shapeIndex == parameters.Length
                    ? parameters.Take(parameters.Length).ToArray()
                    : parameters.Take(parameters.Length - 1).ToArray());

                var shapes = GetShapes(parameters, shapeIndex + 1);
                var point = new PointF(args[0], args[1]);
                
                foreach (var shape in shapes)
                {
                    shape.Transform(Transformation.Translate(point));
                    picture.Update();
                }

                UpdateHistory();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}