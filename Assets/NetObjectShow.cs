using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
namespace IDG.FightClient
{
    //
    abstract public class NetObjectShow<T> : MonoBehaviour where T:NetData,new()
    {
      
        //public static void Instantiate(NetObjectShow netObject,V2 position,Ratio rotation)
        //{
        //    //NetInfo netinfo = Instantiate(netObject.gameObject, position.ToVector3(), rotation.ToUnityRotation()).GetComponent<NetObject>().net;

        //    //netinfo.Init( position,rotation);
        //    UnityGameObjectPool.instance.Get(netObject, position, rotation);
        //}
        public NetData data;
        // Use this for initialization

        protected void Start()
        {

          
            InitData();
            InitCollider();
            //net.Position = new V2(transform.position.x, transform.position.z);
            // net.Rotation =new Ratio( transform.rotation.y);
            //Debug.Log("net.Input.framUpdate += FrameUpdate;");
        }
        protected void InitData() {
            if (data == null)
            {
               
                data = new T();
                data.show = this;
                data.Init();
               
                data.Position = new V2(transform.position.x, transform.position.z);
                data.Start();
            }
        }
        protected void InitCollider()
        {
            Collider2DBase_IDG collider2D = GetComponent<Collider2DBase_IDG>();
            if (collider2D != null)
            {
                Debug.Log("collider2D");
                data.Shap= collider2D.GetShap();
            }
        }
        protected void LerpNetPos(float timer)
        {
            if (data == null) return;
            if (Vector3.Distance(transform.position, data.Position.ToVector3()) > 0.1 * timer)
            {
                MoveSpeed(1);
            }
            else
            {
                MoveSpeed(0);
            }
            transform.position = Vector3.Lerp(transform.position, data.Position.ToVector3(), timer);
            transform.rotation = Quaternion.Euler(0, -data.Rotation.ToFloat(), 0);
            
        }
        // Update is called once per frame
        protected void Update()
        {
            LerpNetPos(Time.deltaTime*10);
        }

        
        private void OnDrawGizmos()
        {
            if (data==null||data.Shap == null) return;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(data.Shap.position.ToVector3(), new Vector3(data.Width.ToFloat(), 0, data.Height.ToFloat()));
            for (int i = 0; i < data.Shap.PointsCount; i++)
            {
                Gizmos.DrawCube((data.Shap.GetPoint(i) + data.Position).ToVector3(), Vector3.one * 0.1f);
            }
        }

     

        public virtual void PoolReset(V2 position, Ratio rotation)
        {
            GetComponent<MeshRenderer>().enabled = true;
            transform.position = position.ToVector3();
            transform.rotation = rotation.ToUnityRotation();
            data.Reset(position, rotation);
        }

        public virtual void PoolRecover()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        protected virtual void MoveSpeed(float speed)
        {

        }
    }
    //[System.Serializable]

    /// <summary>
    /// 基础网络数据类 所有网络相关的物体数据基类（需同步位置与需要帧调用的类继承此类）
    /// </summary>
    public abstract class NetData
    {
        public string tag;
        public string name;
        public bool active=true;
        private V2 _position=new V2();
        private V2 _lastPos=new V2();
        private Ratio _lastRota = new Ratio();
        public bool usePhysicsCheck=false;
        // public CollisonInfo collisonInfo = new CollisonInfo();
        protected List<NetData> lastCollisonDatas = new List<NetData>();
        public List<NetData> collisonDatas=new List<NetData>();
        public NetData parent;
        public MonoBehaviour show;
        public Ratio Width
        {
            get
            {
                return Shap.width;
            }
        }
        public Ratio Height
        {
            get
            {
                return Shap.height;
            }
        }
        public List<Tree4> trees = new List<Tree4>();
        public bool isTrigger=false;
        private Ratio _rotation = new Ratio();
        protected abstract string PrefabPath();
        protected abstract void FrameUpdate();
        protected void DataFrameUpdate()
        {
            if (!active) return;
            PhysicsEffect();
            FrameUpdate();
            //this.collisonInfo.Start();
            if (usePhysicsCheck)
            {
                foreach (var other in this.collisonDatas)
                {
                    if (!lastCollisonDatas.Contains(other))
                    {
                        OnPhysicsCheckEnter(other);
                    }
                    else
                    {
                        lastCollisonDatas.Remove(other);
                    }
                    //Debug.Log("1 "+this.collisonDatas+" "+this.collisonDatas.Count);
                    OnPhysicsCheckStay(other);
                }
                foreach (var other in lastCollisonDatas)
                {
                    OnPhysicsCheckExit(other);
                }
            }
            lastCollisonDatas.Clear();
            lastCollisonDatas.AddRange(collisonDatas);

            //  collisonDatas = ShapPhysics.CheckAll(this);
           
            
            collisonDatas.Clear();
        }
        public virtual void Init()
        {
            Input.framUpdate += DataFrameUpdate;
           
            Debug.Log(name+"init");
        }
        public bool CheckCollision(NetData a)
        {
           
            foreach (var item in collisonDatas)
            {

                if (!item.isTrigger)
                {
                    return true;
                }
            }

            return false;
        }
        public virtual void Start()
        {

        }

        public virtual void OnPhysicsCheckStay(NetData other)
        {
           // UnityEngine.Debug.Log("Stay触发");
        }
        public virtual void OnPhysicsCheckEnter(NetData other)
        {
          //  UnityEngine.Debug.Log("Enter触发");
        }
        public virtual void OnPhysicsCheckExit(NetData other)
        {
            // UnityEngine.Debug.Log("Exit触发");
        }
        public void Reset(V2 position,Ratio rotation)
        {
            _position = position;
            _rotation = rotation;
            Debug.Log("reset");

        }
        public GameObject GetPrefab()
        {
            return Resources.Load(PrefabPath())as GameObject;
        }
        
        public static GameObject Instantiate<T>(NetData data) where T:NetData,new()
        {
            GameObject obj = GameObject.Instantiate(data.GetPrefab(), data.Position.ToVector3(), data.Rotation.ToUnityRotation());
            obj.GetComponent<NetObjectShow<T>>().data = data;
            data.show = obj.GetComponent<NetObjectShow<T>>();
            return obj;
        }
        public static void Destory<T>(MonoBehaviour show) where T : NetData, new()
        {
            //if (show == null) return;
            if (show == null) { Debug.Log("show is Null"); }
            (show as NetObjectShow<T>).data.Destory();
            GameObject.Destroy(show.gameObject);
        }
        private void Destory()
        {
            this.active = false;
            ShapPhysics.Remove(this);
        }
        public V2 forward
        {
            get
            {
            //      Debug.Log(_rotation);
                return V2.Parse(_rotation);
            }
        }
        public V2 Position
        {
            get
            {
                if (parent != null)
                {
                    return parent.Position;
                }
                return _position;
            }
            set
            {
                if (Shap == null)
                {
                    _position = value;
                    _lastPos = _position;
                   
                }
                else
                {
                    _position = value;
                    Tree4.SetActive(this);

                }

            }
        }
        protected void PhysicsEffect()
        {
            if (Shap == null) return;
            if (isTrigger || !CheckCollision(this))
            {
                if (_position != _lastPos || _rotation != _lastRota)
                {
                    _lastPos = _position;
                    _lastRota = _rotation;
                    Shap.ResetSize();

                    Tree4.Move(this);
                }
            }
            else
            {
                _rotation = _lastRota;
                _position = _lastPos;
            }
        }
        //public Ratio Left
        //{
        //    get
        //    {
        //        return  _shap.left + _previewPos.x;
        //    }
        //}
        //public Ratio Right
        //{
        //    get
        //    {
        //        return _shap.right + _previewPos.x;
        //    }
        //}
        //public Ratio Up
        //{
        //    get
        //    {
        //        return _shap.up + _previewPos.y;
        //    }
        //}
        //public Ratio Down
        //{
        //    get
        //    {
        //        return _shap.down + _previewPos.y;
        //    }
        //}
        public Ratio Rotation
        {
            get
            {
                    return _rotation;
            }
            set
            {
                if (Shap==null)
                {
                    _rotation = value % 360;
                    _lastRota = _rotation;
                }
                else
                {
                    _rotation = value % 360;
                   
                }
               
            }
        }
        public int ClientId=-1;
        private ShapBase _shap;//=new BoxShap(new Ratio(1,2));
        public ShapBase Shap
        {
            get
            {
                return _shap;
            }
            set
            {
                //ShapPhysics.Add(this);
                //_shap = new BoxShap(new Ratio(1, 2));

                if (_shap != null)
                {
                    ShapPhysics.Remove(this);
                }
                if (value != null)
                {
                    
                    _shap = value;
                    _shap.netinfo = this;
                    ShapPhysics.Add(this);
                    
                }
                else
                {
                    _shap = value;
                    
                }
                
            }
        }
        public Ratio deltaTime
        {
            get
            {
                return FightClient.deltaTime;
            }
        }
        public InputUnit Input
        {
            get
            {
                return InputCenter.Instance[this.ClientId];
            }
        }
       
    }
    //[System.Serializable]

    //public class TestShap : ShapBase
    //{

    //    public TestShap(bool isA)
    //    {
           
    //        V2[] v2s;
    //        if (isA)
    //        {
               
    //            v2s = new V2[3];
    //            v2s[0] = new V2(4, 5);
    //            v2s[1] = new V2(4, 11);
    //            v2s[2] = new V2(9, 9);
    //        }
    //        else
    //        {
    //            v2s = new V2[4];
    //            v2s[0] = new V2(7, 3);
    //            v2s[1] = new V2(5, 7);
    //            v2s[2] = new V2(12, 7);
    //            v2s[3] = new V2(10, 2);
    //        }
            
    //        Points = v2s;
    //    }

    //}
    
    
}