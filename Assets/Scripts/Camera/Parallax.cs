using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform m_followingTarget;
    [SerializeField, Range(0f, 1f)] float m_parallaxStrength = 0.1f;
    [SerializeField] bool m_disableVerticalParallax;
    Vector3 targetPreviosPosition;

    private void Start()
    {
        if (!m_followingTarget)
        {
            m_followingTarget = Camera.main.transform;
        }

        targetPreviosPosition = m_followingTarget.position;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 delta = m_followingTarget.position - targetPreviosPosition;

        if (m_disableVerticalParallax)
        {
            delta.y = 0;
        }

        targetPreviosPosition = m_followingTarget.position;

        transform.position += delta * m_parallaxStrength;
    }
}
