using System;
using ConsoleUI;

namespace GraphicsEditor
{
    public class DrawCircleCommand : BaseDraw
    {
        public override string Name => "circle";
        public override string Help => "Рисует круг";

        public override string Description => "Рисует круг с центром в указанных координатах и данным радиусом, " +
                                              "принимает 3 аргумента: коордианты центра окружности по оси Х (абсцисс) " +
                                              "и Y (ординат) а так же радиус.";

        public override string[] Synonyms => new[] {"cl"};
        public override int ArgumentsNumber => 3;

        public DrawCircleCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            float x, y, radius;

            try
            {
                float[] commandAgrs;
                ProcessDrawParams(parameters, out commandAgrs);
                x = commandAgrs[0];
                y = commandAgrs[1];
                radius = commandAgrs[2];


                var circle = new Circle(x, y, radius, Guid.NewGuid().ToString());
                picture.Add(circle);
                
                UpdateHitory();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override string NameParams()
        {
            return "координаты центра окружности по оси X и Y, а так же радиус";
        }
    }
}