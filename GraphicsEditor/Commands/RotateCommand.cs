using System;
using System.Drawing;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor
{
    public class RotateCommand : BaseEdit
    {
        public override string Name => "rotate";

        public override string Help => "Повернуть фигуры";

        public override string[] Synonyms => new string[] { };

        public override string Description =>
            "Введите: rotate <X> <Y> <angle> <index> чтобы повернуть фигуру относительно точки";

        public RotateCommand(Picture picture) : base(picture)
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
                    var trans = Transformation.Translate(
                                    new PointF(args[0], -args[1]))
                                * Transformation.Rotate(args[2])
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