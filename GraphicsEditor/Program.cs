using System;
using ConsoleUI;
using DrawablesUI;
using GraphicsEditor.Select;
using GraphicsEditor.Svg;

namespace GraphicsEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var picture = new Picture();
            var ui = new DrawableGUI(picture);
            var app = new Application();

            app.AddCommand(new ExitCommand(app));
            app.AddCommand(new ExplainCommand(app));
            app.AddCommand(new HelpCommand(app));
            app.AddCommand(new DrawPointCommand(picture));
            app.AddCommand(new DrawLineCommand(picture));
            app.AddCommand(new DrawEllipseCommand(picture));
            app.AddCommand(new DrawCircleCommand(picture));
            app.AddCommand(new ListCommand(picture));
            app.AddCommand(new RemoveCommand(picture));
            app.AddCommand(new RotateCommand(picture));
            app.AddCommand(new ScaleCommand(picture));
            app.AddCommand(new TranslateCommand(picture));
            app.AddCommand(new GroupCommand(picture));
            app.AddCommand(new UngroupCommand(picture));
            app.AddCommand(new SelectionListCommand());
            app.AddCommand(new SelectCommand(picture));
            app.AddCommand(new SelectionAddCommand(picture));
            app.AddCommand(new SelectionRemoveCommand(picture));
            app.AddCommand(new UndoCommand(picture));
            app.AddCommand(new RedoCommand(picture));
            app.AddCommand(new SaveCommand(picture));
            app.AddCommand(new LoadCommand(app, picture));
            app.AddCommand(new ExportCommand(picture, new SvgExporter()));

            CommandHistoryContainer.Init(picture);

            picture.Changed += ui.Refresh;
            ui.Start();
            app.Run(Console.In);
            ui.Stop();
        }
    }
}