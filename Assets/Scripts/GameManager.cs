using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    private int _levelIndex = 0;

    private Question[] _questions;
    private int _questionIndex = 0;
    private int _scoreAnswer = 0;
    private float _timer;
    private bool _getAnswer = false;

    [Header("Main UI")]
    [SerializeField] private GameObject _mainWindow;
    [SerializeField] private GameObject _overWindow;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _indexText;

    [Header("Question")]
    [SerializeField] private Text _questionText;
    [SerializeField] private Text[] _answerTexts;
    [SerializeField] private Sprite[] _answerSprites;

    [Header("GameOver")]
    [SerializeField] private Text _overText;
    [SerializeField] private Sprite[] _overSprites;
    [SerializeField] private Image _overBackgroundImage;
    [SerializeField] private Button _overButton;

    [SerializeField] private AudioClip _buttonClip;

    private void Awake()
    {
        _levelIndex = PlayerPrefs.GetInt("LevelLoaded") - 1;
        _levelText.text = $"Level {_levelIndex + 1}";
        LevelLoad(_levelIndex);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _timerText.text = $"Time: {Mathf.CeilToInt(_timer)}sec";
        _indexText.text = $"{_questionIndex}/5";
    }

    private void LevelLoad(int index)
    {
        _questions = _levels[index].questions;
        LoadQuestion();
    }

    public void LoadQuestion()
    {
        if (_questionIndex >= _questions.Length)
        {
            GameOver();
            return;
        }
        _getAnswer = false;

        foreach (Text answerText in _answerTexts)
            answerText.GetComponentInParent<Image>().sprite = _answerSprites[2];

        _questionText.text = _questions[_questionIndex].question;
        for(int i = 0; i < _questions[_questionIndex].answer.Length; i++)
        {
            Text answerText = _answerTexts[i];
            answerText.text = _questions[_questionIndex].answer[i];
        }

        _questionIndex += 1;
    }

    public void GetAnswerQuestion(int index)
    {
        AudioManager.instance.PlayAudio(_buttonClip);
        if (_getAnswer) return;

        if (index != _questions[_questionIndex - 1].indexRight)
            _answerTexts[index].GetComponentInParent<Image>().sprite = _answerSprites[0];
        else
        {
            _answerTexts[index].GetComponentInParent<Image>().sprite = _answerSprites[1];
            _scoreAnswer += 1;
        }

        _getAnswer = true;
    }

    public void GameOver()
    {
        _mainWindow.SetActive(false);
        _overWindow.SetActive(true);
        _overButton.onClick.AddListener(() => SceneManager.LoadScene("Level"));
        _overText.text = $"{_scoreAnswer} out of 5 Questions";

        if (_scoreAnswer == 5)
        {
            PlayerPrefs.SetString($"Level {_levelIndex + 1}", "Ended");
            _overBackgroundImage.sprite = _overSprites[1];
            _overButton.GetComponentInChildren<Text>().text = "Next Level";
            if (_levelIndex + 2 < 25)
                PlayerPrefs.SetInt("LevelLoaded", _levelIndex + 2);
            Database.instance.AddNewScoreDatabase(Mathf.CeilToInt(_timer));
        }
        else
        {
            _overBackgroundImage.sprite = _overSprites[0];
            _overButton.GetComponentInChildren<Text>().text = "Try Again";
        }
    }

    public void Home() => SceneManager.LoadScene("Menu");
}
