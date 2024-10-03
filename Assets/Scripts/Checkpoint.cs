using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;
    public GameObject LightFire;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoin id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }

    }

    public void ActivateCheckpoint()
    {
        if (!activationStatus)
            AudioManager.instance.PlaySFX(4, transform);

        activationStatus = true;
        LightFire.SetActive(true);
        anim.SetBool("active", true);
    }
}
