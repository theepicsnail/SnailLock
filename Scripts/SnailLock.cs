using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRCSDK2;
using System.Text.RegularExpressions;

[AddComponentMenu("Snail/Snail Lock")]
public class SnailLock : MonoBehaviour
{
    public string[] Passwords;
 
    [HideInInspector]
    public List<string> errors = new List<string>();
    [HideInInspector]
    public List<string> warnings = new List<string>();
    public void OnValidate()
    {
        errors.Clear();
        warnings.Clear();
        foreach (string password in Passwords)
        {
            if (!Regex.IsMatch(password, @"^[a-z0-9]+$"))
                errors.Add("Password '" + password + "' should only contain a-z or 0-9.");
            if (password.Length < 5)
                warnings.Add("Password '" + password + "' is short.");
        }
    }
}
