using UnityEngine;

public class SceneData : SingletonMono<SceneData>
{
    //玩家数据
    public GameObject player;
    public Vector3 pos;
    //场景物体数据
    public BoxCollider2D leftColl;
    public bool isCanTrigger;
    public SaveGameData info;
    public void Init(SaveGameData info = null)
    {
        if (info == null)
        {
            player.transform.position = pos;
            leftColl.isTrigger = isCanTrigger;
        }
        if (info != null)
        {
            player.transform.position = info.pos;
            leftColl.isTrigger = info.leftcollIsTrigger;
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Load();
        } 
    }
    /// <summary>
    /// 添加需要存储的数据
    /// </summary>
    public void Save()
    {
        SaveGameData curInfo = SaveDataMgr.Instance.save;
        curInfo.pos = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);
        curInfo.leftcollIsTrigger = leftColl.isTrigger;
        SaveDataMgr.Instance.SaveData();
    }
    public void Load()
    {
        info = SaveDataMgr.Instance.LoadData();
        Init(info);
    }
}
