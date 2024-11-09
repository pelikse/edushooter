using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionUIManager : MMSingleton<ConditionUIManager>
{
    public List<ConditionTracker> Trackers = new List<ConditionTracker>();

    [Space]

    public GameObject TrackerPrefab;
    public Transform TrackerParent;

    private void Start()
    {
        GetConditionTrackers();

        foreach (var tracker in Trackers)
        {
            tracker.gameObject.SetActive(false);
        }
    }

    public void TrackNewCondition(PowerupCondition condition, float duration)
    {
        //activate an inactive tracker and set it
        foreach (var tracker in Trackers)
        {
            if (!tracker.gameObject.activeSelf)
            {
                Debug.Log("adding new condition " + duration + " " + condition.ConditionColor);
                tracker.InitializeCondition(duration, condition.ConditionColor, condition.ConditionSprite);
                tracker.gameObject.SetActive(true);
                return;
            }
        }
        //else add a new tracker and set it

        if (TrackerParent != null)
        {
            GameObject newTracker = Instantiate(TrackerPrefab, TrackerParent);

            if (newTracker.TryGetComponent(out ConditionTracker tracker))
            {
                tracker.InitializeCondition(duration, condition.ConditionColor, condition.ConditionSprite);
                tracker.gameObject.SetActive(true);
                return;
            }
        }
    }

    //grabs all children with ConditionTracker component
    private void GetConditionTrackers()
    {
        Trackers.Clear();

        foreach (Transform child in transform)
        {
            ConditionTracker tracker = child.GetComponent<ConditionTracker>();
            if (tracker != null && !Trackers.Contains(tracker))
            {
                Trackers.Add(tracker);
            }
        }
    }
}
