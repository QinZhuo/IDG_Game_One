using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;

public class UnityGameObjectPool :MonoBehaviour {
    public Dictionary<PoolType, Pool> pools = new Dictionary<PoolType, Pool>();
    
    public List< int> poolsSize;
    public static UnityGameObjectPool instance;
    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    public void Insert(NetObject netObject)
    {
        
        GetPool(netObject.GetPoolType()).Insert(netObject);
    }
    public void Recover(NetObject netObject)
    {
      
        GetPool(netObject.GetPoolType()).Recover(netObject);
       
    }
    public PoolObject Get(NetObject netObject, V2 position, Ratio rotation)
    {
       
        return GetPool(netObject.GetPoolType()).Get(netObject,position,rotation);
    }
    protected Pool GetPool(PoolType type)
    {
        Pool pool = null;
        if (!pools.TryGetValue(type, out pool))
        {
            pool = new Pool(poolsSize[(int)(type)]);
            pools.Add(type,pool);
        }
        return pool;
    }
}
public class Pool
{
    List<NetObject> objects = new List<NetObject>();
    Queue<NetObject> pool = new Queue<NetObject>();
    
    public int maxSize;
    public Pool(int size)
    {
        maxSize = size;
        Debug.Log("init Pool:" + size);
    }
    public void Insert(NetObject poolObject)
    {
        objects.Add(poolObject);
    }
    public void Recover(NetObject poolObject)
    {
        if (objects.Contains(poolObject))
        {
            objects.Remove(poolObject);
            
        }
        poolObject.PoolRecover();
        pool.Enqueue(poolObject);
    }
    public PoolObject Get(NetObject netObject,V2 position,Ratio rotation)
    {
        if (pool.Count > 0)
        {
            NetObject obj = pool.Dequeue();
            obj.PoolReset(position, rotation);
            Debug.Log("pop");
            return obj;
        }
        else
        {
            if(objects.Count<maxSize)
            {
                NetObject netObj =GameObject.Instantiate(netObject.gameObject, position.ToVector3(), rotation.ToUnityRotation()).GetComponent<NetObject>();
                netObj.net.Init(position, rotation);
              
                objects.Add(netObj);
                Debug.Log("new");
                return netObj;
            }
            else
            {
                NetObject obj = objects[0];
                objects.Remove(obj);
                objects.Add(obj);
                obj.PoolRecover();
                obj.PoolReset(position, rotation);
                Debug.Log("reset");
                return obj;
            }
        }
        
    }
}
public interface PoolObject
{
    PoolType GetPoolType();
    void PoolReset(V2 position, Ratio rotation);
    void PoolRecover();
}
public enum PoolType
{
    bullet=0,
}