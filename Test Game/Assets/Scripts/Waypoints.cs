using UnityEngine;

public class Waypoints : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Colliding on " + this.gameObject.tag);

        if (other.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
