using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataInstaller : MonoBehaviour
{
    [SerializeField] private bool _fromTheBeginning;
    [SerializeField] private ConfigData _allConfigData;
    private bool _showTerms = true;

    private void Start()
    {
        InstallBindings();
    }

    private void InstallBindings()
    {
        BindFileNames();
        BindRegistration();
        BindSettings();
        BindIdeas();
        BindCheckList();
        BindPassword();
        StartLoading();
    }

    private void StartLoading()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");

        if (HtmlText != "")
        {
            LoadFirebaseConfig();
        }

        else
        {
            LoadScene();
        }
    }

    public void LoadFirebaseConfig()
    {
        CheckRemoteConfigValues();
    }


    public Task CheckRemoteConfigValues()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            LoadScene();
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            LoadScene();
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                foreach (var item in remoteConfig.AllValues)
                {
                    switch (item.Key)
                    {
                        case "url":
                            {
                                _allConfigData.Url = item.Value.StringValue;
                                break;
                            }
                        case "showAgree":
                            {
                                _allConfigData.ShowAgree = item.Value.BooleanValue;
                                break;
                            }
                    }
                }

                _showTerms = _allConfigData.ShowAgree;
                Debug.Log(_showTerms + "/" + _allConfigData.ShowAgree);
                var reg = SaveSystem.LoadData<RegistrationSaveData>();
                reg.Link = _allConfigData.Url;
                SaveSystem.SaveData(reg);
                LoadScene();
            });
        
    }

    private void LoadScene()
    {
        if (_showTerms)
        {
            if (PlayerPrefs.HasKey("Onboarding"))
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                SceneManager.LoadScene("Onboarding");
            }
        }
        else
        {
            SceneManager.LoadScene("PasswordScene");
        }
        
    }

    private void BindRegistration()
    {
        {
            var reg = SaveSystem.LoadData<RegistrationSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                reg = null;
            }
#endif

            if (reg == null)
            {
                reg = new RegistrationSaveData("", false);
                SaveSystem.SaveData(reg);
            }

        }
    }

    private void BindSettings()
    {
        {
            var settings = SaveSystem.LoadData<SettingSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                settings = null;
            }
#endif

            if (settings == null)
            {
                settings = new SettingSaveData(false, "");
                SaveSystem.SaveData(settings);
            }

        }
    }

    private void BindIdeas()
    {
        {
            var ideas = SaveSystem.LoadData<IdeasSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                ideas = null;
            }
#endif

            if (ideas == null)
            {
                List<IdeasData> ideasDatas = new List<IdeasData>();
                ideas = new IdeasSaveData(ideasDatas);
                SaveSystem.SaveData(ideas);
            }

        }
    }

    private void BindCheckList()
    {
        {
            var checkList = SaveSystem.LoadData<CheckListSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                checkList = null;
            }
#endif

            if (checkList == null)
            {
                List<CheckListData> checkListDatas = new List<CheckListData>();
                checkList = new CheckListSaveData(checkListDatas);
                SaveSystem.SaveData(checkList);
            }

        }
    }

    private void BindPassword()
    {
        {
            var password = SaveSystem.LoadData<PasswordSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                password = null;
            }
#endif

            if (password == null)
            {
                List<PasswordData> passwordDatas = new List<PasswordData>();
                password = new PasswordSaveData(passwordDatas);
                SaveSystem.SaveData(password);
            }

        }
    }

    private void BindFileNames()
    {
        FileNamesContainer.Add(typeof(RegistrationSaveData), FileNames.RegData);
        FileNamesContainer.Add(typeof(SettingSaveData), FileNames.SettingsData);
        FileNamesContainer.Add(typeof(IdeasSaveData), FileNames.IdeasData);
        FileNamesContainer.Add(typeof(CheckListSaveData), FileNames.CheckListData);
        FileNamesContainer.Add(typeof(PasswordSaveData), FileNames.PasswordData);
    }

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

}

[Serializable]
public class ConfigData
{
    public string Url;
    public bool ShowAgree;
}