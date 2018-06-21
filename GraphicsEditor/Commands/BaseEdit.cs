using System;
using System.Collections.Generic;
using ConsoleUI;
using GraphicsEditor.Select;

namespace GraphicsEditor
{
    public abstract class BaseEdit : ICommand
    {
        protected readonly Picture picture;
        public abstract string Name { get; }
        public abstract string Help { get; }
        public abstract string Description { get; }
        public abstract string[] Synonyms { get; }

        public abstract void Execute(params string[] parameters);

        protected BaseEdit(Picture picture)
        {
            this.picture = picture;
        }

        public void UpdateHistory()
        {
            CommandHistoryContainer.GetInstance().OnEdit();
        }

        protected IEnumerable<IShape> GetShapes(string[] parameters, int shapeIndex)
        {
            if (parameters.Length < shapeIndex)
            {
                var shapes = SelectionContainer.GetInstance().Shapes;
                if (shapes.Count == 0)
                {
                    throw new ArgumentException("Для команды необходимы аргументы либо выбор с помощью " +
                                                "команды 'select'");
                }

                return shapes;
            }

            var shape = ShapeLocator.Parse(parameters[shapeIndex - 1], picture).Shape;
            return new List<IShape> {shape};
        }
    }
}