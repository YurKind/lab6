using System;
using System.Linq;
using System.Windows.Input;
using GraphicsEditor.Select;
using ICommand = ConsoleUI.ICommand;

namespace GraphicsEditor
{
    public class UndoCommand : ICommand
    {
        public string Name => "undo";
        public string Help => "Отменяет предыдущее действие";
        public string Description => "Введите 'undo', чтобы отменить предыдущее действие";
        public string[] Synonyms => new string[] { };

        private Picture picture;

        public UndoCommand(Picture picture)
        {
            this.picture = picture;
        }

        public void Execute(params string[] parameters)
        {
            var shapes = CommandHistoryContainer.GetInstance().OnUndo();

            if (shapes == null)
            {
                Console.WriteLine("Нет действий, которые можно было бы откатить");
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