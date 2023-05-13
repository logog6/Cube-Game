using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelData
{

    public float BestTime { set; get; }
    public float SilverTime { set; get; }
    public float GoldTime { set; get; }

    public LevelData(string levelName)
    {
        string data = PlayerPrefs.GetString(levelName);
        if(data == "")
        {
            return;
        }
        string[] allData = data.Split('&');
        BestTime = float.Parse(allData[0]);
        SilverTime = float.Parse(allData[1]);
        GoldTime = float.Parse(allData[2]);
    }
}
    

public class MainMenu : MonoBehaviour
{
    public Sprite[] borders;
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;
    public GameObject shopButtonPrefab;
    public GameObject shopItemsContainer;
    public Text skinText;

    public Material playerMaterial;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private bool nextLevelLocked = false;

    private int[] costes = { 0, 150, 150, 150, 300, 300, 300, 300, 500, 500, 500, 500, 1000, 1250, 1500, 2000 };

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        skinText.text = "Point: " + GameManager.Instance.currency.ToString();
        cameraTransform = Camera.main.transform;

        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
        foreach(Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform,false);
            LevelData level = new LevelData(thumbnail.name);

            string minutes = ((int)level.BestTime / 60).ToString("00");
            string seconds = (level.BestTime % 60).ToString("00.00");

            GameObject bottomPanel = container.transform.GetChild(0).GetChild(0).gameObject;
            
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.BestTime != 0.0f) ? minutes + ":" + seconds : "Brak czasu!" ;

            container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked;
            container.GetComponent<Button>().interactable = !nextLevelLocked;

            if (level.BestTime == 0.0f)
            {
                nextLevelLocked = true;
            }
            else if (level.BestTime < level.GoldTime)
            {
                //złota tekstura 
                bottomPanel.GetComponentInParent<Image>().sprite = borders[2];
            }
            else if(level.BestTime < level.SilverTime)
            {
                //srebrna teksura
                bottomPanel.GetComponentInParent<Image>().sprite = borders[1];
            }
            else
            {
                //bronzowa tekstura
                bottomPanel.GetComponentInParent<Image>().sprite = borders[0];
            }
            
            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }

        int textureIndex = 0;
        Sprite[] textures = Resources.LoadAll<Sprite>("Player");
        foreach (Sprite texture in textures)
        {
            GameObject container = Instantiate(shopButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = texture;
            container.transform.SetParent(shopItemsContainer.transform, false);

            int index = textureIndex;
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
            container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costes[index].ToString();
            if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
            {
                container.transform.GetChild(0).gameObject.SetActive(false);
            }
            textureIndex++;
        }
    }

    private void Update()
    {
       if(cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, 4 * Time.deltaTime);
        } 
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu (Transform menuTranform)
    {
        cameraDesiredLookAt = menuTranform;
    }

    private void ChangePlayerSkin (int index)
    {
        if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
        {

            float x = (index % 4) * 0.25f;
            float y = (index / 4) * 0.25f;

            if (y == 0.0f)
                y = 0.75f;
            else if (y == 0.25f)
                y = 0.5f;
            else if (y == 0.50f)
                y = 0.25f;
            else if (y == 0.75f)
                y = 0.0f;

            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
            GameManager.Instance.currentSkinIndex = index;
            GameManager.Instance.Save();
        }
        else
        {
            // nie masz skinu, kopisz? 
            int cost = costes[index];
            if(GameManager.Instance.currency >= cost)
            {
                GameManager.Instance.currency -= cost;
                GameManager.Instance.skinAvailability += 1 << index;
                GameManager.Instance.Save();
                skinText.text = "Skin: " + GameManager.Instance.currency.ToString();
                shopItemsContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
                ChangePlayerSkin(index);
            }
        }
    }
}
