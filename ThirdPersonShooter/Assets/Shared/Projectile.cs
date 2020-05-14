using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 100;
    [SerializeField] float timeToLive = 3;
    [SerializeField] int damage = 1;
    [SerializeField] Transform bullethole;
    Vector3 destination;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.VelocityChange);
        Destroy(gameObject, timeToLive);
    }
    private void Update()
    {
        if (isDestinationReached())
            Destroy(gameObject);
        if (destination != Vector3.zero)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 15f))
        {
            Check(hit);
        }
    }

    /* 子弹碰撞到目标物体时 */
    private void Check(RaycastHit hitinfo)
    {
        var destructable = hitinfo.transform.GetComponent<Destructable>();
        // 设置弹痕
        destination = hitinfo.point + hitinfo.normal * 0.002f;
        Transform hole = Instantiate(bullethole, destination, Quaternion.LookRotation(hitinfo.normal) * Quaternion.Euler(0, 180, 0));
        hole.SetParent(hitinfo.transform);

        if (destructable == null)
            return;
        // 对角色进行扣血
        if (destructable.parent != null)
        {
            var rootDestructable = destructable.parent;
            rootDestructable.TakeDamage(damage, destructable.name);
        } else
        {
            destructable.TakeDamage(damage, "");
        }
        
    }

    bool isDestinationReached()
    {
        if (destination == Vector3.zero)
            return false;
        Vector3 directionToDestination = destination - transform.position;
        float dot = Vector3.Dot(directionToDestination, transform.forward);
        if (dot < 0f)
            return true;
        return false;
    }
}
