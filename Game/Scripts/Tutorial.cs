using Godot;
using System;
using System.Collections.Generic;

namespace CrankUp;
public partial class Tutorial : Node
{
    private Node tutorial;
    private List<Window> tutorials = new List<Window>();
    private int currentTutorialIndex;
    private bool tutorialOn = false;
<<<<<<< HEAD
    [Export] private AudioStream clickSound;
=======
>>>>>>> 8a165564561152121a3ce3d167ff9dc1e441c839

    public override void _Ready()
    {
        // signaalin haku
        Pause pause = GetNode<Pause>("/root/Level1/Ui/ControlsRightUi/Pause");
        tutorial = pause.GetNode<Node>("Tutorial");

        if (tutorial == null)
        {
            GD.PrintErr("[ERROR] Tutorial node not found! Check the node path.");
            return;
        }

        foreach (Node child in tutorial.GetChildren())
        {
            if (child is Window tutorialWindow)
            {
                tutorials.Add(tutorialWindow);
                tutorialWindow.Visible = false; // Hide all tutorials initially
            }
        }

        // Skip button setup
        TextureButton skipButton = GetNodeOrNull<TextureButton>("SkipButton");
        if (skipButton != null)
        {
            skipButton.Pressed += SkipTutorial;
        }
        else
        {
            GD.PrintErr("[ERROR] SkipButton not found! Make sure it exists in the scene.");
        }

        if (pause != null) // Tarkistetaan, että pause-node löytyi
        {
            pause.Connect("TutorialStartedEventHandler", new Callable(this, nameof(StartTutorial)));
            GD.Print("Tutorial: TutorialStarted signaali yhdistetty."); // Lisätään tämä tuloste
        }
        else
        {
            GD.PrintErr("Tutorial: Pause nodea ei löytynyt, TutorialStarted signaalia ei yhdistetty!");
        }
    }

    public void StartTutorial()
    {
        GD.Print("Tutorial started");
        tutorialOn = true;
        currentTutorialIndex = 0;

        if (tutorials.Count > 0 && tutorials[currentTutorialIndex] != null)
        {
            tutorials[currentTutorialIndex].Visible = true;
        }
        else
        {
            GD.PrintErr("[ERROR] No tutorial windows found in the list.");
            tutorialOn = false; // Estetään lisäyritykset
        }
    }

    // input pitäs olla ok
    /// <summary>
    /// Handles input events.
    /// </summary>
    /// <param name="event"></param>
    public void _input(InputEvent @event)
    {
        if (tutorialOn && @event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.IsPressed())
        {
            NextTutorial();
        }
    }

    // pitäis olla ok
    /// <summary>
    /// Moves to the next tutorial step.
    /// </summary>
    private void NextTutorial()
    {
        GD.Print("Next tutorial");
        if (currentTutorialIndex < tutorials.Count && tutorials[currentTutorialIndex] != null)
        {
            // Current tutorial
            tutorials[currentTutorialIndex].Visible = true;

            //Hide the previous one
            if (currentTutorialIndex > 0 && tutorials[currentTutorialIndex - 1] != null)
            {
                tutorials[currentTutorialIndex - 1].Visible = false;
            }

            // Show the current tutorial
            tutorials[currentTutorialIndex].Visible = true;

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

    // pitäis olla ok
    public void SkipTutorial()
    {
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
        tutorialOn = false;
        GD.Print("Tutorial skipped");
    }
}