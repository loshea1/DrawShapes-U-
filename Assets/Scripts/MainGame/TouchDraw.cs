using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BurtSharp.UnityInterface;

public class TouchDraw : MonoBehaviour
{
    Coroutine drawing;
    public SpriteRenderer targetSprite;
    public PolygonCollider2D drawArea;
    public ConnectionSetter robot;

    private Vector2 lastPos;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos3D = robot.GetToolPosition();
        Vector2 currentPos = new Vector2(currentPos3D.x, currentPos3D.y);

        //Check if the robot moved enough to start drawing
        if (Vector2.Distance(currentPos, lastPos) > 0.001f)
        {
            if(drawing == null)
            StartLine();
        }
        else
        {
            if(drawing == null)
            FinishLine();
        }
    }

    void StartLine()
    {
        if(drawing != null)
        {
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
    }

    void FinishLine()
    {
        if(drawing != null)
        {
        StopCoroutine(drawing);
        drawing = null;
        }
    }


IEnumerator DrawLine()
{
    GameObject go = Instantiate(Resources.Load<GameObject>("Line"), targetSprite.transform);
    LineRenderer line = go.GetComponent<LineRenderer>();

    line.positionCount = 0;
    line.useWorldSpace = true;

    float z = targetSprite.transform.position.z - 0.01f;

    while (true)
    {
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 worldPos = robot.GetToolPosition();
        worldPos.z = z;

        //Convert to 2D for collider check
        Vector2 point2D = worldPos;

        // Only draw inside sprite
        //if (targetSprite.bounds.Contains(worldPos))
        if(drawArea.OverlapPoint(point2D))
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, worldPos);
        }

        yield return null;
    }
}

public void ClearLines()
{
    // Stop drawing if currently drawing
    if (drawing != null)
    {
        StopCoroutine(drawing);
        drawing = null;
    }

    // Destroy all line objects under the target sprite
    for (int i = targetSprite.transform.childCount - 1; i >= 0; i--)
    {
        Destroy(targetSprite.transform.GetChild(i).gameObject);
    }
}


  /*  IEnumerator DrawLine()
    {
        GameObject newGameObject = Instantiate(Resources.Load("Line") as GameObject);
        LineRenderer line = newGameObject.GetComponent<LineRenderer>();

        line.sortingLayerName = "TopMost";
        line.sortingOrder = 1000;
        line.positionCount = 0;

        while(true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount-1, position);
            yield return null;
        }

    }
    */
}
