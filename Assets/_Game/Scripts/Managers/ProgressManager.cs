using System;
using System.Collections.Generic;
using UnityEngine;

namespace PanteonDemo
{
    [System.Serializable]
    public class LevelProgress
    {
        [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")] public string LevelName;
    }

    public class ProgressManager : PersistentSingleton<ProgressManager>
                                    ,EventListener<LevelEvent>
    {
        [SerializeField] private List<LevelProgress> _levels;
        
        private PlayerData _playerData;
        private const string _playerDataPrefKey = "PlayerData";

        protected override void Awake()
        {
            base.Awake();

            if (!PlayerPrefs.HasKey(_playerDataPrefKey))
            {
                _playerData = new PlayerData
                {
                    LevelName = _levels[0].LevelName,
                    LevelIndex = 0
                };
            }

            else
            {
                _playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString(_playerDataPrefKey));
            }
        }
        
        private void SetPlayerData()
        {
            PlayerPrefs.SetString(_playerDataPrefKey, JsonUtility.ToJson(_playerData));
        }
        
        private void GetPlayerData()
        {
            PlayerPrefs.SetString(_playerDataPrefKey, JsonUtility.ToJson(_playerData));
        }
        
        private void SetNextLevel()
        {
            int index = _playerData.LevelIndex + 1 < _levels.Count ? _playerData.LevelIndex + 1 : 0;
            _playerData.LevelName = _levels[index].LevelName;
            _playerData.LevelIndex = index;
        }

        public string GetCurrentLevelName()
        {
            return _playerData.LevelName;
        }
        
        public string GetNextLevelName()
        {
            int index = _playerData.LevelIndex + 1 < _levels.Count ? _playerData.LevelIndex + 1 : 0;
            return _levels[index].LevelName;
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<LevelEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<LevelEvent>(this);
        }

        public void OnEventTrigger(LevelEvent currentEvent)
        {
            if (currentEvent.State == LevelState.Completed)
            {
                SetNextLevel();
            }
        }
    }
}