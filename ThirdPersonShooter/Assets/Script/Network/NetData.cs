using System.Collections.Generic;

abstract class NetData
{
    public string tag;
    public string name;
    public bool active = true;
    public bool isTrigger = false;
    bool start = false;
    public int clientID = -1;
    public TransformComponent transform;
    public NetworkClient client;
    public List<BaseComponent> components;

    public bool IsLocalPlayer
    {
        get
        {
            return client.IsLocalId(clientID);
        }
    }

    public abstract string PrefabPath();
    protected abstract void FrameUpdate();

    // 返回netData对应玩家的输入
    public NClientInput clientInput
    {
        get { return client.inputController.getClientInput(this.clientID); }
    }
    public virtual void Init(NetworkClient client)
    {
        this.client = client;
        client.inputController.frameUpdate += DataFrameUpdate;
        components = new List<BaseComponent>();
        transform = new TransformComponent();
        transform.Init(this);
    }

    public T AddComponent<T>() where T: BaseComponent, new()
    {
        T component = new T();
        this.components.Add(component);
        return component;
    }
    protected void DataFrameUpdate()
    {
        if (!active) return;
        if (!start)
        {
            Start();
            start = true;
        }
        FrameUpdate();
        foreach (var item in components)
        {
            item.Update();
        }
    }

    public virtual void Start() { }
    public void Destroy()
    {
        active = false;
        client.inputController.frameUpdate -= DataFrameUpdate;
    }
}

