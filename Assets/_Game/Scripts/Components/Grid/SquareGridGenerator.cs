using System.Collections.Generic;
using StrategyDemo.Interfaces;
using StrategyDemo.Logic;
using UnityEngine;

namespace StrategyDemo.Component
{
    public class SquareGridGenerator : GridGenerator
    {
        protected override void SetCell()
        {
            _cellArray = new CellInfo[_cellSize.x, _cellSize.y];
            Vector3 currPos = cellDownLeftPoint.position;

            gridSO.SetGridDownLeftPosition(GetWorldToCellsWorldPosition(currPos));
            gridCollider.SetCollider();

            for (int y = 0; y < _cellSize.y; y++)
            {
                for (int x = 0; x < _cellSize.x; x++)
                {
                    Vector3 cellPosition = GetWorldToCellsWorldPosition(currPos);
                    currPos = cellPosition;
                    cellPosition += (Vector3) (_gridCellSize * .5f);

                    Instantiate(_cellPrefab, cellPosition, Quaternion.identity, cellParent);

                    _cellArray[x, y] = new SquareCell()
                    {
                        CenterPosition = cellPosition,
                        Index = new Vector2Int(x, y)
                    };


                    currPos.x += _gridCellSize.x;
                }

                currPos.x = cellDownLeftPoint.position.x;
                currPos.y += _gridCellSize.y;
            }

            SetNeigbors();
            gridSO.SetCellArray(_cellArray);
        }

        protected override void SetNeigbors()
        {
            foreach (CellInfo cell in _cellArray)
            {
                CellNeighborCalculator.GetNeighbor((ICellShape) cell, this, _cellSize);
            }
        }

        public override bool CalculateObjectFromUpRightCenter(Vector3 position, Vector2Int size, out Vector3 centerPos,
            out List<CellInfo> cellList)
        {
            cellList = new List<CellInfo>();

            Vector3 upRightPos = GetWorldToCellsWorldPosition(position);
            CellInfo upRightCell = GetCellInfoToWorldPosition(position);

            Vector3 downLeftPos = new Vector3(upRightPos.x - (size.x - .5f) * _gridCellSize.x,
                upRightPos.y - (size.y - .5f) * _gridCellSize.y, transform.position.z);

            centerPos = new Vector3(downLeftPos.x + size.x * _gridCellSize.x * .5f,
                downLeftPos.y + size.y * _gridCellSize.y * .5f,
                transform.position.z);

            for (int x = upRightCell.Index.x; x < size.x; x--)
            {
                for (int y = upRightCell.Index.y; y < size.y; y--)
                {
                    cellList.Add(_cellArray[x, y]);
                }
            }

            return gridCollider.IsItWithinBorders(upRightPos, out Vector2Int overflowDirection);
        }

        public override Vector3 GetWorldToCellsCenterPosition(Vector3 position)
        {
            return GetWorldToCellsWorldPosition(position) + (Vector3) (_gridCellSize * .5f);
        }

        public override void CalculateObjectCenter(Vector3 position, Vector2Int size, out Vector3 centerPos,
            out List<CellInfo> cellList)
        {
            cellList = new List<CellInfo>();

            Vector3 downLeftPos = GetWorldToCellsWorldPosition(position);
            CellInfo downLeftCell = GetCellInfoToWorldPosition(position);

            Vector3 upRightPos = new Vector3(downLeftPos.x + (size.x - .5f) * _gridCellSize.x,
                downLeftPos.y + (size.y - .5f) * _gridCellSize.y, transform.position.z);

            centerPos.z = transform.position.z;

            int minX = downLeftCell.Index.x;
            int minY = downLeftCell.Index.y;
            int maxX = _cellArray.GetLength(0);
            int maxY = _cellArray.GetLength(1);
            Vector3 rightUp = GetWorldToCellsWorldPosition(_cellArray[maxX - 1, maxY - 1].CenterPosition);

            if (minX + size.x <= _cellArray.GetLength(0))
            {
                maxX = minX + size.x;
                centerPos.x = downLeftPos.x + size.x * _gridCellSize.x * .5f;
            }
            else
            {
                centerPos.x = rightUp.x - (size.x * .5f - 1) * _gridCellSize.x;
            }

            if (minY + size.y <= _cellArray.GetLength(1))
            {
                maxY = minY + size.y;
                centerPos.y = downLeftPos.y + size.y * _gridCellSize.y * .5f;
            }
            else
            {
                centerPos.y = rightUp.y - (size.y * .5f - 1) * _gridCellSize.y;
            }

            for (int x = maxX - size.x; x < maxX; x++)
            {
                for (int y = maxY - size.y; y < maxY; y++)
                {
                    cellList.Add(_cellArray[x, y]);
                }
            }
        }

        protected override Vector3 CalculateObjectCenter(Vector3 position, Vector2Int size)
        {
            Vector3 downLeftPos = GetWorldToCellsWorldPosition(position);

            return new Vector3(downLeftPos.x + size.x * _gridCellSize.x * .5f,
                downLeftPos.y + size.y * _gridCellSize.y * .5f,
                transform.position.z);
        }

        public override Vector3 GetRightUpOrigin(Vector3 origin, Vector2Int size)
        {
            Vector3 downLeftPos = GetWorldToCellsWorldPosition(origin);


            return GetWorldToCellsWorldPosition(new Vector3(downLeftPos.x + size.x * _gridCellSize.x,
                downLeftPos.y + size.y * _gridCellSize.y, transform.position.z));
        }

        public override List<CellInfo> GetSpawnArea(Vector2Int size, out Vector3 position)
        {
            List<CellInfo> spawnArea = new List<CellInfo>();
            bool isFinish = false;

            for (int x = 0; x < _cellArray.GetLength(0); x++)
            {
                for (int y = 0; y < _cellArray.GetLength(1); y++)
                {
                    CellInfo cell = _cellArray[x, y];
                    isFinish = IsAreaPlaceable(cell, size, out spawnArea);

                    if (isFinish)
                    {
                        break;
                    }

                    spawnArea.Clear();
                }

                if (isFinish)
                {
                    break;
                }
            }

            position = default;

            if (spawnArea.Count > 0)
            {
                position = CalculateObjectCenter(spawnArea[0].CenterPosition, size);
            }

            return spawnArea;
        }

        bool IsAreaPlaceable(CellInfo cell, Vector2Int size, out List<CellInfo> cellArea)
        {
            cellArea = new List<CellInfo>();
            
            Vector2Int index = cell.Index;
            Vector2Int maxIndex = cell.Index + size;
            
            if (maxIndex.x > _cellArray.GetLength(0) || maxIndex.y > _cellArray.GetLength(1))
                return false;

            for (int x = index.x; x < maxIndex.x; x++)
            {
                for (int y = index.y; y < maxIndex.y; y++)
                {
                    if (!_cellArray[x, y].IsWalkable)
                        return false;
                    cellArea.Add(_cellArray[x, y]);
                }
            }

            return true;
        }
    }
}