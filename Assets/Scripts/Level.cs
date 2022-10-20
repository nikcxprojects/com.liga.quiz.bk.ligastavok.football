using UnityEngine;

[CreateAssetMenu(fileName = "Level")]
public class Level : ScriptableObject
{
    public Question[] questions;
}

[System.Serializable]
public struct Question
{
    public string question;
    public string[] answer;
    public int indexRight;
}