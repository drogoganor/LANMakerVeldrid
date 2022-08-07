using ImGuiNET;
using LANMaker.SampleBase;
using System.Numerics;

#nullable disable

namespace LANMaker.Components
{
    public class InstalledGamesView : IMenuComponent
    {
        private readonly IApplicationWindow _window;

        public InstalledGamesView(IApplicationWindow window)
        {
            _window = window;
        }

        public void Draw(float deltaSeconds, Vector2 position)
        {
            var sidebarWidth = 410;
            var windowSize = new Vector2(_window.Width, _window.Height);
            var menuSize = new Vector2(_window.Width - (sidebarWidth + 10), _window.Height - 20);
            var menuPadding = 0f;
            ImGui.SetNextWindowSize(menuSize);

            var menuPos = new Vector2(sidebarWidth, 10);
            ImGui.SetNextWindowPos(menuPos);
            //ImGui.PushFont(font.Value);

            if (ImGui.Begin("Installed Games",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoResize))
            {
                //HorizontallyCenteredText("Installed Games", menuSize.X);
            }

            //base.Draw(deltaSeconds);
        }
    }
}
