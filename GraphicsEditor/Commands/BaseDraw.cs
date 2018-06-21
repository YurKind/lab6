using System;
using ConsoleUI;

namespace GraphicsEditor
{
    public abstract class BaseDraw : ICommand
    {
        protected Picture picture;

        public abstract string Name { get; }
        public abstract string Help { get; }
        public abstract string Description { get; }
        public abstract string[] Synonyms { get; }
        public abstract int ArgumentsNumber { get; }
        
        public abstract void Execute(params string[] parameters);

        protected BaseDraw(Picture picture)
        {
            this.picture = picture;
        }

        public void UpdateHitory()
        {
            CommandHistoryContainer.GetInstance().OnEdit();
        }
        
        protected void ProcessDrawParams(string[] rawParams, out float[] arguments)
        {
            if (rawParams.Length != ArgumentsNumber)
            {
                throw new ArgumentException(
                    "Вы ввели неверное количество параметров.\n" +
                    $"Для команды {Name} ({string.Join(", ", Synonyms)}) " +
                    $"нужно {ArgumentsNumber} параметра: \n{NameParams()}.");
            }
            
            arguments = ParameterParser.ParseToFloat(rawParams);
            
            const int maxValue = 1000000000;
            foreach (var arg in arguments)
            {
                if (arg > maxValue)
                {
                    throw new ArgumentException(
                        $"Неверный параметр: '{arg}' \n" +
                        "Вы ввели слишком большое число! " +
                        $"Попробуйте меньше, чем {maxValue}");
                }
            }
        }

        protected abstract string NameParams();
    }
}