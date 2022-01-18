using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public int numCornerVertices;
    public int numCapVertices;
    private Vector3 zeroVector;
    private Rigidbody2D rb;
    public List<Vector3> pointList;
    public GameObject waypoint;
    [HideInInspector]
    public LineRenderer lineRenderer;
    [HideInInspector]
    public LineRenderer lineRendererPlayer;
    private GameObject touchInstance;
    private Vector2 touchPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        lineRendererPlayer = GetComponent<LineRenderer>();
        lineRendererPlayer.widthMultiplier = 0.2f;
        lineRenderer = new GameObject("Path").AddComponent<LineRenderer>();
        lineRenderer.material = lineRendererPlayer.material;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.sortingOrder = 1;
        zeroVector = Vector3.zero;
        pointList = new List<Vector3>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchInstance = Instantiate<GameObject>(waypoint);
            touchInstance.transform.position = touchPosition;
            pointList.Add(touchPosition);
            Debug.Log(pointList[0]);
        }
        MovePlayer();
    }

    void MovePlayer()
    {   
        if (pointList.Count > 0)
        {
            //Reads the number of points for LineRenderer object from our list
            lineRenderer.positionCount = pointList.Count;
            //Sets the direction of the player
            Vector2 direction = pointList[0] - transform.position;
            //Calculates angle for player's direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (!direction.Equals(zeroVector))
            {
                //-90f makes Player look into the centre
                rb.rotation = angle - 90f;
            }
            
            //Draws the path between the waypoints 
            lineRendererPlayer.SetPosition(0, transform.position);
            lineRendererPlayer.SetPosition(1, pointList[0]);
            lineRenderer.SetPositions(pointList.ToArray());
            lineRenderer.numCornerVertices = numCornerVertices;
            lineRenderer.numCapVertices = numCapVertices;
            lineRenderer.alignment = LineAlignment.TransformZ;
          
            if (pointList.Count > 1)
            {
                if (transform.position.Equals(pointList[0]))
                {
                    pointList.Remove(pointList[0]);                 
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, pointList[0], moveSpeed * Time.deltaTime);
        }
    }
}