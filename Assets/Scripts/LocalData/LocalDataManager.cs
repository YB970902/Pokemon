using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum StatusType
{
    None,
    Data,
}

public class LocalDataManager : Singleton<LocalDataManager>
{
    private const string RelativeLocalDataFolder = "/Resources/LocalData";
    private readonly string AbsoluteLocalDataFolder = Application.dataPath + RelativeLocalDataFolder;

    public LocalDataList<LDStatus> Status { get; private set; }

    public LocalDataManager()
    {
        Status = new LocalDataList<LDStatus>();
    }
    
    /// <summary>
    /// 로컬 데이터 폴더 하위에 있는 모든 데이터를 로드한다.
    /// </summary>
    public void LoadAll()
    {
        if (Directory.Exists(AbsoluteLocalDataFolder) == false)
        {
            Directory.CreateDirectory(AbsoluteLocalDataFolder);
        }
        
        var localDataDirectory = new DirectoryInfo(AbsoluteLocalDataFolder);

        // 로컬 경로 내에 있는 모든 폴더들 가져오기
        var directories = localDataDirectory.GetDirectories();

        foreach (var directory in directories)
        {
            Load(directory);
        }
    }

    /// <summary>
    /// 로컬 데이터를 로드한다
    /// </summary>
    private void Load(DirectoryInfo _directoryInfo)
    {
        // 폴더 이름
        var folderName = _directoryInfo.Name;
        // 파싱할 대상의 클래스 타입
        var classType = Type.GetType($"LD{folderName}");
        // 파싱한 데이터를 저장할 타깃 프로퍼티 
        var targetProperty = GetType().GetProperty(folderName);
        // 타깃 프로퍼티에 값을 세팅할 함수
        var targetInitMethod = targetProperty.GetValue(this).GetType().GetMethod("Init");
        
        var files = _directoryInfo.GetFiles();
        FileInfo csvFileInfo = null;
        
        // 폴더 내에서 csv 파일을 찾아낸다
        foreach (var file in files)
        {
            if (IsCsv(file))
            {
                csvFileInfo = file;
                break;
            }
        }

        // 폴더 내에 파일이 없는경우
        if (csvFileInfo == null)
        {
            Debug.LogError($"파일이 존재하지 않습니다. {folderName}");
            return;
        }

        // csv파일을 라인 별로 나눠 저장한다
        StreamReader sr = new StreamReader(csvFileInfo.FullName);
        var lines = sr.ReadToEnd().Split(Environment.NewLine);
        sr.Close();

        // 첫 번째 라인에는 어떤 프로퍼티에 저장할지 이름이 명시되어있다.
        var propertyNames = lines[0].Split(",");
        List<PropertyInfo> propertyTypes = GetPropertyTypeList(classType, lines[0]);

        // 파싱으로 생성될 모든 데이터가 저장될 리스트
        var dataList = Activator.CreateInstance(typeof(List<>).MakeGenericType(classType));
        // 모든 데이터가 저장될 리스트에 추가할 함수
        var dataListAddMethod = dataList.GetType().GetMethod("Add");

        // 두 번째 라인부터 파싱을 시작한다.
        for (int i = 1; i < lines.Length; ++i)
        {
            // 데이터 객체를 생성한다.
            var newData = Activator.CreateInstance(classType);
            
            var lineList = lines[i].Split(",");
            for(int j = 0; j < lineList.Length; ++j)
            {
                var currentType = propertyTypes[j].PropertyType;
                var currentProperty = newData.GetType().GetProperty(propertyNames[j]);
                
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    // 리스트인 경우
                    
                    // 리스트 객체를 생성한다.
                    var newList = Activator.CreateInstance(currentProperty.PropertyType);
                    var genericType = currentType.GetGenericArguments()[0];
                    
                    // 데이터를 |를 기준으로 나눈다. 콤마는 이미 사용중이므로 가장 덜 사용될만한 문자를 선정한다.
                    var listData = lineList[j].Split("|");
                    // 리스트에 추가하는 함수
                    var getMethod = newList.GetType().GetMethod("Add");
                    
                    foreach (var data in listData)
                    {
                        getMethod.Invoke(newList, new object[] { Convert.ChangeType(data, genericType) });
                    }
                    
                    // 프로퍼티에 리스트를 넣어준다.
                    currentProperty.SetValue(newData, newList);
                }
                else if (currentType.IsEnum)
                {
                    // 열거형인 경우
                    currentProperty.SetValue(newData, Enum.Parse(currentType, lineList[j]));
                }
                else
                {
                    // 그 외 타입인경우
                    currentProperty.SetValue(newData, Convert.ChangeType(lineList[j], currentType));
                }
            }

            // 파싱된 데이터를 집어넣는다.
            dataListAddMethod.Invoke(dataList, new object[] { newData });
        }
        
        // 파싱한 모든 데이터를 최종 리스트에 넣는다.
        targetInitMethod.Invoke(targetProperty.GetValue(this), new object[] { dataList });
    }

    private bool IsCsv(FileInfo _fileInfo)
    {
        return _fileInfo.Extension == ".csv";
    }

    /// <summary>
    /// 문자열을 읽고 해당하는 프로퍼티 정보를 리스트에 담아 반환한다.
    /// </summary>
    private List<PropertyInfo> GetPropertyTypeList(Type _classType, string _firstLine)
    {
        var firstLineNames = _firstLine.Split(",");
        List<PropertyInfo> result = new List<PropertyInfo>(firstLineNames.Length);

        foreach (var name in firstLineNames)
        {
            var propertyType = _classType.GetProperty(name);
            if (propertyType == null)
            {
                Debug.LogError("해당 타입은 존재하지 않습니다.");
                return null;
            }
            result.Add(propertyType);
        }
        
        return result;
    }
}
