using System;

namespace GraphicsEditor
{
    public class DrawEllipseCommand : BaseDraw
    {
        public override string Name => "ellipse";
        public override string Help => "Рисует эллипс";
        public override string Description => "Рисует эллипс с центром в указанных координатах" +
                                              "с задаными шириной и высотой, а так же углом наклона, принимает" +
                                              "5 аргументов: координаты центра эллипса по оси Х (абсцисс) и " +
                                              "Y (ординат), ширину и высоту, а так же угол наклона.";
        public override string[] Synonyms => new[] {"el"};
        public override int ArgumentsNumber => 5;

        public DrawEllipseCommand(Picture picture) : base(picture)
        {
        }
        
        public override void Execute(params string[] parameters)
        {
            float x, y, width, height, rotateAngle;
            try
            {
                float[] commandArgs;
                ProcessDrawParams(parameters, out commandArgs);
                
                x = commandArgs[0];
                y = commandArgs[1];
                width = commandArgs[2];
                height = commandArgs[3];
                rotateAngle = commandArgs[4];

                var ellipse = new Ellipse(x, y, width, height, rotateAngle, Guid.NewGuid().ToString());

                picture.Add(ellipse);
                UpdateHitory();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            } 
        }

        protected override string NameParams()
        {
            return "координаты центра эллипса по оси Х и Y, его ширина, высота, а так же угол наклона";
        }
    }
}