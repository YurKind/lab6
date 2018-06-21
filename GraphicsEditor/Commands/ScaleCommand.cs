using System;
using System.Drawing;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor
{
    public class ScaleCommand : BaseEdit
    {
        public override string Name => "scale";

        public override string Help => "Масштабировать фигуры";

        public override string[] Synonyms => new string[] { };

        public override string Description =>
            " Введите: scale <X> <Y> <scale factor> <index> чтобы отмасштабировать фигур относительно точки";

        public ScaleCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            try
            {
                var shapeIndex = 3;
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

                foreach (var shape in shapes)
                {
                    var trans = Transformation.Translate(new PointF(args[0],
                                    -args[1]))
                                * Transformation.Scale(
                                    new PointF(args[0], args[1]),
                                    args[2])
                                * Transformation.Translate(new PointF(args[0],
                                    args[1]));

                    shape.Transform(trans);
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