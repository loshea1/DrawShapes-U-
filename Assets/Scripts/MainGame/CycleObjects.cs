using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    private int currentIndex = 0;

    private void Start()
    {
        ShowCurrent();
    }

    public void Forward()
    {
        currentIndex++;

        if (currentIndex >= objects.Length)
            currentIndex = 0;

        ShowCurrent();
    }

    public void Backward()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = objects.Length - 1;

        ShowCurrent();
    }

    private void ShowCurrent()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }
    }
    
        public TouchDraw GetCurrentTouchDraw()
    {
        return objects[currentIndex].GetComponent<TouchDraw>();
    }


}
