using System;
using ConsoleUI;
using GraphicsEditor.Svg;

namespace GraphicsEditor
{
    public class ExportCommand : ICommand
    {
        private Picture picture;
        private ISvgExporter exporter;
        
        public string Name => "export";
        public string Help => "Экспортирует картинку в файл формата 'svg'";
        public string Description => "Введите export <имя_файла> чтобы экспортировать картинку";
        public string[] Synonyms => new string[] { };

        public ExportCommand(Picture picture, ISvgExporter exporter)
        {
            this.picture = picture;
            this.exporter = exporter;
        }
        
        public void Execute(params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                Console.WriteLine("Неверный набор аргументов. Команда принимает один аргумент - имя файла!");
            }

            var fileName = parameters[0];

            try
            {
                exporter.Export(picture.shapes, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка при создании документа: {e.Message}");
            }
        }
    }
}