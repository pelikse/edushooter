using Firebase.Firestore;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [System.Serializable]
    public class LeaderboardResponse
    {
        public LeaderboardEntryData[] leaderboard;
    }

    [System.Serializable]
    public class LeaderboardEntryData
    {
        public string PlayerID;
        public string PlayerName;
        public int DifficultyLevel;
        public FirebaseHandler.FirebaseTimestamp FinishTime;
    }

    [SerializeField] private GameObject LoadingIndicator;
    [SerializeField] private GameObject ErrorIndicator;
    [SerializeField] private GameObject NoDataIndicator;
    [Space]
    [SerializeField] private LeaderboardResponse LeaderboardData;

    [Space, Space]

    [SerializeField] private MapType InitialMap;

    [Space]

    public GameObject EntryPrefab;
    public Transform LeaderboardContents;

    [Space]

    public PlayerLeaderboardEntry PlayerEntry;

    // constants
    const string LEADERBOARD_URL = "https://getleaderboard-tfxf5ks7wa-de.a.run.app/";
    const string BASE_MAP_LEADERBOARD_URL = "https://getleaderboardfrommap-tfxf5ks7wa-de.a.run.app?mapType=";

    const int TIMEOUT = 8;
    const int MAX_ENTRIES = 10;

    // storage
    private MapType CurrentMap = MapType.Neighborhood;

    public void _Initialize()
    {
        //get the initial map type
        CurrentMap = InitialMap;

        FetchData();
    }

    public void ChangeLeaderboardMap(MapType map)
    {
        CurrentMap = map;
        FetchData();
    }

    private void FetchData()
    {
        PlayerEntry.SetEntryValue(CurrentMap);

        Debug.Log("fetching data, current map is " + CurrentMap.ToString());
        //destroy all previous entries
        foreach (Transform child in LeaderboardContents)
        {
            Destroy(child.gameObject);
        }

        //display loading indicator
        LoadingIndicator.SetActive(true);
        ErrorIndicator.SetActive(false);
        NoDataIndicator.SetActive(false);

        //try and populate the leaderboard
        PopulateLeaderboard();
    }

    private void PopulateLeaderboard()
    {
        Debug.Log("getting leaderboard data...");
        StartCoroutine(GetLeaderboardData());
    }

    private IEnumerator GetLeaderboardData()
    {
        // make a http web request to the leaderboard API using the base URL
        UnityWebRequest request = UnityWebRequest.Get(BASE_MAP_LEADERBOARD_URL + CurrentMap.ToString());
        request.timeout = TIMEOUT;

        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            LoadingIndicator.SetActive(false);

            if (request.responseCode.Equals(404))
            {
                Debug.Log("didnt find any data, display to user!");
                NoDataIndicator.SetActive(true);
            }
            else
            {
                ErrorIndicator.SetActive(true);
            }
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Successfully fetched leaderboard data!");

            string jsonResponse = request.downloadHandler.text;

            LeaderboardData = JsonUtility.FromJson<LeaderboardResponse>(jsonResponse);

            InsertLeaderboardEntry(LeaderboardData);

            LoadingIndicator?.SetActive(false);
        }
        else
        {
            Debug.LogError("Request timed out");
            LoadingIndicator.SetActive(false);
            ErrorIndicator.SetActive(true);
        }
    }

    private void InsertLeaderboardEntry(LeaderboardResponse response)
    {
        LeaderboardEntry entry_init;
        int i = 1;

        foreach (LeaderboardEntryData data in response.leaderboard)
        {
            if (i > MAX_ENTRIES) break;

            // loop logic
            // instantiate the leaderboard entry as a child
            GameObject entry = Instantiate(EntryPrefab, LeaderboardContents);

            if (entry.TryGetComponent(out entry_init))
            {
                string username;
                if (data.PlayerName != null)
                {
                    username = data.PlayerName;
                }
                else
                {
                    username = data.PlayerID;
                }
                entry_init.SetEntry(username, data.DifficultyLevel, i, data.FinishTime._seconds);
            }

            i++;
        }

    }

    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        if (gameEvent.EventName.Equals("SwitchedPanel"))
        {
            _Initialize();
        }
    }
    #endregion
}
