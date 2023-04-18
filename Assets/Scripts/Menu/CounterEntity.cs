using TMPro;
using UnityEngine;

public class CounterEntity : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
    private static int m_countEntity;
    private static int m_countAgressiveEntity;    
    public static bool  isUpdate { get; set; }

    public static int GetCountEntity()
    {
        return m_countEntity;
    }

    public static int GetCountAgressiveEntity()
    {
        return m_countAgressiveEntity;
    }

    private void Awake()
    {
        isUpdate= true;
    }

    private void Update()
    {
        ShowCountEntity();
    }

    public static void AddEntity()
    {
        m_countEntity++;
        isUpdate = true;
    }

    public static void AddAgressiveEntity()
    {
        m_countAgressiveEntity++;
        isUpdate= true;
    }

    public static void DeleteEntity()
    {
        m_countEntity--;
        isUpdate = true;
    }

    public static void DeleteAgressiveEntity()
    {
        m_countAgressiveEntity--;
        isUpdate = true;
    }

    private void ShowCountEntity()
    {
        if (isUpdate == true)
        {
            isUpdate = false;
            m_Text.text = $"Total creatures: {m_countEntity}" + $"\nAgressive creatures: {m_countAgressiveEntity}";
        }
    }
}
