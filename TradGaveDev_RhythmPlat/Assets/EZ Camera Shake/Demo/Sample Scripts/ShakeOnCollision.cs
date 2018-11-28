using UnityEngine;
using EZCameraShake;

public class ShakeOnCollision: MonoBehaviour
{
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;
    public float travelAmount = .5f;

    private void Start()
    {
        LevelManager.enemyPlayerCollision.AddListener(Shake);
    }

    void Shake ()
    {
        // this will shake the camera only in the x direction with no rotation as this caused more issues than I was willing to put up with
        Vector3 posInfluence = new Vector3(travelAmount, travelAmount, 0);
        Vector3 rotInfluence = new Vector3(0, 0, 0);
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime, posInfluence, rotInfluence);
	}
}
