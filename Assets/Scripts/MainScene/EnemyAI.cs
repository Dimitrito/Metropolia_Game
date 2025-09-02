using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Enemy enemy;
    public Timer timer;
    public float decisionDelay = 3f; 

    private void Start()
    {
        StartCoroutine(ThinkRoutine());
    }

    private IEnumerator ThinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(decisionDelay);

            int tax = timer.tax;

            if (enemy.money < tax * 2)
                continue;

            if (enemy.botOffices.Count == 0)
            {
                enemy.CreateOffice();
                continue;
            }

            float roll = Random.value;

            if (roll < 0.3f)
            {
                enemy.CreateOffice();
            }
            else
            {
                EnemyOffice target = enemy.botOffices[Random.Range(0, enemy.botOffices.Count)];
                enemy.ImproveOffice(target);
            }
        }
    }
}
