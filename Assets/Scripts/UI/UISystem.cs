using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//All the UI that is not dependant on player status is managed here

public class UISystem : MonoBehaviour
{
    public static UISystem current;
    [SerializeField]
    Transform worldCanvas;
    [SerializeField]
    Transform overlayCanvas;
    [SerializeField]
    GameObject endDoorMenu;

    [SerializeField]
    GameObject damagePopup;
    [SerializeField]
    GameObject announcerPrefab;
    GameObject currentAnnouncer;
    [SerializeField]
    float announcerYOffset;
    [SerializeField]
    float damageTextOffset;
    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        GameEvents.current.onEndDoorEnter += OnEndDoorTriggerEnter;
        GameEvents.current.onPlayerEncounteredInteractor += AnnounceInteraction;
        GameEvents.current.onPlayerLeftInteractor += DeleteInteraction;
        GameEvents.current.onPlayerInstantiated.AddListener(ShowLevel);
        GameEvents.current.onEndDoorEnter += ShowEndDoorMenu;
        GameEvents.current.onLevelEnded += CleanCanvases;
        GameEvents.current.onEnemyTookDamage += OnEnemyTookDamage;
    }
    void OnEndDoorTriggerEnter()
    {
        
    }
    void AnnounceInteraction(InteractionEventArgs args)
    {
        if(args.InteractionType != InteractionType.ClimbLadder)
        {
            if (currentAnnouncer != null)
            {
                Destroy(currentAnnouncer);
            }
            currentAnnouncer = Instantiate(announcerPrefab, args.InteractorPosition + Vector2.up * announcerYOffset, Quaternion.identity);
            currentAnnouncer.transform.SetParent(worldCanvas);
            currentAnnouncer.GetComponentInChildren<Text>().text = "Press " + args.Interactor.InteractionKey().ToString() + " to interact with " + args.Interactor.Name();
        }
        
    }
    void DeleteInteraction()
    {
        if (currentAnnouncer != null)
        {
            Destroy(currentAnnouncer);
        }
    }
    void ShowLevel()
    {
        currentAnnouncer = Instantiate(announcerPrefab, PlayerIdentity.instance.transform.position, Quaternion.identity);
        currentAnnouncer.transform.SetParent(worldCanvas);
        currentAnnouncer.GetComponentInChildren<Text>().text = "Level " + GameData.current.Level;
    }
    void ShowEndDoorMenu()
    {
        endDoorMenu.SetActive(true);
    }
    void CleanCanvases(bool nextLevel)
    {
        CustomUtilities.Clear(worldCanvas, Destroy);
        endDoorMenu.SetActive(false);
    }
    public void OnButtonClicked(string button)
    {
        GameEvents.current.UIButtonClicked(button);
    }
    public void OnEnemyTookDamage(GameObject enemy, float damage)
    {
        GameObject popup = Instantiate(damagePopup, (Vector2)enemy.transform.position + Vector2.up * damageTextOffset + CustomUtilities.RandomVector2(1,0), Quaternion.identity);
        popup.transform.SetParent(worldCanvas);
        popup.GetComponent<TMPro.TMP_Text>().text = damage.ToString();

    }
}
