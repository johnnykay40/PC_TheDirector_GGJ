using UnityEngine;

public class InfectionController : MonoBehaviour
{
    [SerializeField] private float infection;
    [SerializeField] private float maxInfection;
    [SerializeField] private InfectedBar infectedBar;
    public GameController gameController;

    private void Start() 
    {
      infection = 0;
      infectedBar.StartInfectionBar(infection, maxInfection);
    }
    
    public void PlayerDamage(float damage)
    {
        infection += damage;
        infectedBar.ChangeInfection(infection);
        if (infection >= maxInfection)
        {
            Destroy(gameObject);
            //activar panel de game over
            gameController.GameOver();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponentInChildren<Enemy>();
        if(enemy != null)
        {
            Debug.Log(enemy.name);
            PlayerDamage(enemy.InfectionRate);
        }
    }
}
     


