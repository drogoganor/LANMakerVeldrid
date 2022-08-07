using ImGuiNET;
using LANMaker.Data.Enums;
using LANMaker.SampleBase;
using System.Numerics;

#nullable disable

namespace LANMaker.Components
{
    public class MainMenu : Menu
    {
        public event Action OnNewGame;

        private readonly string[] MenuItems = new[]
        {
            "Installed Games",
            "Store",
            "Downloads",
            "Configure"
        };

        private readonly IMenuComponent[] Menus;

        private int SelectedIndex = 0;
        private int LastSelectedIndex = 0;

        private readonly InstalledGamesView _installedGamesMenu;

        public MainMenu(
            IApplicationWindow window,
            InstalledGamesView installedGamesMenu) : base(window)
        {
            _installedGamesMenu = installedGamesMenu;
            //var installedGamesView = new InstalledGamesView(window);

            Menus = new IMenuComponent[]
            {
                _installedGamesMenu,
                _installedGamesMenu,
                _installedGamesMenu,
                _installedGamesMenu
            };

            //installedGamesView.Show();
        }

        private void NewGame()
        {
            Hide();
            OnNewGame?.Invoke();
        }

        private void ExitGame()
        {
            Hide();
            Window.Close();
        }

        protected override void Draw(float deltaSeconds)
        {
            var windowSize = new Vector2(Window.Width, Window.Height);
            var menuSize = new Vector2(400, 600);
            var menuPadding = 0f;
            var buttonSize = new Vector2(menuSize.X - menuPadding, 32);
            ImGui.SetNextWindowSize(menuSize);

            var menuPos = Vector2.Zero; // (windowSize - menuSize) / 2;
            ImGui.SetNextWindowPos(menuPos);
            ImGui.PushFont(font.Value);

            if (ImGui.Begin("Main Menu",
                ImGuiWindowFlags.NoTitleBar |
                ImGuiWindowFlags.NoDecoration |
                ImGuiWindowFlags.NoBackground |
                ImGuiWindowFlags.NoCollapse |
                ImGuiWindowFlags.NoMove |
                ImGuiWindowFlags.NoResize))
            {
                HorizontallyCenteredText("LANMaker", menuSize.X);

                ImGui.SetCursorPosX(menuPadding / 2f);

                if (ImGui.ListBox("", ref SelectedIndex, MenuItems, MenuItems.Length))
                {
                    //if (LastSelectedIndex != SelectedIndex)
                    //{
                    //    var previousView = Menus[LastSelectedIndex];
                    //    //previousView.Hide();
                    //    LastSelectedIndex = SelectedIndex;
                    //}

                    ////var selectedView = Menus[SelectedIndex];
                    ////selectedView.Show();
                };
            }

            Menus[SelectedIndex].Draw(deltaSeconds, Vector2.Zero);

            base.Draw(deltaSeconds);
        }
    }
}
