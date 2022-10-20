using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    [Header("Scores")]
    [SerializeField] private Transform _scoreWindowTransform;
    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private Sprite[] _scoreSprites;
    private List<GameObject> _scoreInitialied = new List<GameObject>();

    [Header("Level")]
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private Sprite[] _levelSprites;

    public void LoadLevel(int index)
    {
        PlayerPrefs.SetInt("LevelLoaded", index);
        SceneManager.LoadScene("Level");
    }

    public void ViewLevels()
    {
        for(int i = 0; i < _levelButtons.Length; i++)
        {
            Image levelImage = _levelButtons[i].GetComponent<Image>();
            if(PlayerPrefs.GetString($"Level {i + 1}") == "Ended")
                levelImage.sprite = _levelSprites[1];
            else
                levelImage.sprite = _levelSprites[0];
        }
    }


    public void ViewScores()
    {
        List<int> scores = Database.instance.GetScoresDatabase();

        if (scores.Count == 0)
            return;

        for (int i = scores.Count - 1; i > -1; i--)
        {
            GameObject scoreObj = Instantiate(_scorePrefab, _scoreWindowTransform);
            scoreObj.GetComponent<RectTransform>().localScale = Vector3.one;

            Text scoreText = scoreObj.GetComponentInChildren<Text>();
            scoreText.text = $"{scores[i]}sec";

            if (scores.Count - i > 3)
                scoreObj.GetComponent<Image>().sprite = _scoreSprites[1];

            _scoreInitialied.Add(scoreObj);
        }
    }

    /// <summary>
    /// Clears information about records
    /// </summary>
    public void ClearScores()
    {
        foreach (GameObject score in _scoreInitialied)
            Destroy(score);

        _scoreInitialied.Clear();
    }

}
