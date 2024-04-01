using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField]private float m_heatPoint;

    public void SetHeatPoint(float heatPoint)
    {
        m_heatPoint = heatPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Raptor" && collision.isTrigger==false)
        {
            var raptor = collision.GetComponent<Raptor>();
            if (raptor.GetHealthBar().GetMaxHealth()!=raptor.Lives)
            {
                raptor.AddHeatPoint(m_heatPoint);
                Destroy(this.gameObject);
            }
        }
    }
}
