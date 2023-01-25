using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
        foreach(Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform,false);

            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
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
        //Camera.main.transform.LookAt(menuTranform.position);
        cameraDesiredLookAt = menuTranform;
    }
}
