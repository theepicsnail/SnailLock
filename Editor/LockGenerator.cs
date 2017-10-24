using System.Collections.Generic;
using UnityEngine;

public class LockGenerator
{
    private class State
    {
        public GameObject Object = new GameObject();
        public Dictionary<KeyCode, State> Transitions = new Dictionary<KeyCode, State>();
        public bool Accept = false;
    }
    private const bool humanReadable = false;
    private State generateState(string hint)
    {
        State s = new State();
        s.Object.transform.parent = root.transform;
        s.Object.SetActive(false);
        s.Object.name = humanReadable ? hint : "SnailLockObject" + Random.Range(1000000, 10000000-1);
        generatedStates.Add(s);
        return s;
    }

    private HashSet<State> generatedStates = new HashSet<State>();
    private Transform root;
    private State start;

    public LockGenerator(Transform transform)
    {
        root = transform;
        start = generateState("Start");
        start.Object.SetActive(true);
    }

    public void InsertPassword(string password)
    {
        State cur = start;
        foreach (char c in password)
        {
            KeyCode code = Util.KeyMap[c];
            State next;
            if (!cur.Transitions.TryGetValue(code, out next))
            {
                next = generateState("" + c);
                cur.Transitions.Add(code, next);
            }
            cur = next;
        }
        cur.Accept = true;
    }

    public void SetupTriggers(GameObject onSuccess, GameObject onFailure)
    {
        State fail1 = generateState("fail1");
        State fail2 = generateState("fail2");
        foreach (State cur in generatedStates)
        {
            foreach (KeyCode code in Util.KeyMap.Values)
            {
                State next;
                if (!cur.Transitions.TryGetValue(code, out next))
                {
                    // On a bad key:
                    // Fail1 -> Fail2
                    // Fail2 -> Fail1
                    // Other -> Fail1
                    // This bounces the user between 2 states until they push enter.
                    next = cur == fail1 ? fail2 : fail1;
                }
                Util.AddTransferEvent(cur.Object, code, next.Object);
            }
            // TODO failure states should go here instead of just reenabling start.Object.
            Util.AddTransferEvent(cur.Object, KeyCode.Return, cur.Accept ? onSuccess : start.Object); 
            Util.AddTransferEvent(cur.Object, KeyCode.KeypadEnter, cur.Accept ? onSuccess : start.Object);
            Util.AddTransferEvent(cur.Object, KeyCode.Escape, start.Object);
        }
    }

}