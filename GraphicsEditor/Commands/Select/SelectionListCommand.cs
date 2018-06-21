using System;
using ConsoleUI;

namespace GraphicsEditor.Select
{
    public class SelectionListCommand : ICommand
    {
        public string Name => "selection-list";
        public string Help => "Отображает выбранные фигуры";
        public string Description => "Введите 'selection-list' для отображения списка выбранных фигур.";
        public string[] Synonyms => new[] { "sl" };

        public void Execute(params string[] parameters)
        {
            var shapes = SelectionContainer.GetInstance().Shapes;
            Console.WriteLine(shapes.Count > 0 ? "Выбранные фигуры:" : "Ничего не выбрано");
            
            int i = 0;
            foreach (var shape in shapes)
            {
                string path = "";
                path += i;

                if (shape is CompoundShape cshape)
                {
                    Console.Write(cshape.ToString(path));
                }
                else
                {
                    Console.WriteLine($"[{i}] {shape}");
                }

                i++;
            }
        }
    }

}