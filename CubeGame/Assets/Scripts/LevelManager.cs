using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public GameObject pauseMenu;
    public Transform respawnPoint;
    private GameObject player;

    private float startTime;
    public float silverTime;
    public float goldTime;


    private void Start()
    {
        instance = this;
        pauseMenu.SetActive(false);
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        Respawn();
    }

    private void Update()
    {
        if(player.transform.position.y < -10.0f)
        {
            Respawn();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory()
    {
        float duration = Time.time - startTime;
        if(duration < goldTime)
        {
            GameManager.Instance.currency += 50;
        }
        else if (duration < silverTime)
        {
            GameManager.Instance.currency += 25;
        }
        else
        {
            GameManager.Instance.currency += 10;
        }
        GameManager.Instance.Save();
        //"30&60&45"
        LevelData level = new LevelData(SceneManager.GetActiveScene().name);
        string saveString =  "";
        saveString += (level.BestTime > duration || level.BestTime == 0.0f) ? duration.ToString() : level.BestTime.ToString();
        saveString += '&';
        saveString += silverTime.ToString();
        saveString += '&';
        saveString += goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);

        SceneManager.LoadScene("MainMenu");
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.position;
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
