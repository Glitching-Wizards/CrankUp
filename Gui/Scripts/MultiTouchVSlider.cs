using Godot;
using System.Collections.Generic;

namespace CrankUp;

[GlobalClass]
public partial class MultiTouchVSlider : VSlider
{
    // Store active touches with their corresponding slider instances
    private Dictionary<int, MultiTouchVSlider> activeTouches = new();

    public override void _Ready()
    {
        AddToGroup("multi_sliders"); // Add this slider to the multi_sliders group
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            if (touchEvent.Pressed)
            {
                HandleTouchPress(touchEvent.Index, touchEvent.Position);
            }
            else
            {
                HandleTouchRelease(touchEvent.Index);
            }
        }
        else if (@event is InputEventScreenDrag dragEvent)
        {
            HandleTouchDrag(dragEvent.Index, dragEvent.Position);
        }
    }

    // Handle the press of a touch on the slider
    private void HandleTouchPress(int touchIndex, Vector2 position)
    {
        // Iterate over all sliders in the group
        foreach (MultiTouchVSlider slider in GetTree().GetNodesInGroup("multi_sliders"))
        {
            if (slider.GetGlobalRect().HasPoint(position)) // Check if the touch is inside the slider's bounds
            {
                // Store the slider for this touch
                activeTouches[touchIndex] = slider;
                slider.UpdateSliderValue(position); // Update the slider value based on the touch position
                break;
            }
        }
    }

    // Handle the release of a touch
    private void HandleTouchRelease(int touchIndex)
    {
        if (activeTouches.ContainsKey(touchIndex))
        {
            activeTouches.Remove(touchIndex); // Remove the touch when it is released
        }
    }

    // Handle dragging (moving) a touch
    private void HandleTouchDrag(int touchIndex, Vector2 position)
    {
        if (activeTouches.ContainsKey(touchIndex)) // If the touch is active
        {
            activeTouches[touchIndex].UpdateSliderValue(position); // Update the slider value
        }
    }

    // Update the slider's value based on the touch position
    public void UpdateSliderValue(Vector2 eventPos)
    {
        // Calculate the value based on the Y position of the touch event relative to the slider
        float normalizedPos = (eventPos.Y - GlobalPosition.Y) / GetRect().Size.Y;  // Use GetRect().Size.y for height
        Value = Mathf.Clamp((1 - normalizedPos) * MaxValue, MinValue, MaxValue); // Corrected line, clamping the value
    }
}