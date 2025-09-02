using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int money;
    public int officePrice = 50;
    public GameObject office;
    public UIManager manager;
    public GameObject officeUI;
    public Stats stats;

    public List<EnemyOffice> botOffices = new List<EnemyOffice>();

    public bool CreateOffice() 
    {
        if (manager.AddBuild() && WasteMoney(officePrice))
        {
            GameObject newOffice = Instantiate(office, new Vector3(-2.5f, 0f, 0.5f), Quaternion.identity);
            newOffice.GetComponent<EnemyOffice>().SetEnemy(this);
            newOffice.GetComponent<EnemyOffice>().SetUiObject(officeUI);
            newOffice.GetComponent<EnemyOffice>().SetStats(stats);

            botOffices.Add(newOffice.GetComponent<EnemyOffice>());
            return true;
        }
        else
            return false;
    }

    public void ImproveOffice(EnemyOffice office)
    {
        if (botOffices.Count == 0)
            return;

        if (office == null)
        {
            office = botOffices[Random.Range(0, botOffices.Count)];
            if (office == null) return;
        }

        int[] prices = office.GetParamsPrice();
        if (prices == null || prices.Length < 3)
        {
            Debug.LogWarning("Office returned invalid prices.");
            return;
        }

        int start = Random.Range(0, 3);
        for (int i = 0; i < 3; i++)
        {
            int idx = (start + i) % 3;
            int price = prices[idx];

            if (price <= 0)
                price = 100;

            if (WasteMoney(price))
            {
                switch (idx)
                {
                    case 0:
                        office.ImproveIncome();
                        break;
                    case 1:
                        office.ImproveFrequency();
                        break;
                    case 2:
                        office.ImproveProtection();
                        break;
                }

                office.IncreasePrice(idx);
                return; 
            }
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool WasteMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }
    public void RemoveOffice(EnemyOffice office)
    {
        if (botOffices.Contains(office))
        {
            botOffices.Remove(office);
        }
    }

}
