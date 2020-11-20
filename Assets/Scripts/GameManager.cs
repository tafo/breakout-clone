using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject ballPrefab;
        public GameObject playerPrefab;

        public Text scoreText;
        public Text ballsText;
        public Text levelText;
        public Text highScoreText;

        public GameObject panelMenu;
        public GameObject panelPlay;
        public GameObject panelLevelCompleted;
        public GameObject panelGameOver;

        public GameObject[] levels;

        public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
        State _state;

        public GameObject _currentBall;
        public GameObject _currentLevel;
        public bool _isSwitchingState;

        public static GameManager Instance { get; private set; }

        private int _score;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                scoreText.text = "Score: " + Score + "(HighScore:" + PlayerPrefs.GetInt("highScore") + ")";
            }
        }

        private int _level;

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                levelText.text = "Level: " + Level;
            }
        }

        private int _balls;

        public int Balls
        {
            get => _balls;
            set
            {
                _balls = value;
                ballsText.text = "Balls: " + Balls;
            }
        }

        private void Start()
        {
            Instance = this;
            SwitchState(State.MENU);
        }

        public void PlayClicked()
        {
            SwitchState(State.INIT);
        }

        public void SwitchState(State newState, float delay = 0)
        {
            StartCoroutine(SwitchDelay(newState, delay));
        }

        private IEnumerator SwitchDelay(State newState, float delay)
        {
            _isSwitchingState = true;
            yield return new WaitForSeconds(delay);
            EndState();
            _state = newState;
            StartState(newState);
            _isSwitchingState = false;
        }

        private void StartState(State newState)
        {
            switch (newState)
            {
                case State.MENU:
                    Cursor.visible = true;
                    panelMenu.SetActive(true);
                    break;
                case State.INIT:
                    Cursor.visible = false;
                    panelPlay.SetActive(true);
                    Score = 0;
                    Level = 0;
                    Balls = 3;
                    if (_currentLevel != null)
                    {
                        Destroy(_currentLevel);
                    }
                    Instantiate(playerPrefab);
                    SwitchState(State.LOADLEVEL);
                    break;
                case State.PLAY:
                    break;
                case State.LEVELCOMPLETED:
                    Destroy(_currentBall);
                    Destroy(_currentLevel);
                    Level++;
                    panelLevelCompleted.SetActive(true);
                    SwitchState(State.LOADLEVEL, 2f);
                    break;
                case State.LOADLEVEL:
                    if (Level >= levels.Length)
                    {
                        SwitchState(State.GAMEOVER);
                    }
                    else
                    {
                        _currentLevel = Instantiate(levels[Level]);
                        SwitchState(State.PLAY);
                    }
                    break;
                case State.GAMEOVER:
                    if (Score > PlayerPrefs.GetInt("highScore"))
                    {
                        PlayerPrefs.SetInt("highScore", Score);
                    }
                    panelGameOver.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            switch (_state)
            {
                case State.MENU:
                    break;
                case State.INIT:
                    break;
                case State.PLAY:
                    if (_currentBall == null)
                    {
                        if (Balls > 0)
                        {
                            _currentBall = Instantiate(ballPrefab);
                        }
                        else
                        {
                            SwitchState(State.GAMEOVER);
                        }
                    }
                    if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                    {
                        SwitchState(State.LEVELCOMPLETED);
                    }
                    break;
                case State.LEVELCOMPLETED:
                    break;
                case State.LOADLEVEL:
                    break;
                case State.GAMEOVER:
                    if (Input.anyKeyDown)
                    {
                        SwitchState(State.MENU);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EndState()
        {
            switch (_state)
            {
                case State.MENU:
                    panelMenu.SetActive(false);
                    break;
                case State.INIT:
                    break;
                case State.PLAY:
                    break;
                case State.LEVELCOMPLETED:
                    panelLevelCompleted.SetActive(false);
                    break;
                case State.LOADLEVEL:
                    break;
                case State.GAMEOVER:
                    panelPlay.SetActive(false);
                    panelGameOver.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
