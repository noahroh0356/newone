using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveMgr
{
    //SaveFile() fName로 저장하기 + T 타입의 데이터를 파일 이름
    public static void SaveData<T>(string fName, T data)
    {

#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, fName);
#else
        string path = Path.Combine(Application.persistentDataPath, fName);
#endif
        Debug.Log($"데이터 로드 경로 : {path}");
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        string ToJsonData = JsonUtility.ToJson(data, true);
        Debug.Log(ToJsonData);
        File.WriteAllText(path, ToJsonData);
    }

    // LoadFile 데이터 로드하기
    public static T LoadData<T>(string fName)
    {
        T data = default;
#if UNITY_EDITOR //유니티 에디터에서만 동작되는 코드
        string path = Path.Combine(Application.dataPath, fName);
#else
				//에디터가 아닌 경우 
        string path = Path.Combine(Application.persistentDataPath, fName);
#endif
        Debug.Log($"데이터 세이브 경로 : {path}");

        if (File.Exists(path))
        {
            string FromJsonData = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(FromJsonData);
        }

        return data;
    }
}