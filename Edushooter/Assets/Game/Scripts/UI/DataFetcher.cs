using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface that enforces data fetching for the UI
public interface DataFetcher
{
    public abstract void FetchData(PlayerExternalData data);
}
