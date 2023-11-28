using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneTransition : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadSceneAsync("DemoLevel", LoadSceneMode.Additive);
    }
}
