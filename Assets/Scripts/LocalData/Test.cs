using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        var localDataMgr = LocalDataManager.Instance;
        localDataMgr.LoadAll();
        var data = localDataMgr.Status;
        var data1 = data[1];
        var dataList = data.DataList;
    }
}
