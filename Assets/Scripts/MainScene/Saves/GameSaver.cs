using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public static GameSaver Instance;
    public GameObject uiImprove;
    public GameObject uiHarm;

    public Stats stats;
    public Timer timer;
    public Enemy enemy;
    public List<Building> playerBuildings = new List<Building>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            stats = FindObjectOfType<Stats>();
            timer = FindObjectOfType<Timer>();
            enemy = FindObjectOfType<Enemy>();
            playerBuildings = new List<Building>(FindObjectsOfType<Building>());

            uiImprove = GameObject.Find("BuildingUpgrade");
            uiHarm = GameObject.Find("EnemyOffice");

            uiImprove.SetActive(false);
            uiHarm.SetActive(false);
        }
    }

    public void Save()
    {
        if (stats == null || timer == null || enemy == null)
        {
            Debug.LogWarning("Save aborted: нет ссылок на объекты сцены!");
            return;
        }

        GameSave save = new GameSave();

        save.stats = new StatsData
        {
            money = stats.GetCurrentMoney(),
            level = stats.GetCurrentLevel()
        };

        foreach (var b in playerBuildings)
        {
            if (b == null) continue;
            save.buildings.Add(new BuildingData
            {
                prefabName = b.gameObject.name.Replace("(Clone)", ""),
                position = b.transform.position,
                incomeLevel = b.GetIncomeLevel(),
                frequencyLevel = b.GetFrequencyLevel(),
                protectionLevel = b.GetProtectionLevel()
            });
        }

        save.timer = new TimerData
        {
            currentTime = timer.GetCurrentTime(),
            tax = timer.tax
        };

        EnemyData eData = new EnemyData
        {
            money = enemy.money
        };
        foreach (var office in enemy.botOffices)
        {
            if (office == null) continue;
            eData.offices.Add(new EnemyOfficeData
            {
                prefabName = office.gameObject.name.Replace("(Clone)", ""),
                position = office.transform.position,
                incomeLevel = office.GetIncomeLevel(),
                frequencyLevel = office.GetFrequencyLevel(),
                protectionLevel = office.GetProtectionLevel()
            });
        }
        save.enemy = eData;

        SaveLoadManager.SaveGame(save);
    }

    public void Load()
    {
        GameSave save = SaveLoadManager.LoadGame();
        if (save == null) return;

        if (stats == null || timer == null || enemy == null)
        {
            Debug.LogWarning("Load aborted: нет ссылок на объекты сцены!");
            return;
        }

        stats.LoadFromData(save.stats);

        foreach (var b in playerBuildings)
            if (b != null) Destroy(b.gameObject);
        playerBuildings.Clear();

        foreach (var bData in save.buildings)
        {
            GameObject prefab = Resources.Load<GameObject>($"Buildings/{bData.prefabName}");
            if (prefab == null) continue;

            GameObject obj = Instantiate(prefab, bData.position, Quaternion.identity);
            Building b = obj.GetComponent<Building>();
            b.SetLevels(bData.incomeLevel, bData.frequencyLevel, bData.protectionLevel);
            b.SetUIObject(uiImprove);
            playerBuildings.Add(b);
        }

        timer.LoadFromData(save.timer);

        enemy.money = save.enemy.money;
        foreach (var office in enemy.botOffices)
            if (office != null) Destroy(office.gameObject);
        enemy.botOffices.Clear();

        foreach (var oData in save.enemy.offices)
        {
            GameObject prefab = Resources.Load<GameObject>($"EnemyOffices/{oData.prefabName}");
            if (prefab == null) continue;

            GameObject obj = Instantiate(prefab, oData.position, Quaternion.identity);
            EnemyOffice office = obj.GetComponent<EnemyOffice>();
            office.SetLevels(oData.incomeLevel, oData.frequencyLevel, oData.protectionLevel);
            office.SetEnemy(enemy);
            office.SetUiObject(uiHarm);
            enemy.botOffices.Add(office);
        }
    }
}
