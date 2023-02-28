using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private const float timeBefor = 3.0f;

    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public GameObject pauseMenu;
    public GameObject finishMenu;
    public Transform respawnPoint;
    public Text finishTimerText;
    public Text timerText;
    private GameObject player;

    private float startTime;
    private float levelDuration;
    public float silverTime;
    public float goldTime;


    private void Start()
    {
        instance = this;
        pauseMenu.SetActive(false);
        finishMenu.SetActive(false);
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        Respawn();
    }

    private void Update()
    {
        if(player.transform.position.y < -10.0f)
        {
            Respawn();
            RestartLevel();
        }

        if (Time.time - startTime < timeBefor)
        {
            return;
        }

        levelDuration = Time.time - (startTime + timeBefor);
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");

        timerText.text = minutes + ":" + seconds;
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = (pauseMenu.activeSelf) ? 0 : 1;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory()
    {
        foreach(Transform t in finishMenu.transform.parent)
        {
            t.gameObject.SetActive(false);
        }
        
        finishMenu.SetActive(true);

        levelDuration = Time.time - (startTime + timeBefor);
        string minutes = ((int)levelDuration / 60).ToString("00");
        string seconds = (levelDuration % 60).ToString("00.00");
        finishTimerText.text = minutes + ":" + seconds;

        Rigidbody rigit = player.GetComponent<Rigidbody>();
        rigit.constraints = RigidbodyConstraints.FreezePosition;

        

        if (levelDuration < goldTime)
        {
            GameManager.Instance.currency += 50;
            finishTimerText.color = Color.yellow;
        }
        else if (levelDuration < silverTime)
        {
            GameManager.Instance.currency += 25;
            finishTimerText.color = Color.gray;
        }
        else
        {
            GameManager.Instance.currency += 10;
            finishTimerText.color = new Color(0.8f, 0.5f, 0.2f, 1.0f);//(205/255 = 0.8 , 127/255 = 0.5, 50/255 = 0.2) kolor brązowy 
        }
        GameManager.Instance.Save();
        //"30&60&45"
        LevelData level = new LevelData(SceneManager.GetActiveScene().name);
        string saveString =  "";
        saveString += (level.BestTime > levelDuration || level.BestTime == 0.0f) ? levelDuration.ToString() : level.BestTime.ToString();
        saveString += '&';
        saveString += silverTime.ToString();
        saveString += '&';
        saveString += goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);

        //SceneManager.LoadScene("MainMenu");
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.position;
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
