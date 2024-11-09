using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeaderboardTab : MonoBehaviour
{
    public LeaderboardManager Leaderboard;
    public MapType Map;

    public void ChangeTab()
    {
        Leaderboard.ChangeLeaderboardMap(Map);
    }
}
