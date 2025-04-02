using Godot;
using System;
using System.Collections.Generic;


namespace CrankUp;
public partial class Tutorial : Node
{
    private Node tutorial;
    private List<Window> tutorials = new List<Window>();
    private int currentTutorialIndex = 0;
    private bool tutorialOn = true;

    public override void _Ready()
    {
        tutorial = GetNode<Node>("Tutorial");

        if (tutorial == null)
        {
            GD.PrintErr("[ERROR] Tutorial node not found! Check the node path.");
            return;
        }

        // Skip
        TextureButton skipButton = GetNodeOrNull<TextureButton>("SkipButton");
        if (skipButton != null)
        {
            skipButton.Pressed += SkipTutorial;
        }
        else
        {
            GD.PrintErr("[ERROR] SkipButton not found! Make sure it exists in the scene.");
        }

        if (tutorialOn)
        {
            StartTutorial();
        }
        else
        {
            GD.PrintErr("[ERROR] Tutorial node not found! Check the node path.");
        }
    }

    public void StartTutorial()
    {
        if (tutorial == null)
        {
            GD.PrintErr("[ERROR] Tutorial node is null. Cannot load tutorial scenes.");
            return;
        }

        // Load the tutorial scenes
        for (int i = 1; i <= 9; i++)
        {

            var tutorialWindow = tutorial.GetNodeOrNull<Window>($"Tutorial{i}");
            if (tutorialWindow != null)
            {
                tutorialWindow.Visible = true;
                tutorials.Add(tutorialWindow);
            }
            else
            {
                GD.PrintErr($"[ERROR] Tutorial{i} not found!");
            }
        }
        GD.Print("Tutorial started");
    }

    /// <summary>
    /// Handles input events.
    /// </summary>
    /// <param name="event"></param>
    public void _input(InputEvent @event)
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
        if (currentTutorialIndex < tutorials.Count && tutorials[currentTutorialIndex] != null)
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
            tutorialOn = false;
            foreach (var tutorial in tutorials)
            {
                if (tutorial != null)
                {
                    tutorial.Visible = false;
                }
            }
            GD.Print("Tutorial finished");
        }
    }

    public void SkipTutorial()
    {
        tutorialOn = false;

        // Hide all tutorials
        if (tutorials.Count > 0)
        {
            foreach (var tutorial in tutorials)
            {
                if (tutorial != null)
                {
                    tutorial.Visible = false;
                }
            }
        }
        GD.Print("Tutorial skipped");
    }
}
