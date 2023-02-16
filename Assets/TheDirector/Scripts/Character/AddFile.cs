using UnityEngine;

public class AddFile : MonoBehaviour
{
    public ScoreController scoreController;
    public GameObject file;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scoreController.Score();
            gameObject.SetActive(false);
            Debug.Log("HERE");
        }
    }
}
