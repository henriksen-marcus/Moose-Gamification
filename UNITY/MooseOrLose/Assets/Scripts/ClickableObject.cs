using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClickableObjectInfo
{
    public enum ObjectType
    {
        Moose,
        Wolf,
        Forest
    }

    public ObjectType type;

    public int age_years;
    public int age_months;
    public int age_days;
    public int weight;

    public Gender gender;
    public int antler_tags;
    public bool pregnant;
    public int days_pregnant;
    public int children_in_belly;
}

/* Base class for all visible objects in
 * the world that can ble clicked on and
 * retrieved some UI info from. */
public abstract class ClickableObject : MonoBehaviour
{
    /* When hovering hte mouse cursor */
    protected Outline outline;
    protected bool IsSelected;

    // This function needs to be overridden.
    public abstract ClickableObjectInfo GetClickInfo();

    public void ToggleOutline(bool enabled) => outline.enabled = enabled;

    public void SetOutlineSelected(bool selected)
    {
        outline.OutlineColor = selected ? Color.green : Color.white;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 6.5f;
        outline.enabled = selected;
        IsSelected = selected;
    }
}
