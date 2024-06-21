using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillTooltip;

    void Start()
    {
        SwitchTo(null);
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
            return;
        }

        SwitchTo(_menu);
    }
}
