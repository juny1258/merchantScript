using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataChangeEvent : MonoBehaviour
{
    private static DataChangeEvent instance;

    public static DataChangeEvent Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataChangeEvent>();
                if (instance == null)
                {
                    var container = new GameObject("DataChangeEvent");
                    instance = container.AddComponent<DataChangeEvent>();
                }
            }

            return instance;
        }
    }

    public delegate void Event();

    public delegate void EventWithInt(int index);

    public delegate void EventWithFloat(float index);

    // 스크린을 눌렀을 때 이벤트
    public static event EventWithInt TabScreen;

    public static event Event ResetPosition;

    public static event EventWithFloat TradeCompeleteEvent;

    public static event EventWithInt CostumeChange;
    
    public static event EventWithInt RidingChange;

    public static event EventWithInt PetChange;

    public static event EventWithInt StartTradeEvent;

    public static event Event TownChangeEvent;

    public static event EventWithInt SkillStartEvent;

    public static event Event PurchaseCostumeEvent;
    public static event Event PurchaseRidingEvent;
    public static event Event PurchasePetEvent1;
    public static event Event PurchaseTownEvent;
    public static event Event PurchaseBackgroundEvent;

    public static event Event PurchasePakageEvent;

    public static event Event ReinforceEvent;

    public static event Event OpenMenuEvent;

    public static event Event ChangeSkinEvent;

    public static event Event ResetDataEvent;

    public static event Event PvpTabEvent;

    public static event Event SyncPvpDataEvent;

    public void SyncPvpData()
    {
        if (SyncPvpDataEvent != null)
        {
            SyncPvpDataEvent();
        }
    }

    public void PvpTab()
    {
        if (PvpTabEvent != null)
        {
            PvpTabEvent();
        }
    }

    public void Tab(int index)
    {
        if (TabScreen != null)
        {
            TabScreen(index);
        }
    }

    public void ResetPositions()
    {
        if (ResetPosition != null)
        {
            ResetPosition();
        }
    }

    public void CostumeDataChanged(int index)
    {
        if (CostumeChange != null)
        {
            CostumeChange(index);
        }
    }
    
    public void RidingDataChanged(int index)
    {
        if (RidingChange != null)
        {
            RidingChange(index);
        }
    }

    public void PetDataChanged(int index)
    {
        if (PetChange != null)
        {
            PetChange(index);
        }
    }

    public void StartTrade(int index)
    {
        if (StartTradeEvent != null)
        {
            StartTradeEvent(index);
        }
    }

    public void TradeComplete(float gold)
    {
        if (TradeCompeleteEvent != null)
        {
            TradeCompeleteEvent(gold);
        }
    }

    public void TownChange()
    {
        if (TownChangeEvent != null)
        {
            TownChangeEvent();
        }
    }
    
    public void PurchaseCostume()
    {
        if (PurchaseCostumeEvent != null)
        {
            PurchaseCostumeEvent();
        }   
    }
    public void PurchaseRiding()
    {
        if (PurchaseRidingEvent != null)
        {
            PurchaseRidingEvent();
        }   
    }
    public void PurchasePet1()
    {
        if (PurchasePetEvent1 != null)
        {
            PurchasePetEvent1();
        }   
    }
    public void PurchaseTown()
    {
        if (PurchaseTownEvent != null)
        {
            PurchaseTownEvent();
        }   
    }
    
    public void PurchaseBackground()
    {
        if (PurchaseBackgroundEvent != null)
        {
            PurchaseBackgroundEvent();
        }   
    }

    public void SkillStarted(int i)
    {
        if (SkillStartEvent != null)
        {
            SkillStartEvent(i);
        }
    }

    public void PurchasePakage()
    {
        if (PurchasePakageEvent != null)
        {
            PurchasePakageEvent();
        }
    }

    public void Reinforce()
    {
        if (ReinforceEvent != null)
        {
            ReinforceEvent();
        }
    }

    public void OpenMenu()
    {
        if (OpenMenuEvent != null)
        {
            OpenMenuEvent();
        }
    }

    public void ChangeSkin()
    {
        if (ChangeSkinEvent != null)
        {
            ChangeSkinEvent();
        }
    }

    public void ResetData()
    {
        if (ResetDataEvent != null)
        {
            ResetDataEvent();
        }
    }
}