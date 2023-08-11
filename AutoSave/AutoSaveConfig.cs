using UnityEngine;

namespace AutoSaving
{
    public class AutoSaveConfig : ScriptableObject
    {
        [Tooltip("AutoSave Intervals. Set in Minutes. Default is 10 Minute Intervals.")]
        public int _autoSaveInterval = 10;
    }
}