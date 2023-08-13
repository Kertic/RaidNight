using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public class ReloadTesting 
    {
        [InitializeOnLoadMethod]
        static void onReload()
        {
            Debug.Log("Current time of reload: " + Time.time);
        }
    }
}
