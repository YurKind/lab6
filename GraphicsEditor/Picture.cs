using System;
using System.Collections.Generic;
using DrawablesUI;
using GraphicsEditor.Select;

namespace GraphicsEditor
{
    public class Picture : IDrawable
    {
        public readonly List<IShape> shapes = new List<IShape>();
        private readonly object lockObject = new object();

        public event Action Changed;

        public void Remove(IShape shape)
        {
            lock (lockObject)
            {
                shapes.Remove(shape);
            }
        }

        public void RemoveAt(int index)
        {
            lock (lockObject)
            {
                shapes.RemoveAt(index);
                Changed?.Invoke();
            }
        }

        public void RemoveAt(int[] index)
        {
            if (index.Length > 1)
            {
                lock (lockObject)
                {
                    var underPosition = new int[index.Length - 1];
                    Array.Copy(index, 0, underPosition, 0, index.Length - 1);
                    var shape = ShapeAt(underPosition);

                    if (shape is CompoundShape compoundShape)
                    {
                        compoundShape.Shapes.RemoveAt(index[index.Length - 1]);
                        if (compoundShape.Shapes.Count == 0)
                        {
                            RemoveAt(underPosition);
                        }
                    }
                }
            }
            else
            {
                RemoveAt(index[0]);
            }

            Changed?.Invoke();
        }

        public void Add(IShape shape)
        {
            lock (lockObject)
            {
                shapes.Add(shape);
                if (Changed != null)
                {
                    Changed();
                }
            }
        }

        public void Add(int index, IShape shape)
        {
            lock (lockObject)
            {
                shapes.Insert(index, shape);
                if (Changed != null)
                    Changed();
            }
        }

        public void Update()
        {
            lock (lockObject)
            {
                Changed();
            }
        }

        public IShape ShapeAt(int index)
        {
            lock (lockObject)
            {
                return shapes[index];
            }
        }

        public IShape ShapeAt(int[] position)
        {
            if (position.Length > 1)
            {

                var underPosition = new int[position.Length - 1];
                Array.Copy(position, 1, underPosition, 0, position.Length - 1);
                return Search(underPosition, shapes[position[0]]);
            }
            else if (position[0] >= 0 && position[0] < shapes.Count)
            {
                return shapes[position[0]];
            }
            else
            {
                throw new ArgumentException("Элемент с указанным индексом не существует");
            }
        }
        private IShape Search(int[] position, IShape shape)
        {
            if (shape is CompoundShape)
            {
                var compoundShape = shape as CompoundShape;
                if (position.Length != 1)
                {
                    if (position[0] >= 0 && position[0] < compoundShape.Shapes.Count)
                    {
                        var underPosition = new int[position.Length - 1];
                        Array.Copy(position, 1, underPosition, 0, position.Length - 1);

                        return Search(underPosition, compoundShape.Shapes[position[0]]);
                    }
                    else
                    {
                        throw new ArgumentException("Элемент с указанным индексом не существует");
                    }
                }
                else
                {
                    return compoundShape.Shapes[position[0]];
                }
            }
            else
            {
                throw new ArgumentException("Элемент с указанным индексом не существует");
            }
        }

        public void Group(string[] indexes)
        {
            var positions = new List<string>();
            foreach (var index in indexes)
            {
                if (positions.IndexOf(index) == -1)
                {
                    bool correct = true;
                    foreach (var position in positions)
                    {
                        if (index.Length > position.Length && index.StartsWith(position))
                        {
                            correct = false;
                            break;
                        }
                        else if (index.Length < position.Length && position.StartsWith(index))
                        {
                            positions.Remove(position);
                        }
                    }
                    if (correct)
                    {
                        positions.Add(index);
                    }
                }
            }

            if (positions.Count < 2)
            {
                throw new ArgumentException("Для группировки необходимы минимум 2 фигуры");
            }

            lock (lockObject)
            {
                int j = 0;
                var shapes = new ShapeLocator[positions.Count];
                foreach (var position in positions)
                {
                    shapes[j] = ShapeLocator.Parse(position, this);

                    j++;
                }

                CompoundShape compoundShape = new CompoundShape();
                foreach (var shape in shapes)
                {
                    compoundShape.Shapes.Add(shape.Shape);
                    if (shape.Parent != null)
                    {
                        shape.Parent.Shapes.Remove(shape.Shape);
                    }
                    else
                    {
                        Remove(shape.Shape);
                    }
                }

                foreach (var shape in shapes)
                {
                    if (shape.Parent != null && shape.Parent.Shapes.Count < 2)
                    {
                        if (shape.GrandParent != null)
                        {
                            if (shape.Parent.Shapes.Count == 1)
                            {
                                shape.GrandParent.Shapes.Add(shape.Parent.Shapes[0]);
                            }
                            shape.GrandParent.Shapes.Remove(shape.Parent);
                        }
                        else
                        {
                            if (shape.Parent.Shapes.Count == 1)
                            {
                                Add(shape.Parent.Shapes[0]);
                            }
                            Remove(shape.Parent);
                        }
                    }
                }
                Add(compoundShape);
            }
        }

        public void Ungroup(string[] indexes)
        {
            var shape = ShapeLocator.Parse(indexes[0], this);
            SelectionContainer.GetInstance().OnUngroup(shape);
            if (shape.Shape is CompoundShape)
            {
                var compoundShape = shape.Shape as CompoundShape;
                if (shape.Parent != null)
                {
                    shape.Parent.Shapes.Remove(shape.Shape);
                    foreach (var currentShape in compoundShape.Shapes)
                    {
                        shape.Parent.Shapes.Add(currentShape);
                    }
                }
                else
                {
                    Remove(shape.Shape);
                    foreach (var currentShape in compoundShape.Shapes)
                    {
                        Add(currentShape);
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Фигура с указанным индексом '{indexes[0]}' не составная");
            }
        }
        public int CountShapes()
        {
            return shapes.Count;
        }

        public void Draw(IDrawer drawer)
        {
            lock (lockObject)
            {
                foreach (var shape in shapes)
                {
                    shape.Draw(drawer);
                }
            }
        }
    }
}