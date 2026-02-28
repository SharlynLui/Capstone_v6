using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreReader : MonoBehaviour
{
    [Header("Data Input")]
    public TextAsset JSONText;

    // Use the Dynamic script for smooth animation
    public ImgsFillDynamic roundFillController;

    [Header("Settings")]
    public float frameRate = 2.0f;
    public float animationSpeed = 1.0f;

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
        public float overall; // Added based on your JSON needs
    }

    [System.Serializable]
    public class FrameList
    {
        public FrameData[] frames;
    }

    public FrameList myFrameList = new FrameList();

    void Start()
    {
        if (JSONText != null)
        {
            myFrameList = JsonUtility.FromJson<FrameList>(JSONText.text);

            if (myFrameList.frames != null && myFrameList.frames.Length > 0)
            {
                StartCoroutine(PlayScoreSequence());
            }
        }
    }

    IEnumerator PlayScoreSequence()
    {
        int index = 0;
        while (true)
        {
            float currentScore = myFrameList.frames[index].overall;

            // 1. Update the UI
            if (roundFillController != null)
            {
                roundFillController.SetValue(currentScore, false, animationSpeed);
            }

            // 2. Tell the Environment script to check the score
            // We use .Instance so we don't need a public variable slot
            if (EnvironmentEffects.Instance != null)
            {
                EnvironmentEffects.Instance.TriggerEffect(currentScore, 0.80f);
            }

            index = (index + 1) % myFrameList.frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }
}