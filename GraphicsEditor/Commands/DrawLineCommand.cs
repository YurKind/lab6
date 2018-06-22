using System;

namespace GraphicsEditor
{
    public class DrawLineCommand : BaseDraw
    {
        public override string Name => "line";
        public override string Help => "Рисует линию";

        public override string Description => "Рисует на экране линию по указанным координатам, " +
                                              "принимает 4 аргумента: координаты" +
                                              " начала и конца отрезка по оси X (абсцисс) и Y (ординат)";

        public override string[] Synonyms => new[] {"ln"};
        public override int ArgumentsNumber => 4;

        public DrawLineCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            float startX, startY, endX, endY;

            try
            {
                float[] commandArgs;
                ProcessDrawParams(parameters, out commandArgs);

                startX = commandArgs[0];
                startY = commandArgs[1];
                endX = commandArgs[2];
                endY = commandArgs[3];
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            var line = new Line(startX, startY, endX, endY, Guid.NewGuid().ToString());

            picture.Add(line);

            UpdateHitory();
        }

        protected override string NameParams()
        {
            return "координаты начала и конца отрезка по оси Х и Y";
        }
    }
}