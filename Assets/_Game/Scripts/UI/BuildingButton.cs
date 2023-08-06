using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanteonDemo
{
    public class BuildingButton : PoolableObject
    {
        [Header("Core Elements")] [SerializeField]
        private Image _imgElement;

        [SerializeField] private TMPro.TextMeshProUGUI _textTitle;
        [SerializeField] private TMPro.TextMeshProUGUI _textHealth;

        private Building _currentBuildingData;

        public void SetElementValue(Building buildingData)
        {
            _currentBuildingData = buildingData;
            _imgElement.sprite = buildingData.Image;
            _textTitle.text = buildingData.Name;
            _textHealth.text = buildingData.Health.ToString();
        }

        public void OnButtonClicked()
        {
            uint rowCount = _currentBuildingData.Row;
            uint columnCount = _currentBuildingData.Column;

            List<GridsCell> cellList =
                GridSystem.Instance.GetEmptyArea(rowCount, columnCount);

            if (cellList != null && cellList.Count > 0)
            {
                float cellSize = GridSystem.Instance.CellSize.x;
                Vector3 changingDist =
                    new Vector3(cellSize * (int) columnCount * .5f, cellSize * (int) rowCount * .5f, 0);
                Vector3 downLeftCorner = cellList[0].transform.position - new Vector3(cellSize, cellSize, 0) * .5f;
                Vector3 position = downLeftCorner + changingDist;
                BuildingController building =
                    SharedLevelManager.Instance.SpawnElement<BuildingController>(_currentBuildingData.Name, position);

                foreach (var cell in cellList)
                {
                    cell.CellBase.IsWalkable = false;
                }

                building.PlacedCellList = cellList;
            }
        }
    }
}