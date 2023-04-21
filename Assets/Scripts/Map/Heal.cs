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
        if (collision.gameObject.name == "Raptor")
        {
            var raptor = collision.GetComponent<Raptor>();
            if (raptor.GetHealthBar().GetMaxHealth()!=raptor.GetLives())
            {
                if (raptor.GetLives() + m_heatPoint > raptor.GetHealthBar().GetMaxHealth())
                {
                    raptor.SetLives(raptor.GetHealthBar().GetMaxHealth());
                }
                else
                {
                    raptor.SetLives(raptor.GetLives()+m_heatPoint);
                }
                raptor.GetHealthBar().ShowHealth(raptor);
                Destroy(this.gameObject);
            }
        }
    }
}
