using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonColorToggle : MonoBehaviour
{
    public List<Button> buttons;
    public Color activeColor = Color.blue;
    public Color defaultColor = Color.white;

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClicked(btn));
        }

        ResetButtonColors();
        if (buttons.Count > 0) ChangeButtonColor(buttons[0]);
    }

    void OnButtonClicked(Button clickedButton)
    {
        ResetButtonColors();
        ChangeButtonColor(clickedButton);
    }

    void ChangeButtonColor(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = activeColor;
        colors.selectedColor = activeColor;
        button.colors = colors;
    }

    void ResetButtonColors()
    {
        foreach (Button btn in buttons)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = defaultColor;
            colors.selectedColor = defaultColor;
            btn.colors = colors;
        }
    }
}
