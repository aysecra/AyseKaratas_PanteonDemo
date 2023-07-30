using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int LevelIndex;
    public string LevelName;
}

[Serializable]
public class Soldier
{
    public uint Health;
    public uint Damage;
}

[Serializable]
public class BuildingData
{
    public uint Health;
}