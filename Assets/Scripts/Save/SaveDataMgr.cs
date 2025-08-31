using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveGameData
{
    public Vector3 pos;
    //需要存储的主角状态
    //当前地图需要的参数信息
    public bool leftcollIsTrigger;


}
/// <summary>
/// 存档管理器
/// </summary>

public class SaveDataMgr : SingletonMono<SaveDataMgr>
{
    public SaveGameData save;
    public void SaveData()
    {
        //传入所需要保存的信息
        //重点为存储关卡状态
        //添加存档键，通过物体的名称进行存储
        var item = JsonUtility.ToJson(save);
        var path = Path.Combine(Application.persistentDataPath, "save.qzg");
        File.WriteAllText(path, item);
        Debug.Log("保存成功");

    }
    public SaveGameData LoadData()
    {
        //读取场景名称
        //根据名称、存储的信息的键值对进行读取
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.qzg"));
        SaveGameData item = JsonUtility.FromJson<SaveGameData>(json);
        return item;
    }
}
