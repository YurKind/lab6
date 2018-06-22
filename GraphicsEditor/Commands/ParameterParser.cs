using System;
using System.Linq;
using ConsoleUI;

namespace GraphicsEditor
{
    public class ParameterParser
    {
        public static float[] ParseToFloat(string[] rawParams)
        {   
            var result = new float[rawParams.Length];

            for (var i = 0; i < result.Length; i++)
            {
                var currentParam = rawParams[i].Replace('.', ',');
                try
                {
                    var arg = float.Parse(currentParam);
                    result[i] = arg;
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"Неверный параметр: '{currentParam}' на позиции {i + 1}.");
                }
                catch (OverflowException)
                {
                    throw new ArgumentException($"Неверный параметр: '{currentParam}' на позиции {i + 1}. " +
                                                "Число за педелами допустимых значений. Введите число от " +
                                                      $"{float.MinValue} до {float.MaxValue}");
                }
            }

            return result;
        }

        public static int[] ProcessRemoveParams(string [] rawParams, int numberOfShapes)
        {
            if (rawParams.Length == 0)
            {
                throw new ArgumentException("Для этой команды нужен хотя бы один аргумент!");
            }
            if (numberOfShapes == 0)
            {
                throw new ArgumentException("На картинке нет фигур! Прежде чем удалять, добавьте фигуры!");
            }
            int[] result = new int[rawParams.Length];
            for(var i = 0; i < rawParams.Length; i++)
            {
                var currentParam = rawParams[i];
                try
                {
                    var arg = int.Parse(currentParam);
                    if (arg < 0)
                    {
                        throw new ArgumentException("Индекс не может быть меньше нуля!");
                    }
                    var lastIndex = numberOfShapes - 1;
                    if (arg > lastIndex)
                    {
                        throw new ArgumentException(
                            "Указан слишком большой индекс!\n" +
                            $"В данный момент на картинке всего {numberOfShapes} фигур." +
                            $" (Максимальный возможный индекс - {lastIndex})");
                    }
                    result[i] = arg;
                }
                catch (FormatException)
                {
                    throw new ArgumentException(
                        $"Неверный параметр: '{currentParam}' на позиции {i + 1}." +
                        " Аргументами этой команды могут быть только целые числа!");
                }
            }

            return result.Distinct().ToArray();
        }

        public static string ProcessSaveOrLoadParams(ICommand command, string[] rawParams)
        {
            if (rawParams.Length != 1)
            {
                throw new ArgumentException(
                    $"Для команды '{command.Name}' " +
                     "нужен один агрумент - имя файла (путь к нему)."
                    );
            }

            return rawParams[0];
        }
        
        public static void CheckWidthAndHeight(float width, float height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Неверные аргументы! Высота и ширина должны быть больше 0!");
            }
        }

        public static void CheckRadius(float radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Неверный аргумент! Радиус должен быть больше 0");
            }
        }
    }
}