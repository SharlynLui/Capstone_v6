using System;

[Serializable]
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
}

[Serializable]
public class PostureData
{
    public FrameData[] frames;
}