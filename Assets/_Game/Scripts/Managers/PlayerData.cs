using System;
using System.Collections.Generic;
using PanteonDemo;
using UnityEngine;
using UnityEngine.Serialization;
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
    public string Name;
    public Sprite Image;
    public uint Health;
    public uint Damage;
}

[Serializable]
public class SoldierType
{
    [SerializeField,  StringInList(typeof(PropertyDrawersHelper), "AllSoldierNames")]
    public string Name;

    public GameObject SoldierObject;
}

[Serializable]
public class Building
{
    public string Name;
    public uint Column;
    public uint Row;
    public uint Health;
    public Sprite Image;
    private List<dynamic> Production;
}

[Serializable]
public class BuildingType
{
    [SerializeField,  StringInList(typeof(PropertyDrawersHelper), "AllBuildingNames")]
    public string Name;

    public GameObject SoldierObject;
}
