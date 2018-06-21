using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleUI;
using System.Collections.Generic;

namespace GraphicsEditor
{
    public class GroupCommand : ICommand
    {
        Picture picture;

        public string Name => "group";

        public string Help => "Сгруппировать фигуры";

        public string[] Synonyms => new string[] { };

        public string Description => "Введите: group <name> <index1> ... чтобы сгруппировать фигуры \n(в <name> обязательно должна содержаться буква и не должно быть пробелов, если в <name> содержаться только цифры то <index1>=<name> ";

        public GroupCommand(Picture picture)
        {
            this.picture = picture;
        }

        public void Execute(params string[] parameters)
        {
            try
            {
                if (parameters.Length < 1)
                {
                    throw new ArgumentException("Отсуствует аргумент");
                }

                picture.Group(parameters);
                
                CommandHistoryContainer.GetInstance().OnEdit();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
