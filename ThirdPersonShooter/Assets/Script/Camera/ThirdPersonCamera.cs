using Shaoshuai.Core;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Vector3 cameraOffset;
    [SerializeField] float damping = 1f;

    Transform cameraLookTarget;
    Transform localPlayer;
    void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
    }

    void HandleOnLocalPlayerJoined (Transform player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.Find("CameraLookTarget");
        if (cameraLookTarget == null)
        {
            cameraLookTarget = localPlayer;
        }
    }
   
    void LateUpdate()
    {
        // 还未启动
        if (cameraLookTarget == null)
            return;
        Vector3 targetPosition = cameraLookTarget.position +
            localPlayer.transform.forward * cameraOffset.z +
            localPlayer.transform.up * cameraOffset.y + 
            localPlayer.transform.right* cameraOffset.x;

        // 角度绕目标旋转
        Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

        // 当摄像头碰到墙壁时，需要把摄像头往前移
        Vector3 collisionTargetPoint = cameraLookTarget.position;
        HandleCameraCollision(collisionTargetPoint, ref targetPosition);
        // 相对于目标的位置
        transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraLookTarget.rotation, damping * Time.deltaTime);
    }

    void HandleCameraCollision(Vector3 toTarget, ref Vector3 fromTarget)
    {
        Debug.DrawLine(fromTarget, toTarget, Color.black);
        RaycastHit hit;
        if (Physics.Linecast(fromTarget, toTarget, out hit))
        {
            fromTarget = new Vector3(hit.point.x, fromTarget.y, hit.point.z);
        }
    }
}
