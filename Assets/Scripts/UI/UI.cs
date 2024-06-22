using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillTooltip;

    private void Awake()
    {
        SwitchTo(skillTreeUI); //we need this to assign event on skill tree slots before we assign event on skill scripts
    }

    void Start()
    {
        SwitchTo(null);
        SwitchTo(inGameUI);
        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWitchKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWitchKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWitchKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWitchKeyTo(optionsUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchTo(null);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);
    }

    public void SwitchWitchKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(inGameUI);
    }
}
