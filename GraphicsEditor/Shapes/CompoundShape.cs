using System.Collections.Generic;
using DrawablesUI;
using System;
using System.Linq;
using System.Text;

namespace GraphicsEditor
{
    public class CompoundShape : IShape
    {
        public IList<IShape> Shapes { get; set; }
        public string name;
        public string UID { get; set; }

        public CompoundShape()
        {
            UID = Guid.NewGuid().ToString();
            Shapes = new List<IShape>();
            name = "Составная фигура";
        }

        public void Filling(string[] arg)
        {
        }

        public virtual void Draw(IDrawer drawer)
        {
            foreach (var shape in Shapes)
            {
                shape.Draw(drawer);
            }
        }

        public void Transform(Transformation transform)
        {
            foreach (var shape in Shapes)
            {
                shape.Transform(transform);
            }
        }

        public string Identifier { get; set; }


        public string ToString(string path)
        {
            string result = "";
            result += $"[{path}] {name}: " + '\n';
            for (var i = 0; i < Shapes.Count; i++)
            {
                var npath = path + ":" + i;
                if (Shapes[i] is CompoundShape)
                {
                    var cshape = (CompoundShape) Shapes[i];
                    result += cshape.ToString(npath);
                }
                else
                {
                    result += $"[{npath}] {Shapes[i]} \n";
                }
            }

            return result;
        }

        public bool ContainsInAnyChildren(IShape shape)
        {
            return GetAllInnerChildren(Shapes).Select(s => s.UID).Contains(shape.UID);
        }

        public IShape GetLastChild()
        {
            return Shapes[0];
        }
        
        public IEnumerable<IShape> GetAllInnerChildren(IEnumerable<IShape> children)
        {
            var result = new List<IShape>();
            foreach (var shape in children)
            {
                if (shape is CompoundShape cs)
                {
                    result.AddRange(GetAllInnerChildren(cs.Shapes));
                }

                result.Add(shape);
            }

            return result;
        }

        public void CleanShapes()
        {
            for (int i = 0; i < Shapes.Count; i++)
            {
                if (Shapes[i] is CompoundShape)
                {
                    var compoundShape = (CompoundShape) Shapes[i];

                    compoundShape.CleanShapes();

                    if (compoundShape.Shapes.Count == 1)
                    {
                        Shapes.Add(compoundShape.Shapes[0]);
                        Shapes.RemoveAt(i);
                    }

                    if (compoundShape.Shapes.Count == 0)
                    {
                        Shapes.RemoveAt(i);
                    }
                }
            }
        }

        public string ToSvg()
        {
            var builder = new StringBuilder();

            ToSvgGroup(builder);

            return builder.ToString();
        }

        private void ToSvgGroup(StringBuilder builder)
        {
            builder.AppendLine("<g>");

            foreach (var shape in Shapes)
            {
                if (shape is CompoundShape compoundShape)
                {
                    compoundShape.ToSvgGroup(builder);
                }
                else
                {
                    builder.AppendLine(shape.ToSvg());
                }
            }

            builder.AppendLine("</g>");
        }

        public IShape Clone()
        {
            return (CompoundShape) MemberwiseClone();
        }
    }
}