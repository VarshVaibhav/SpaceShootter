using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour
{
    public bool isCoopMode = false;
    public bool gameOver = true;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _coopPlayers;
    [SerializeField] private GameObject _pauseMenuPanel;
    private Animator _pauseAnimator;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    void Update()
    {
        if (gameOver== true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButton("Fire"))
            {
                if (isCoopMode == false)
                {
                    Instantiate(_player, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Instantiate(_coopPlayers, Vector3.zero, Quaternion.identity);
                }
                gameOver = false;
                _uiManager.HideTitleScreen();
                _spawnManager.StartSpawnRoutines();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main_Menu");
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanel.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
