using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target; 
    
    void LateUpdate()
    {
        var targetPos = target.transform.position;
        transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
    }
}
