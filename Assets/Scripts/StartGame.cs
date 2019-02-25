using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string favoriteAction;
    public int index;
    public int material;
    public string playerName;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void PlayerSelected(string _favoriteAction, int _index, string _playerName)
    {
        favoriteAction = _favoriteAction;
        index = _index;
        playerName = _playerName;
    }

    public void PetSelected(int _material)
    {
        material = _material;
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene(0);
    }
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //Invoke("InitialiseStuff", 0.5f);
        InitialiseStuff();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void InitialiseStuff()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject pet = GameObject.FindGameObjectWithTag("Pet");

        Renderer[] temp = pet.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].sharedMaterial = pet.GetComponent<AgentVisuals>().GetAllClothes()[material];
        }

        player.GetComponent<PlayerCharacter>().SetFavoriteAction(favoriteAction);
        player.GetComponent<PlayerCharacter>().SetName(playerName);
        player.GetComponent<PlayerCharacter>().SetClothes(player.GetComponent<PlayerVisuals>().GetAllClothes()[index]);

        gameObject.SetActive(false);
    }

}
