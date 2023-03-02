using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClickableObjectInfo
{
    
}

/* Base class for all visible objects in
 * the world that can ble clicked on and
 * retrieved some UI info from. */
public abstract class ClickableObject : MonoBehaviour
{
    // This function needs to be overridden.
    public abstract ClickableObjectInfo GetClickInfo();
}
