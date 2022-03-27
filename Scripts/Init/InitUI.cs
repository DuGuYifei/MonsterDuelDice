using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUI
{
    public void init()
    {
        StaticGameObject.UIDiceParentObject = GameObject.Find("UIDice");
    }
}
