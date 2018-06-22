using System.Collections.Generic;
using System.Linq;

namespace GraphicsEditor.Select
{
    public class SelectionContainer
    {
        private SelectionContainer()
        {
        }

        private static SelectionContainer INSTANCE;

        public List<IShape> Shapes { get; private set; } = new List<IShape>();

        public static SelectionContainer GetInstance()
        {
            return INSTANCE ?? (INSTANCE = new SelectionContainer());
        }

        public void SetSelection(IEnumerable<IShape> shapes)
        {
            Shapes = new List<IShape>();
            AddSelection(shapes);
        }

        public void AddSelection(IShape shape)
        {
            if (!GetAllShapes(Shapes).Contains(shape))
            {
                Shapes.Add(shape);
            }
            
            if (shape is CompoundShape cs)
            {
                foreach (var s in Shapes.ToList())
                {
                    if (cs.ContainsInAnyChildren(s))
                    {
                        Shapes.Remove(s);
                    }
                }
            }
        }

        public void AddSelection(IEnumerable<IShape> shapes)
        {
            foreach (var shape in shapes)
            {
                AddSelection(shape);
            }
        }

        public void RemoveSelection(IShape shape)
        {
            Shapes.Remove(shape);
        }

        public void RemoveSelection(IEnumerable<IShape> shapes)
        {
            foreach (var shape in shapes)
            {
                Shapes.Remove(shape);
            }
        }

        public void OnMainRemove(IShape shape)
        {
            Shapes.Remove(shape);
        }

        public void OnUndo(IEnumerable<IShape> shapes)
        {
            var allShapes = GetAllShapes(shapes);
            Shapes = allShapes.FindAll(shape => Shapes.Select(s => s.UID).Contains(shape.UID));
        }

        private List<IShape> GetAllShapes(IEnumerable<IShape> shapes)
        {
            var result = new List<IShape>();

            foreach (var shape in shapes)
            {
                if (shape is CompoundShape compoundShape)
                {
                    result.AddRange(GetAllShapes(compoundShape.Shapes));
                }

                result.Add(shape);
            }

            return result.Distinct().ToList();
        }

        public void OnUngroup(ShapeLocator shape)
        {
            if (!Shapes.Contains(shape.Shape))
            {
                return;
            }

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
                    RemoveSelection(shape.Shape);

                    foreach (var currentShape in compoundShape.Shapes)
                    {
                        AddSelection(currentShape);
                    }
                }
            }
        }
    }
}