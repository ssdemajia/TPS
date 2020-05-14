using Shaoshuai.Core;

public class AssaultRifle : Shooter
{
    public override void Fire()
    {
        base.Fire();
        if (canFire)
        {

        }
    }

    void Update()
    {
        if (GameManager.Instance.inputController.Reload)
        {
            Reload();
        }
    }
}
