using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{
    public StatsData stats;
    public List<BuildingData> buildings = new List<BuildingData>();
    public TimerData timer;
    public EnemyData enemy;
}

[Serializable]
public class StatsData
{
    public int money;
    public int level;
}

[Serializable]
public class BuildingData
{
    public string prefabName;
    public Vector3 position;
    public int incomeLevel;
    public int frequencyLevel;
    public int protectionLevel;
}

[Serializable]
public class TimerData
{
    public int currentTime;
    public int tax;
}

[Serializable]
public class EnemyData
{
    public int money;
    public List<EnemyOfficeData> offices = new List<EnemyOfficeData>();
}

[Serializable]
public class EnemyOfficeData
{
    public string prefabName;
    public Vector3 position;
    public int incomeLevel;
    public int frequencyLevel;
    public int protectionLevel;
}
