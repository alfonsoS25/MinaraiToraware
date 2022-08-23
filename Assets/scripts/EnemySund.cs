using UnityEngine;

public class EnemySund : MonoBehaviour
{
    public AudioSource[] EnemySounds;

    public void PlayerEnemySound(int sound)
    {
        EnemySounds[sound].Play();
    }
}