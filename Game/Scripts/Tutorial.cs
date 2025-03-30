using Godot;
using System;


namespace CrankUp;
public partial class Tutorial : Node
{
    [Export] private string _tutorialScenePath = "res://Gui/Tutorial";
    private Window[] tutorials;
    private int currentTutorialIndex = 0;
    private Window tutorial1, tutorial2, tutorial3, tutorial4, tutorial5,
                    tutorial6, tutorial7, tutorial8, tutorial9;
    public override void _Ready()
    {
        tutorial = GetNode<Node>("res://Gui/Tutorial/");
        if (tutorial == null)
        {
            GD.PrintErr("[ERROR] Tutorial node not found! Check the node path.");
            return;
        }

        // Skip
        Button skipButton = GetNodeOrNull<Button>("SkipButton");
        if (skipButton != null)
    {
        skipButton.Pressed += SkipTutorial;
    }
    else
    {
        GD.PrintErr("[ERROR] SkipButton not found! Make sure it exists in the scene.");
    }

        StartTutorial();
    }

    public void StartTutorial()
    {
        // Load the tutorial scenes
        if (tutorial != null)
        {
            tutorial1 = tutorial.GetNodeOrNull<Window>("Tutorial1");
            tutorial2 = tutorial.GetNodeOrNull<Window>("Tutorial2");
            tutorial3 = tutorial.GetNodeOrNull<Window>("Tutorial3");
            tutorial4 = tutorial.GetNodeOrNull<Window>("Tutorial4");
            tutorial5 = tutorial.GetNodeOrNull<Window>("Tutorial5");
            tutorial6 = tutorial.GetNodeOrNull<Window>("Tutorial6");
            tutorial7 = tutorial.GetNodeOrNull<Window>("Tutorial7");
            tutorial8 = tutorial.GetNodeOrNull<Window>("Tutorial8");
            tutorial9 = tutorial.GetNodeOrNull<Window>("Tutorial9");

            if (tutorial1 != null) tutorial1.Visible = false;
            if (tutorial2 != null) tutorial2.Visible = false;
            if (tutorial3 != null) tutorial3.Visible = false;
            if (tutorial4 != null) tutorial4.Visible = false;
            if (tutorial5 != null) tutorial5.Visible = false;
            if (tutorial6 != null) tutorial6.Visible = false;
            if (tutorial7 != null) tutorial7.Visible = false;
            if (tutorial8 != null) tutorial8.Visible = false;
            if (tutorial9 != null) tutorial9.Visible = false;
        }

        tutorials = new[] { tutorial1, tutorial2, tutorial3, tutorial4, tutorial5,
                            tutorial6, tutorial7, tutorial8, tutorial9 };

        GD.Print("Tutorial started");
    }

    /// <summary>
    /// Handles input events.
    /// </summary>
    /// <param name="event"></param>
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed())
        {
            NextTutorial();
        }
    }

    /// <summary>
    /// Moves to the next tutorial step.
    /// </summary>
    private void NextTutorial()
    {
        if (currentTutorialIndex < tutorials.Length && tutorials[currentTutorialIndex] != null)
        {
            // Current tutorial
            tutorials[currentTutorialIndex].Visible = true;

            //Hide the previous one
            if (currentTutorialIndex > 0 && tutorials[currentTutorialIndex - 1] != null)
            {
                tutorials[currentTutorialIndex - 1].Visible = false;
            }

            // Move to the next tutorial step
            currentTutorialIndex++;
        }
        else
        {
            GD.Print("Tutorial finished");
        }
    }

    public void SkipTutorial()
    {
        GD.Print("Tutorial skipped");
        // Hide all tutorials
        foreach (var tutorial in tutorials)
        {
            if (tutorial != null)
            {
                tutorial.Visible = false;
            }
        }
    }
}
