using UnityEngine;
using EZCameraShake;

public class ShakeOnCollision: MonoBehaviour
{
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;
    public float slideAmount = .5f;

    private void Start()
    {
        LevelManager.enemyPlayerCollision.AddListener(Shake);
    }

    void Shake ()
    {
        Vector3 posInfluence = new Vector3(slideAmount, 0, 0);
        Vector3 rotInfluence = new Vector3(0, 0, 0);
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime, posInfluence, rotInfluence);
	}
}
