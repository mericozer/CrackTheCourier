using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private Animator transitionAnim;
    private void Start()
    {
        transitionAnim = GetComponent<Animator>();

    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Tranisiton(sceneName));

    }

    IEnumerator Tranisiton(string sceneName)
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
