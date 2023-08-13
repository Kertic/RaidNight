using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class ReloadTesting1 
    {
        [InitializeOnLoadMethod]
        static void OnReload()
        {
            Debug.Log("Current time of reload 1: " + Time.time);
        }
    }
}
