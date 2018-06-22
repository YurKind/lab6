using System.Collections.Generic;
using System.Linq;

namespace GraphicsEditor
{
    public class CommandHistoryContainer
    {
        private static Picture picture;

        public Stack<List<IShape>> ToUndoStack { get; set; } = new Stack<List<IShape>>();
        public Stack<List<IShape>> ToRedoStack { get; set; } = new Stack<List<IShape>>();

        private static CommandHistoryContainer container;

        private CommandHistoryContainer()
        {
        }

        public static CommandHistoryContainer GetInstance()
        {
            return container;
        }

        public static void Init(Picture pic)
        {
            container = new CommandHistoryContainer();
            container.ToUndoStack.Push(new List<IShape>());
            picture = pic;
        }

        public void OnEdit()
        {
            if (!picture.shapes.Any(shape => shape is CompoundShape))
            {
                ToUndoStack.Push(new List<IShape>(picture.shapes.Select(shape => shape.Clone()).ToList()));
            }
            else
            {
                var cloneList = new List<IShape>();
                foreach (var shape in picture.shapes)
                {
                    if (shape is CompoundShape compoundShape)
                    {
                        cloneList.Add(GetAllCompoundShapeClones(compoundShape));
                    }
                    else
                    {
                        cloneList.Add(shape.Clone());
                    }
                }

                ToUndoStack.Push(cloneList);
            }

            ToRedoStack = new Stack<List<IShape>>();
        }

        public List<IShape> OnUndo()
        {
            if (!IsStackEmpty())
            {
                ToRedoStack.Push(ToUndoStack.Pop());

                var copy = ToUndoStack.Peek().Select(shape =>
                    shape is CompoundShape compoundShape ? GetAllCompoundShapeClones(compoundShape) : shape.Clone()
                ).ToList();

                return copy;
            }

            return null;
        }

        public List<IShape> OnRedo()
        {
            if (ToRedoStack.Count != 0 && ToRedoStack.Any())
            {
                var copy = ToRedoStack.Peek().Select(shape =>
                    shape is CompoundShape compoundShape ? GetAllCompoundShapeClones(compoundShape) : shape.Clone()
                ).ToList();

                ToUndoStack.Push(ToRedoStack.Pop());

                return copy;
            }

            return null;
        }

        private IShape GetAllCompoundShapeClones(CompoundShape compoundShape)
        {
            var clone = compoundShape.Clone();
            ((CompoundShape) clone).Shapes = compoundShape.Shapes
                .Select(s => s is CompoundShape shape ? GetAllCompoundShapeClones(shape) : s.Clone()).ToList();
            return clone;
        }

        private bool IsStackEmpty()
        {
            return ToUndoStack.Count == 1 && ToUndoStack.Peek().Count == 0;
        }
    }
}