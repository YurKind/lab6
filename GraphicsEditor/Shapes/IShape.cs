using DrawablesUI;

namespace GraphicsEditor
{
    public interface IShape : IDrawable
    {
        void Transform(Transformation trans);
        IShape Clone();
        string ToSvg();
        string UID { get; set; }
    }
}