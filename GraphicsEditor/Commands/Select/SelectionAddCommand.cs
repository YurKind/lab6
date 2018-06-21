using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor.Select
{
    public class SelectionAddCommand : ICommand
    {
        private Picture picture;

        public string Name => "selection-add";
        public string Help => "Добавляет указанные фигуры к выбору";
        public string Description => "Введите 'selection-add' <идентификатор_1> ... <идентификатор_n> " +
                                     "чтобы добавить указанные фигуры к выбору.";
        public string[] Synonyms => new[] { "sa" };

        public SelectionAddCommand(Picture picture)
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
                        shapeLocators.Add(ShapeLocator.Parse(parameter, picture));
                    }
                }
            
                SelectionContainer.GetInstance().AddSelection(shapeLocators.Select(sl => sl.Shape).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}