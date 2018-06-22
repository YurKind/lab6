using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI;
using GraphicsEditor.Select;

namespace GraphicsEditor
{
    public class RemoveCommand : BaseEdit
    {

        public override string Name => "remove";
        public override string Help => "Удаляет фигуру с картинки по ее индексу";

        public override string Description => "Удаляет фигуру с картинки по ее индексу. Примает как\n" +
                                     " аргумент список индексов фигур. (индексация начинается с 0, несколько" +
                                     " чисел через пробел.)";

        public override string[] Synonyms => new[] {"rm"};

        public RemoveCommand(Picture picture) : base(picture)
        {
        }

        public override void Execute(params string[] parameters)
        {
            try
            {
                var uniqueIndexes = new List<string>();
                var shapeLocators = new List<ShapeLocator>();

                if (parameters.Length < 1)
                {
                    throw new ArgumentException("Отсуствует аргумент");
                }

                foreach (var parameter in parameters)
                {
                    if (uniqueIndexes.IndexOf(parameter) == -1)
                    {
                        uniqueIndexes.Add(parameter);
                        shapeLocators.Add(ShapeLocator.Parse(parameter, picture));
                    }
                }

                foreach (var shapeLocator in shapeLocators)
                {
                    var shape = shapeLocator.Shape;
                    var parent = shapeLocator.Parent;
                    
                    if (parent != null)
                    {
                        parent.Shapes.Remove(shape);
                        
                        if (parent.Shapes.Count < 2)
                        {
                            var grandParent = shapeLocator.GrandParent;
                            if (grandParent != null)
                            {
                                grandParent.Shapes.Add(parent.Shapes[0]);
                                grandParent.Shapes.Remove(parent);
                                SelectionContainer.GetInstance().AddSelection(parent.Shapes[0]);
                                SelectionContainer.GetInstance().OnMainRemove(parent);
                            }
                            else
                            {
                                picture.Add(parent.Shapes[0]);
                                picture.Remove(parent);
                                SelectionContainer.GetInstance().AddSelection(parent.Shapes[0]);
                                SelectionContainer.GetInstance().OnMainRemove(parent);
                            }
                        }
                    }
                    else
                    {
                        SelectionContainer.GetInstance().OnMainRemove(shape);
                        picture.Remove(shape);
                    }
                }
                UpdateHistory();
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}