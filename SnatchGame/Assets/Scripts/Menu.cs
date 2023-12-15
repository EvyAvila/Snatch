using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button StartButton;

    public Audio GameAudio;

    private void OnEnable()
    {
        SetUIActivation(true);
    }

    private void OnDisable()
    {
        SetUIActivation(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(BeginGame);


        if(GameAudio == null)
        {
            GameAudio = GameObject.Find("MainMenu").GetComponent<Audio>();
        }

        GameAudio.PlayAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BeginGame()
    {
        SceneManager.LoadScene(1);
    }

    private void SetUIActivation(bool condition)
    {
        StartButton.gameObject.SetActive(condition);
    }
}
