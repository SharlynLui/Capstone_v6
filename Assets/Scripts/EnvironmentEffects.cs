using UnityEngine;

public class EnvironmentEffects : MonoBehaviour
{
    // This allows other scripts to find this one easily
    public static EnvironmentEffects Instance;

    [Header("Reward Objects")]
    public GameObject highValuePrefab; // The object that appears at > 0.80
    public bool hideWhenLow = true;    // Should it disappear if score drops?

    private GameObject effectInstance;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerEffect(float currentScore, float threshold)
    {
        if (currentScore >= threshold)
        {
            if (effectInstance == null && highValuePrefab != null)
            {
                // Spawn at the position of the Image Target
                effectInstance = Instantiate(highValuePrefab, transform.position, transform.rotation);

                // Parent it so it stays locked to the image on your phone
                effectInstance.transform.SetParent(this.transform);

                // Reset local position so it's not offset
                effectInstance.transform.localPosition = Vector3.zero;
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