using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor.Select
{
     public class SelectionRemoveCommand : ICommand
    {
        private Picture picture;

        public string Name => "selection-remove";
        public string Help => "Отменяет выбор";
        public string Description => "";
        public string[] Synonyms => new[] { "sr" };

        public SelectionRemoveCommand(Picture picture)
        {
            this.picture = picture;
        }
        
        public void Execute(params string[] parameters)
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
                        shapeLocators.Add(ShapeLocator.Parse(parameter));
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
                                /*SelectionContainer.GetInstance().AddSelection(parent.Shapes[0]);
                                SelectionContainer.GetInstance().RemoveSelection(parent)*/;
                            }
                            else
                            {
                                SelectionContainer.GetInstance().Shapes.Add(parent.Shapes[0]);
                                SelectionContainer.GetInstance().Shapes.Remove(parent);
                                /*SelectionContainer.GetInstance().AddSelection(parent.Shapes[0]);
                                SelectionContainer.GetInstance().RemoveSelection(parent)*/;
                            }
                        }
                        
                        parent.Shapes.Add(shape);
                    }
                    else
                    {
                        SelectionContainer.GetInstance().RemoveSelection(shape);
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}