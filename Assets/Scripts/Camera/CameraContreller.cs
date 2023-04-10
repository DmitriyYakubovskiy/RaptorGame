using UnityEngine;

public class CameraContreller : MonoBehaviour
{
    [SerializeField] private Transform m_tansformPlayer;
    [SerializeField] private float m_speed = 0;
    private Vector3 pos;

    private void Awake()
    {
        m_speed = 13f;
    }
    private void FixedUpdate()
    {
        if (m_tansformPlayer)
        {
            pos = m_tansformPlayer.position;
            pos.z = -10f;
            pos.y = m_tansformPlayer.position.y + 2;

            transform.position = Vector3.Lerp(transform.position, pos, m_speed*Time.deltaTime);
        }
    }
}

