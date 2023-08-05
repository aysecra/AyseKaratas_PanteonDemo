using UnityEngine;

namespace PanteonDemo
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField] private InformationArea _informationArea;

        private void Start()
        {
            _informationArea.ClearInfoArea();
        }

        public void SetInformationArea(Soldier soldier)
        {
            _informationArea.OpenInfo(soldier);
        }
        
        public void SetInformationArea(Building building)
        {
            _informationArea.OpenInfo(building);
        }
    }
}
