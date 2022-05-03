using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObserverButtons : MonoBehaviour
{
    public static bool listenForHotKey = false; //this is useless

    [SerializeField] KeyCode hotKey = KeyCode.None;

    [SerializeField] MonkeyWithButton monkey;

    private void Start()
    {
        monkey = GetComponent<MonkeyWithButton>();
        if(monkey == null)
        {
            monkey = PreviewTool.previewTool; //the only tool connected to multiple buttons, but the singleton pattern is still in place
        }
    }


    private void Update() //only gets called when gameobject is active
    {
        if(Input.GetKeyDown(hotKey))
        {
            Invoke();
        }
    }

    /// <summary>
    /// Call this function from the button
    /// </summary>
    public void Invoke()
    {
        monkey.Invoke();
    }

    /// <summary>
    /// This function is used for the notification servus of the observer pattern
    /// Virtual because some buttons may have specific needs but share a monkey
    /// </summary>
    /// <param name="command"></param>
    /// <param name="status"></param>
    public virtual void ActivationCheck(Command command, Status status)
    {
        bool activeSetting = monkey.ActivationCheck(command, status);
        gameObject.SetActive(activeSetting);
    }
}
