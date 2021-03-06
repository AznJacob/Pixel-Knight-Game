using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel(int toLoad)
    {
        StartCoroutine(LoadLevel(toLoad));
    }

    public IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }
}
