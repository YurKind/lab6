using DrawablesUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEditor.Select;

namespace GraphicsEditor
{
    public class ShapeLocator
    {
        public IShape Shape { get; private set; }
        public CompoundShape Parent { get; private set; }
        public CompoundShape GrandParent { get; private set; }

        public static ShapeLocator Parse(string identifier)
        {
            var shapeLocator = new ShapeLocator();
            var identifierDecomp = identifier.Split(':');
            int index;
            if (identifierDecomp.Length > 0 && int.TryParse(identifierDecomp[0], out index))
            {
                try
                {
                    shapeLocator.Shape = SelectionContainer.GetInstance().Shapes[index];
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentException("Фигуры с таким индетефикатором " + identifier + " не существует");
                }
            }
            else
            {
                throw new ArgumentException("Формат идеентификатора '" + identifier + "' не верен");
            }

            if (identifierDecomp.Length > 1)
            {
                int[] underPosition = new int[identifierDecomp.Length - 1];
                for (int i = 0; i < identifierDecomp.Length - 1; i++)
                {
                    if (!int.TryParse(identifierDecomp[i + 1], out underPosition[i]))
                    {
                        throw new ArgumentException("Формат идеентификатора '" + identifier + "' не верен");
                    }
                }
                Search(shapeLocator, underPosition);
            }

            return shapeLocator;
        }

        
        public static ShapeLocator Parse(string identifier, Picture picture)
        {
            var shapeLocator = new ShapeLocator();
            var identifierDecomp = identifier.Split(':');
            int index;
            if (identifierDecomp.Length > 0 && int.TryParse(identifierDecomp[0], out index))
            {
                try
                {
                    shapeLocator.Shape = picture.ShapeAt(index);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentException("Фигуры с таким индетефикатором " + identifier + " не существует");
                }
            }
            else
            {
                throw new ArgumentException("Формат идеентификатора '" + identifier + "' не верен");
            }

            if (identifierDecomp.Length > 1)
            {
                int[] underPosition = new int[identifierDecomp.Length - 1];
                for (int i = 0; i < identifierDecomp.Length - 1; i++)
                {
                    if (!int.TryParse(identifierDecomp[i + 1], out underPosition[i]))
                    {
                        throw new ArgumentException("Формат идеентификатора '" + identifier + "' не верен");
                    }
                }
                Search(shapeLocator, underPosition);
            }

            return shapeLocator;
        }
        private static void Search(ShapeLocator shapeLocator, int[] index)
        {
            if (shapeLocator.Shape is CompoundShape)
            {
                var compoundShape = shapeLocator.Shape as CompoundShape;
                if (index.Length != 1)
                {
                    if (index[0] >= 0 && index[0] < compoundShape.Shapes.Count)
                    {
                        var underPosition = new int[index.Length - 1];
                        Array.Copy(index, 1, underPosition, 0, index.Length - 1);
                        if (index.Length < 3)
                        {
                            shapeLocator.GrandParent = shapeLocator.Parent;
                            shapeLocator.Parent = compoundShape;
                        }
                        shapeLocator.Shape = compoundShape.Shapes[index[0]];
                        Search(shapeLocator, underPosition);
                    }
                    else
                    {
                        throw new ArgumentException("Элемент с указанным индексом не существует");
                    }
                }
                else
                {
                    shapeLocator.GrandParent = shapeLocator.Parent;
                    shapeLocator.Parent = compoundShape;
                    shapeLocator.Shape = compoundShape.Shapes[index[0]];
                }
            }
            else
            {
                throw new ArgumentException("Элемент с указанным индексом не существует");
            }
        }
        private ShapeLocator()
        {
            Parent = null;
            GrandParent = null;
        }
    }
}
