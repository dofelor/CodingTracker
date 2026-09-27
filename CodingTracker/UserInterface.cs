using System;
using System.Collections.Generic;
using System.Text;
using static CodingTracker.Models.Enums.MenuAction;
using Spectre.Console;
using CodingTracker.Controllers;

namespace CodingTracker
{
    internal class UserInterface
    {
        private readonly ICodingSessionsController _codingSessionController;

        public UserInterface(ICodingSessionsController codingSessionsController)
        {
            _codingSessionController = codingSessionsController;
        }

        internal void MainMenu()
        {
            AnsiConsole.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                AnsiConsole.MarkupLine("Coding tracker\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuAction>()
                    .Title("Select an action:")
                    .AddChoices(Enum.GetValues<MainMenuAction>()));

                switch (choice)
                {
                    case MainMenuAction.Exit:
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupLine("Thank you!");
                        closeApp = true;
                        break;
                    case MainMenuAction.ViewSessions:
                        AnsiConsole.Clear();
                        _codingSessionController.ViewSessions();
                        break;
                    case MainMenuAction.AddSession:
                        AnsiConsole.Clear();
                        _codingSessionController.AddSession();
                        break;
                    case MainMenuAction.DeleteSession:
                        AnsiConsole.Clear();
                        _codingSessionController.DeleteSession();
                        break;
                    case MainMenuAction.UpdateSession:
                        AnsiConsole.Clear();
                        _codingSessionController.UpdateSession();
                        break;
                }
            }
        }
    }
}
