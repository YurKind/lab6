using System;

namespace GraphicsEditor
{
    public class DrawPointCommand : BaseDraw
    {
        public override string Name => "point";
        public override string Help => "Рисует точку";

        public override string Description => "Рисует на экране точку в указанных координатах, принимает два " +
                                              "аргумента: координата по оси X (абсцисс) и Y (ординат)";

        public override string[] Synonyms => new[] {"p", "pnt"};
        public override int ArgumentsNumber => 2;

        public DrawPointCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            float x, y;

            try
            {
                ProcessDrawParams(parameters, out var commandAgrs);

                x = commandAgrs[0];
                y = commandAgrs[1];
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            picture.Add(new Point(x, y, Guid.NewGuid().ToString()));
            
            UpdateHitory();
        }

        protected override string NameParams()
        {
            return "координаты по оси Х и Y";
        }
    }
}