using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    /// <summary>
    /// Event is triggered when changing level state
    /// </summary>
    public struct LevelEvent
    {
        public LevelState State { get; }

        public LevelEvent(LevelState state)
        {
            State = state;
        }
    }
    
    public class GameManager : PersistentSingleton<GameManager>
    {
        
        public void SetLevelPaused()
        {
            // todo: level pause element will added
        }

        public void SetLevelCompleted()
        {
            LoadNextLevel();
        }
        public void SetLevelFailed()
        {
            ReloadLevel();
        }
        
        private void LoadNextLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Completed));
            string nextLevel = ProgressManager.Instance.GetNextLevelName();
            SceneManager.LoadScene(nextLevel);
        }

        private void ReloadLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Failed));
            string currLevel = ProgressManager.Instance.GetCurrentLevelName();
            SceneManager.LoadScene(currLevel);
        }
    }
}