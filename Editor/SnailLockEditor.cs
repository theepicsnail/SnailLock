using UnityEngine;
using UnityEditor;
using System;


[CustomEditor(typeof(SnailLock))]
public class SnailLockEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SnailLock slock = (SnailLock)target;

        var style = new GUIStyle(GUI.skin.label);
        // Show errors
        style.normal.textColor = Color.red;
        if (slock.errors.Count > 0)
        {
            foreach (string error in slock.errors)
            {
                GUILayout.Label(error, style);
            }
            return;
        }
        // Show Warnings
        style.normal.textColor = new Color(1, 1f, 0);
        foreach (string error in slock.warnings)
        {
            GUILayout.Label(error, style);
        }

        // Button
        if (GUILayout.Button("Create Lock"))
        {
            buildLock(slock);
        }
    }

    public Transform getOrCreate(string name, bool forceRecreate = false)
    {
        Transform curObj = ((SnailLock)target).transform;
        Transform obj = curObj.Find(name);
        if (obj && forceRecreate)
        {
            DestroyImmediate(obj.gameObject);
            obj = null;
        }

        if (!obj)
        {
            obj = new GameObject(name).transform;
            obj.parent = curObj;
        }

        return obj;
    }

    private void buildLock(SnailLock slock)
    {
        Transform success = getOrCreate("OnUnlocked");
        success.gameObject.SetActive(false);

        // TODO Re-enable failure states (being locked out).
        //Transform failure = getOrCreate("OnLockedOut");

        /**
         * Right now there's a bug where SetGameObjectActive doesn't work for uncle relationships.
         * Lock
         *  OnUnlocked
         *  _States
         *    Start
         *    a
         *    b -> Can not enable 'OnUnlocked'
         *  
         *  As such a lot of this logic has been screwed up.
         *  Since I don't want to delete the users 'OnUnlocked' object every build, this cleans
         *  out any of the other objects under the lock (leaving only OnUnlocked).
         *  This is trash and should be fixed...
         */
         /*
        Transform root = slock.transform;
        for (int i = root.childCount - 1; i >= 0; i--)
        {
            Transform child = root.GetChild(i);
            if (child != success)
                DestroyImmediate(child.gameObject);
        }

        LockGenerator gen = new LockGenerator(root);
        */

        Transform root = getOrCreate("_States", true);
        LockGenerator gen = new LockGenerator(root);

        foreach (String password in slock.Passwords)
        {
            gen.InsertPassword(password);
        }

        gen.SetupTriggers(success.gameObject, null); // failure.gameObject);
    }
}
