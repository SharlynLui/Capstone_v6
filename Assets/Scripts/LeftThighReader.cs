using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftThighReader : MonoBehaviour
{
    public TextAsset JSONText;

    [System.Serializable]
    public class FrameData
    {
        public float left_elbow;
        public float right_elbow;
        public float left_arm;
        public float right_arm;
        public float left_knee;
        public float right_knee;
        public float left_thigh;
        public float right_thigh;
        public float overall;
    }

    [System.Serializable]
    public class FrameList
    {
        public FrameData[] frames;
    }

    public FrameList myFrameList = new FrameList();

    [Header("Orb Settings")]
    public float threshold = 0.80f;
    public float frameRate = 5.0f;
    public GameObject blueOrbPrefab;
    public GameObject redOrbPrefab;

    private GameObject currentOrbInstance;
    private int lastStatus = -1; // 1 for Blue, 2 for Red

    void Start()
    {
        if (JSONText != null)
        {
            // Convert the JSON text into C# object list
            myFrameList = JsonUtility.FromJson<FrameList>(JSONText.text);

            if (myFrameList.frames != null && myFrameList.frames.Length > 0)
            {
                StartCoroutine(PlayOrbSequence());
            }
        }
    }

    IEnumerator PlayOrbSequence()
    {
        int index = 0;
        while (true)
        {
            FrameData current = myFrameList.frames[index];

            // Logic: 1 = Good (Blue), 2 = Bad (Red)
            // CHANGE HERE FOR EACH SCRIPT FOR EACH PART
            int statusNeeded = (current.left_thigh >= threshold) ? 1 : 2;

            if (statusNeeded != lastStatus)
            {
                SwapOrb(statusNeeded);
                lastStatus = statusNeeded;
            }

            index = (index + 1) % myFrameList.frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }

    void SwapOrb(int type)
    {
        if (currentOrbInstance != null) Destroy(currentOrbInstance);

        GameObject prefab = (type == 1) ? blueOrbPrefab : redOrbPrefab;

        if (prefab != null)
        {
            currentOrbInstance = Instantiate(prefab, transform.position, transform.rotation);
            currentOrbInstance.transform.SetParent(this.transform);
            currentOrbInstance.transform.localPosition = new Vector3(0, 0.2f, 0); // X: Left Right, Y: Down Up, Z: Forward Back
            currentOrbInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}