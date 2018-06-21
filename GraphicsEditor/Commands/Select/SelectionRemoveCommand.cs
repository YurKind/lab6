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
                    if (shapeLocator.Parent != null)
                    {
                        shapeLocator.Parent.Shapes.Remove(shapeLocator.Shape);
                        if (shapeLocator.Parent.Shapes.Count < 2)
                        {
                            if (shapeLocator.GrandParent != null)
                            {
                                shapeLocator.GrandParent.Shapes.Add(shapeLocator.Parent.Shapes[0]);
                                shapeLocator.GrandParent.Shapes.Remove(shapeLocator.Parent);
                            }
                            else
                            {
                                SelectionContainer.GetInstance().AddSelection(shapeLocator.Parent.Shapes[0]);
                                SelectionContainer.GetInstance().RemoveSelection(shapeLocator.Parent);
                            }
                        }
                    }
                    else
                    {
                        SelectionContainer.GetInstance().RemoveSelection(shapeLocator.Shape);
                    }
                }
            
                SelectionContainer.GetInstance().RemoveSelection(shapeLocators.Select(sl => sl.Shape).ToList());
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}