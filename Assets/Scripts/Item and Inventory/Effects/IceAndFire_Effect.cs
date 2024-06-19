using UnityEngine;

[CreateAssetMenu(fileName = "Ice and fire effect", menuName = "Data/Item effect/Ice and fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefabs;
    [SerializeField] private float xVelociry;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thirAttack = player.primaryAttack.comboCounter == 2;

        if (thirAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefabs, _respawnPosition.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelociry * player.facingDir, 0);
            Destroy(newIceAndFire,10f);
        }
    } 
}
