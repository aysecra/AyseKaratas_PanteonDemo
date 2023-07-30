using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    public enum LevelState
    {
        Opened,
        Started,
        Paused,
        Failed,
        Completed
    }
    
    public class LevelManager : PersistentSingleton<LevelManager>
    {
        public void SetLevelPaused()
        {
            
        }

        public void SetLevelCompleted()
        {
            
        }
        public void SetLevelFailed()
        {
            
        }
    }
}
