using PanteonDemo.Enum;
using PanteonDemo.Event;
using UnityEngine.SceneManagement;

namespace PanteonDemo.Manager
{
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