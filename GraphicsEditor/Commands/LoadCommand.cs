using System;
using System.Collections.Generic;
using System.IO;
using ConsoleUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphicsEditor
{
    public class LoadCommand : ICommand
    {
        private Application app;
        private Picture picture;

        public string Name => "load";
        public string Help => "Загружает картинку из файла";

        public string Description => "Загружает картинку из файла. Принимает один аргумент - " +
                                     "имя файла (путь к нему)";

        public string[] Synonyms => new[] {"ld"};

        public LoadCommand(Application app, Picture picture)
        {
            this.app = app;
            this.picture = picture;
        }

        public void Execute(params string[] parameters)
        {
            var fileName = "";
            try
            {
                fileName = ParameterParser.ProcessSaveOrLoadParams(this, parameters);

                using (var sr = new StreamReader(fileName))
                {
                    var jsonString = sr.ReadToEnd();
                    var values = JsonConvert.DeserializeObject<List<object>>(jsonString);
                    foreach (var value in values)
                    {
                        ConvertValueToCommands(value);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"По указанному пути ({fileName}) файл не найден!");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ConvertValueToCommands(object value)
        {
            var commands = new List<string>();
            if (value is JArray array)
            {
                foreach (var shape in array)
                {
                    ConvertValueToCommands(shape);
                }

                commands.Add($"group {CreateGroupIndexes(picture.shapes.Count, array.Count)}");
                ExecuteCommands(commands);
            }

            if (value is JValue com)
            {
                commands.Add(com.ToString());
                ExecuteCommands(commands);
            }
            else if (value is string)
            {
                commands.Add(value.ToString());
                ExecuteCommands(commands);
            }
        }

        private void ExecuteCommands(List<String> commands)
        {
            foreach (var cmd in commands)
            {
                var cmdline = cmd.Split(
                    new[] {' ', '\t'},
                    StringSplitOptions.RemoveEmptyEntries
                );
                if (cmdline.Length == 0)
                {
                    continue;
                }

                var args = new string[cmdline.Length - 1];
                Array.Copy(cmdline, 1, args, 0, cmdline.Length - 1);

                var command = app.FindCommand(cmdline[0]);
                command.Execute(args);
            }
        }

        private static string CreateGroupIndexes(int shapesCount, int arrayCount)
        {
            var result = "";

            for (var i = shapesCount - arrayCount; i < shapesCount; i++)
            {
                result += $"{i} ";
            }

            return result;
        }
    }
}