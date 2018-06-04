using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hud : UI_Base
{
    [Header("Hud Reference Properties:")]
    public UI_ActionBar ActionBar;

    protected UI_Manager Manager;

	protected virtual void Awake ()
    {
        tag = "MainHud";
        Manager = UI_Manager.Instance;

	}

	protected virtual void Update ()
    {
		
	}
}
