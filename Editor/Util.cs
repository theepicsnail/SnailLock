using System;
using System.Collections.Generic;
using UnityEngine;
using VRCSDK2;

internal class Util
{
    public static Dictionary<char, KeyCode> KeyMap;
    static Util()
    {
        KeyMap = new Dictionary<char, KeyCode>();
        for (char code = 'a'; code <= 'c'; code++)
            KeyMap[code] = (KeyCode)code;
        for (char code = '0'; code <= '3'; code++)
            KeyMap[code] = (KeyCode)code;
    }

    public static void AddTransferEvent(GameObject srcState, KeyCode code, GameObject endState)
    {
        if(srcState == endState)
        {
            return; 
        }

        // Using 'Always' because it was annoying when trying to break with multiple people.
        VRC_Trigger.TriggerEvent triggerEvent = new VRC_Trigger.TriggerEvent();
        triggerEvent.TriggerType = VRC_Trigger.TriggerType.OnKeyDown;
        triggerEvent.BroadcastType = VRC_EventHandler.VrcBroadcastType.Local;
        triggerEvent.Key = code;

        // Turn on the next state
        VRC_EventHandler.VrcEvent action = new VRC_EventHandler.VrcEvent();
        action.EventType = VRC_EventHandler.VrcEventType.SetGameObjectActive;
        action.ParameterBoolOp = VRC_EventHandler.VrcBooleanOp.True;
        action.ParameterObjects = new GameObject[] { endState };
        triggerEvent.Events.Add(action);

        // Turn off this state
        action = new VRC_EventHandler.VrcEvent();
        action.EventType = VRC_EventHandler.VrcEventType.SetGameObjectActive;
        action.ParameterBoolOp = VRC_EventHandler.VrcBooleanOp.False;
        action.ParameterObjects = new GameObject[] { srcState };
        triggerEvent.Events.Add(action);

        // Add it to the source.
        VRC_Trigger trigger = srcState.AddComponent<VRC_Trigger>();
        trigger.Triggers.Add(triggerEvent);
    }
}