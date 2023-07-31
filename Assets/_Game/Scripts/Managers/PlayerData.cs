using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerData
{
    public int LevelIndex;
    public string LevelName;
}

[Serializable]
public class Soldier
{
    public string Title;
    public Sprite Image;
    public uint Health;
    public uint Damage;
}

[Serializable]
public class BuildingData
{
    public uint Health;
}