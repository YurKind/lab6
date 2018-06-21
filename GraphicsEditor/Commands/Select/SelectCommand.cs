using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor.Select
{
    public class SelectCommand : ICommand
    {
        private Picture picture;
        
        public string Name => "select";
        public string Help => "Выбирает несколько фигур для дальнейшего редактирования";
        public string Description => "Введите 'select' <идентификатор_1> ... <индентификатор_n> для " +
                                     "выбора";
        public string[] Synonyms => new string[] { };

        public SelectCommand(Picture picture)
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
            
                SelectionContainer.GetInstance().SetSelection(shapeLocators.Select(sl => sl.Shape).ToList());
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}