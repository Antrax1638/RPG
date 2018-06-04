using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDebugMode
{
    public enum DebugType
    {
        Info = 0,
        Warning,
        Error,
        Assertion
    };

    [Header("Debug Mode:")]
    public bool DebugMode;
    public DebugType Type = DebugType.Info;

    protected virtual void DebugLog(object Object, int Mode = -1)
    {
        if(DebugMode)
        {
            Type = (Mode >= 0) ? (DebugType)Mode : Type;
            switch (Type)
            {
                case DebugType.Info: Debug.Log(Object); break;
                case DebugType.Warning: Debug.LogWarning(Object); break;
                case DebugType.Error: Debug.LogError(Object); break;
                case DebugType.Assertion: Debug.LogAssertion(Object); break;
                default: Debug.Log(Object); break; 
            }
        }
    }

    protected virtual void DebugLog(object Object, DebugType Mode = DebugType.Info)
    {
        if (DebugMode)
        {
            Type = Mode;
            switch (Type)
            {
                case DebugType.Info: Debug.Log(Object); break;
                case DebugType.Warning: Debug.LogWarning(Object); break;
                case DebugType.Error: Debug.LogError(Object); break;
                case DebugType.Assertion: Debug.LogAssertion(Object); break;
                default: Debug.Log(Object); break;
            }
        }
    }

    protected virtual void DebugLogFormat(string Msg, params object[] args)
    {
        if(DebugMode)
            Debug.LogFormat(Msg, args);
    }
}
    
