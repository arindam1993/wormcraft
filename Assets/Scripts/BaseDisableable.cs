using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDisableable : MonoBehaviour
{
    private bool __disabled = false;
    public virtual void WormcraftDisable()
    {
        __disabled = true;
    }

    public virtual void WormcraftEnable()
    {
        __disabled = false;
    }

    public bool IsWormcraftDisabled()
    {
        return __disabled;
    }

    public virtual Vector2 GetCenter()
    {
        return this.transform.position;
    }
}
