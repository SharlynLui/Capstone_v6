using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class OrbController : MonoBehaviour
{
    [Header("Orb References")]
    public GameObject blueOrb;
    public GameObject redOrb;

    [Header("Settings")]
    public float threshold = 0.60f;

    private PostureData postureData;
    private int currentFrame = 0;

    void Start()
    {
        if (blueOrb == null) Debug.LogError("blueOrb is not assigned!");
        if (redOrb == null) Debug.LogError("redOrb is not assigned!");

        StartCoroutine(LoadJSON());
    }

    IEnumerator LoadJSON()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "dummy_data.json");

        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                postureData = JsonUtility.FromJson<PostureData>(json);
                Debug.Log($"JSON loaded. Total frames: {postureData.frames.Length}");
                RenderFrame(currentFrame);
            }
            else
            {
                Debug.LogError($"Failed to load JSON: {request.error}");
            }
        }
    }

    void RenderFrame(int frameIndex)
    {
        if (postureData == null || postureData.frames == null)
        {
            Debug.LogError("No posture data loaded.");
            return;
        }

        if (frameIndex >= postureData.frames.Length)
        {
            Debug.LogWarning("Frame index out of range.");
            return;
        }

        FrameData frame = postureData.frames[frameIndex];
        float rightElbowValue = frame.right_elbow;

        Debug.Log($"Frame {frameIndex} | right_elbow: {rightElbowValue} | Threshold: {threshold}");

        if (rightElbowValue < threshold)
        {
            redOrb.SetActive(true);
            blueOrb.SetActive(false);
            Debug.Log("Result: RED ORB (below threshold)");
        }
        else
        {
            blueOrb.SetActive(true);
            redOrb.SetActive(false);
            Debug.Log("Result: BLUE ORB (good posture)");
        }
    }
}