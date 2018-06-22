using System;

namespace GraphicsEditor
{
    public class UngroupCommand : BaseEdit
    {
        public override string Name => "ungroup";

        public override string Help => "Разгруппировать фигуры";

        public override string[] Synonyms => new string[] { };

        public override string Description => "Введите: ugroup <index1> ... Чтобы разгруппировать";

        public UngroupCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            try
            {
                if (parameters.Length != 1)
                {
                    throw new ArgumentException("Должен быть один параметр");
                }

                picture.Ungroup(parameters);

                UpdateHistory();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
