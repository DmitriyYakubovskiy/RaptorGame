using Assets.Scripts.AllEntity;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object[] m_AIEntity;
    private UnityEngine.Object m_explosion;
    [SerializeField] private float m_startTime;
    private float m_time = 0;

    [SerializeField] private float m_SpawnY;
    [SerializeField] private int m_beginSpawnXRange;
    [SerializeField] private int m_endSpawnXRange;

    [SerializeField] private int m_maxEntity;

    private float allTime = 0;

    private void Awake()
    {
        m_explosion = Resources.Load("Prefabs/Environment/Smoke");
        m_time = m_startTime;
    }


    private void Update()
    {
        if (Mathf.FloorToInt(allTime / 60) == 1)
        {
            allTime = 0;
            m_startTime *= 0.70f;
        }
        if (RechargeTimeSpawn())
        {
            if (CounterEntity.GetCountEntity() < m_maxEntity)
            {
                SpawnEntity();
                m_time = m_startTime;
            }
        }
        allTime += Time.deltaTime;
    }

    private bool RechargeTimeSpawn()
    {
        m_time -= Time.deltaTime;
        if (m_time > 0)
        {
            return false;
        }
        return true;
    }

    private void SpawnEntity()
    {
        System.Random rand = new System.Random();
        int chance = rand.Next(1, 101);
        int positionX = rand.Next(m_beginSpawnXRange, m_endSpawnXRange);
        int index = 0;

        SpawnSmoke(positionX);

        if (chance > 95) index = 3;
        else if (chance > 85) index = 1;
        else if (chance > 60) index = 0;
        else index = 2;
        GameObject entity = (GameObject)Instantiate(m_AIEntity[index]);
        entity.transform.position = new Vector3(positionX, m_SpawnY, 0);
    }

    private void SpawnSmoke(int positionX)
    {
        GameObject explosionRef = (GameObject)Instantiate(m_explosion);
        explosionRef.transform.position = new Vector3(positionX, m_SpawnY, 0);
        Destroy(explosionRef, 1f);
    }
}
