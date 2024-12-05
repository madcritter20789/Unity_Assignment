using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterControllerGrid : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving;
    private AStarPathfinding pathfinder;
    private List<Vector3> path;

    void Start()
    {
        pathfinder = FindObjectOfType<AStarPathfinding>();
    }

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                path = pathfinder.FindPath(transform.position, hit.point);
                if (path != null && path.Count > 0)
                {
                    StartCoroutine(MoveAlongPath());
                }
            }
        }
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;
        foreach (Vector3 position in path)
        {
            while (Vector3.Distance(transform.position, position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = position; // Snap to position
        }
        isMoving = false;
    }
}
