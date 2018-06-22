using System;
using System.Linq;
using ConsoleUI;
using GraphicsEditor.Select;

namespace GraphicsEditor
{
    public class RedoCommand : ICommand
    {
        public string Name => "redo";
        public string Help => "Возвращает отменённое действие";
        public string Description => "Введите 'redo', чтобы вернуть отменённое действие";
        public string[] Synonyms => new string[] {};

        private Picture picture;

        public RedoCommand(Picture picture)
        {
            this.picture = picture;
        }
        
        public void Execute(params string[] parameters)
        {

            if (parameters.Length > 0)
            {
                Console.WriteLine("redo не нужны аргументы");
                return;
            }
            
            var shapes = CommandHistoryContainer.GetInstance().OnRedo();

            if (shapes == null)
            {
                Console.WriteLine("Нет действией, которые можно было бы вернуть");
                return;
            }

            var currentShapes = picture.shapes;
            
            foreach (var shape in currentShapes.ToList())
            {
                picture.Remove(shape);
            }

            foreach (var shape in shapes)
            {
                picture.Add(shape);
            }
            
            SelectionContainer.GetInstance().OnUndo(picture.shapes);
        }
    }
}