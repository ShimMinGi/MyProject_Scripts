using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class JobInfo
{
    /// <summary>
    /// 키 값
    /// </summary>
    public int key;

    /// <summary>
    /// 직업 이름
    /// </summary>
    public string jobName;

    /// <summary>
    /// 직업 타입
    /// </summary>
    public DesignEnums.JobType jobType;

    /// <summary>
    /// 체력 값
    /// </summary>
    public int hp;

    /// <summary>
    /// 마나 값
    /// </summary>
    public int mp;

    /// <summary>
    /// 직업 설명
    /// </summary>
    public string jobDesc;

}
public class JobInfoLoader
{
    public List<JobInfo> ItemsList { get; private set; }
    public Dictionary<int, JobInfo> ItemsDict { get; private set; }
    public JobInfoLoader(string path = "JSON/JobInfo")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, JobInfo>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<JobInfo> Items;
    }

    public JobInfo GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public JobInfo GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
