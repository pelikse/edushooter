using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class CreditContentInjecter : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public List<CreditSection> Credits;
    public Transform CreditsParent;

    [Space]

    public GameObject CreditSectionPrefab;
    public GameObject CreditRowPrefab;
    public GameObject CreditSpacerPrefab;

    //cache
    private CreditSectionTitle _title;
    private CreditSectionRow _row;

    public void InjectCredits()
    {
        foreach (var creditSection in Credits)
        {
            //inject title
            _title = Instantiate(CreditSectionPrefab, CreditsParent).GetComponent<CreditSectionTitle>();
            _title.Init(creditSection.SectionTitle);

            //inject rows of attribution
            foreach (var rows in creditSection.Attributions)
            {
                _row = Instantiate(CreditRowPrefab, CreditsParent).GetComponent<CreditSectionRow>();
                _row.Init(rows.Role, rows.Name);
            }

            //inject spacer
            Instantiate(CreditSpacerPrefab, CreditsParent);
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
        switch (gameEvent.EventName)
        {
            case "SwitchedPanel":
                InjectCredits();
                break;
        }
    }
    #endregion
}
