using System;
using System.Collections.Generic;
using PanteonDemo;
using PanteonDemo.Logic;
using UnityEngine;

public class GridsCell : MonoBehaviour
{
    private uint _row;
    private uint _column;

    private CellInfo _gridsCellBase;
    private GridGenerator _gridGenerator;

    public uint Column
    {
        get => _column;
        set => _column = value;
    }

    public uint Row
    {
        get => _row;
        set => _row = value;
    }

    public CellInfo CellBase => _gridsCellBase;

    private void Start()
    {
        // _gridGenerator = GridGenerator.Instance;
    }

    public void GetHexagonNeighbours()
    {
        List<CellInfo> neighbours = new List<CellInfo>();

        // get down neighbours
        if (_row > 0)
        {
            // neighbours.Add(_gridGenerator.GetCell((int) (_row - 1), (int) _column).CellBase);
        }

        // // get up neighbours
        // if (_row < _gridGenerator.RowCount - 1)
        // {
        //     // GridsCell cell = _gridGenerator.GetCell((int) (_row + 1), (int) _column);
        //     // neighbours.Add(cell.CellBase);
        // }

        // get left neighbours
        if (_column > 0)
        {
            // neighbours.Add(_gridGenerator.GetCell((int) _row, (int) (_column - 1)).CellBase);
        }

        // get right neighbours
        // if (_column < _gridGenerator.ColumnCount - 1)
        // {
        //     // neighbours.Add(_gridGenerator.GetCell((int) _row, (int) (_column + 1)).CellBase);
        // }

        _gridsCellBase.Neighbors = neighbours;
        gameObject.name = $"{gameObject.name}({_row},{_column})";
    }

    public void SetCellBase()
    {
        // _gridsCellBase = new GridsCellBase() {CellObjectScript = this};
    }
}