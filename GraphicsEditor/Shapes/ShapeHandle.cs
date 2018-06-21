namespace GraphicsEditor
{
    public class ShapeHandle
    {
        public CompoundShape Parent { get; set; }
        public IShape Shape { get; set; }

        public ShapeHandle(CompoundShape parent, IShape shape)
        {
            Parent = parent;
            Shape = shape;
        }
    }
}
