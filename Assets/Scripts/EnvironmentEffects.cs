using UnityEngine;

public class EnvironmentEffects : MonoBehaviour
{
    public static EnvironmentEffects Instance;

    [Header("Reward Objects")]
    public GameObject Plexus_effect;
    public bool hideWhenLow = true;

    [Header("Placement Settings")]
    public Vector3 spawnOffset = new Vector3(0, -0.3f, 0);
    public Vector3 spawnScale = new Vector3(0.5f, 0.5f, 0.5f);

    private GameObject effectInstance;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerEffect(float currentScore, float threshold)
    {
        if (currentScore >= threshold)
        {
            if (effectInstance == null && Plexus_effect != null)
            {
                // Spawn the effect
                effectInstance = Instantiate(Plexus_effect, transform.position, transform.rotation);

                // Parent to the Image Target
                effectInstance.transform.SetParent(this.transform);

                // Apply position and scale
                effectInstance.transform.localPosition = spawnOffset;
                effectInstance.transform.localScale = spawnScale;
            }
        }
        else if (hideWhenLow)
        {
            if (effectInstance != null)
            {
                Destroy(effectInstance);
            }
        }
    }
}