using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Icons;


public class ProfilePopup
{
    #region Fields
    private const string menuPath = GameController.menusPath + "profilePopup";

    private Button profileCloseButton;
    private Button profileBackgroundClose;
    private GameObject profilePopUp;
    private RectTransform profilePopUpRect;
    private Dictionary<Languages, Toggle> languagesToggles = new Dictionary<Languages, Toggle>();
    private Languages currentLanguage = Languages.English;

    public bool profileMenuIsOpen;

    private float animationDuration = 0.3f;
    #endregion

    #region Initialize
    public void Initialize()
    {

        profilePopUp = GameObject.Find(menuPath);
        profilePopUpRect = GameObject.Find(menuPath + "/bg").GetComponent<RectTransform>();

        profileCloseButton = GameObject.Find(menuPath + "/bg/closeButton").GetComponent<Button>();
        profileCloseButton.onClick.AddListener(() => {
            ClosePopup();
        });

        profileBackgroundClose = GameObject.Find(menuPath + "/background").GetComponent<Button>();
        profileBackgroundClose.onClick.AddListener(() => {
            ClosePopup();
        });

        var toggleEnglish = GameObject.Find(menuPath + "/bg/language/english").GetComponent<Toggle>();
        var toggleRussian = GameObject.Find(menuPath + "/bg/language/russian").GetComponent<Toggle>();

        languagesToggles.Add(Languages.English, toggleEnglish);
        languagesToggles.Add(Languages.Russian, toggleRussian);

        languagesToggles[currentLanguage].isOn = true;

        toggleEnglish.onValueChanged.AddListener((bool isOn) => { if (isOn) SendLanguage(Languages.English); });
        toggleRussian.onValueChanged.AddListener((bool isOn) => { if (isOn) SendLanguage(Languages.Russian); });


        var imageClickedAchievementPercentage = GameObject.Find(menuPath + "/bg/clickedAchievement/progressbarBack/mask").GetComponent<Image>();
        var imageUpgradesAchievementPercentage = GameObject.Find(menuPath + "/bg/upgradesAchievement/progressbarBack/mask").GetComponent<Image>();
        var textClickedAchievementPercentage = GameObject.Find(menuPath + "/bg/clickedAchievement/progressbarBack/count").GetComponent<TextMeshProUGUI>();
        var textUpgradesAchievementPercentage = GameObject.Find(menuPath + "/bg/upgradesAchievement/progressbarBack/count").GetComponent<TextMeshProUGUI>();
        var textClickedAchievementBonus = GameObject.Find(menuPath + "/bg/clickedAchievement/quantity").GetComponent<TextMeshProUGUI>();
        var textUpgradesAchievementBonus = GameObject.Find(menuPath + "/bg/upgradesAchievement/quantity").GetComponent<TextMeshProUGUI>();

        int clickedAchievementValue = GameEntities.Achievements.achievementsMade[Achievement.BananasClicked];
        int upgradesAchievementValue = GameEntities.Achievements.achievementsMade[Achievement.BananaUpgrades];

        int nextClickedAchievementValue = Achievements.achievementMilestones[Achievement.BananasClicked].FirstOrDefault(x => x > clickedAchievementValue);
        int nextUpgradesAchievementValue = Achievements.achievementMilestones[Achievement.BananaUpgrades].FirstOrDefault(x => x > clickedAchievementValue);

        int clickedAchievementBonus = Achievements.achievementMilestones[Achievement.BananasClicked].IndexOf(nextClickedAchievementValue);
        int upgradesAchievementBonus = Achievements.achievementMilestones[Achievement.BananaUpgrades].IndexOf(nextUpgradesAchievementValue);

        imageClickedAchievementPercentage.fillAmount = (float)clickedAchievementValue / nextClickedAchievementValue;
        imageUpgradesAchievementPercentage.fillAmount = (float)upgradesAchievementValue / nextUpgradesAchievementValue;

        textClickedAchievementPercentage.text = $"{clickedAchievementValue} / {nextClickedAchievementValue}";
        textUpgradesAchievementPercentage.text = $"{upgradesAchievementValue} / {nextUpgradesAchievementValue}";

        textClickedAchievementBonus.text = $"+{clickedAchievementBonus}";
        textUpgradesAchievementBonus.text = $"+{upgradesAchievementBonus}";
    }
    #endregion

    public void OpenPopup()
    {
        GameEntities.GameController.StartCoroutine(GameEntities.GameController.InitializeMenu(MenuName.ProfilePopup, Initialize, OpenPopupActions));
    }

    private void OpenPopupActions()
    {

        if (!profileMenuIsOpen)
        {
            GameEntities.GameController.StartCoroutine(TogglePopupWithZoomAnimation(true, profilePopUpRect, Vector3.zero, Vector3.one));
            profilePopUp.transform.SetAsLastSibling();
            profileMenuIsOpen = true;
        }
    }

    public void ClosePopup()
    {

        if (profileMenuIsOpen)
        {
            GameEntities.GameController.StartCoroutine(TogglePopupWithZoomAnimation(false, profilePopUpRect, Vector3.one, Vector3.zero));
            profileMenuIsOpen = false;
        }
    }

    private IEnumerator TogglePopupWithZoomAnimation(bool Open, RectTransform PopupRect, Vector3 StartScale, Vector3 FinalScale)
    {

        PopupRect.localScale = StartScale;

        if (Open)
        {
            PopupRect.DOKill(true);
            PopupRect.parent.gameObject.SetActive(true);
        }

        yield return new WaitForEndOfFrame();

        PopupRect.DOScale(FinalScale, animationDuration).OnComplete(() => { if (!Open) PopupRect.parent.gameObject.SetActive(false); });
    }

    private void Toggle(Toggle.ToggleEvent t)
    {

    }

    public void SendLanguage(Languages language)
    {
        GameEntities.SocketConnection.ChangeLanguage((int)language);
    }

    public void SetLanguage(int language)
    {
        currentLanguage = (Languages)language;
    }
}
