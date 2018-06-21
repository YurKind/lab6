using System;
using System.IO;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor
{
    public class ListCommand : ICommand
    {
        private Picture picture;
        
        public string Name => "list";
        public string Help => "Выводит список фигур на картинке";
        public string Description => "Выводит список фигур на картинке, не принимает аргументов";
        public string[] Synonyms => new[] {"ls"};

        public ListCommand(Picture picture)
        {
            this.picture = picture;
        }

        public void Execute(params string[] parameters)
        {
            int i = 0;
            foreach (var shape in picture.shapes)
            {
                string path = "";
                path += i;

                if (shape is CompoundShape)
                {
                    var cshape = shape as CompoundShape;
                    Console.Write(cshape.ToString(path));
                }
                else
                {
                    Console.WriteLine(String.Format("[{0}] {1}", i, shape.ToString()));
                }

                i++;
            }
        }
    }
}