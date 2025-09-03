using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] tabContents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActiveTab(0);
    }

    public void ActiveTab(int tabNumber)
    {
        for (int i = 0; i < tabImages.Length; i++)
        {
            if (i == tabNumber)
            {
                tabImages[i].color = Color.gray;
                tabContents[i].SetActive(true);
            }
            else
            {
                tabImages[i].color = Color.white;
                tabContents[i].SetActive(false);
            }
        }
    }
}
