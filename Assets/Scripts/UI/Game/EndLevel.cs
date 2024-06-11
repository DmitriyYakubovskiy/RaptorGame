using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private Transform panel;
    private GameObject portal;

    private void Start()
    {
        portal = GameObject.Find("Portal");
        portal.GetComponent<Portal>().teleport += Teleport;
    }

    public void Teleport()
    {
        panel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (portal == null) return;
        portal.GetComponent<Portal>().teleport -= Teleport;
    }
}
