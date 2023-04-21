using Assets.Scripts.AllEntity;
using UnityEngine;

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

    private void Awake()
    {
        m_explosion = Resources.Load("Prefabs/Entity/Smoke");
        m_time = m_startTime;
    }



    private void Update()
    {
        if (RechargeTimeSpawn())
        {
            if (CounterEntity.GetCountEntity() < m_maxEntity)
            {
                SpawnEntity();
                m_time = (float)CounterEntity.GetCountEntity()/ m_maxEntity*m_startTime;
            }
        }
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

    public void SpawnEntity()
    {
        System.Random rand = new System.Random();

        int index = rand.Next(0, m_AIEntity.GetLength(0));
        int positionX = rand.Next(m_beginSpawnXRange, m_endSpawnXRange);

        GameObject explosionRef = (GameObject)Instantiate(m_explosion);
        explosionRef.transform.position = new Vector3(positionX, m_SpawnY, 0);
        Destroy(explosionRef, 1f);

        GameObject entity = (GameObject)Instantiate(m_AIEntity[index]);
        
        entity.transform.position = new Vector3(positionX, m_SpawnY, 0);
    }
}
