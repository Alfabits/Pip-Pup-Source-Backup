using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIListPopulator {

    private const float BUTTON_OFFSET = 185.0f;
    private const float CONTAINER_OFFSET = 185.0f;
    private const float INITIAL_CONTAINER_OFFSET = 370.0f;

	public static List<GameObject> InsertButtonIntoList(GameObject a_ParentObject, List<GameObject> a_ButtonsList, GameObject a_ButtonToAdd)
    {
        GameObject tempObj = Object.Instantiate(a_ButtonToAdd, a_ParentObject.transform.position, Quaternion.identity);
        tempObj.transform.SetParent(a_ParentObject.transform);
        tempObj.transform.localScale = a_ParentObject.transform.localScale;

        a_ParentObject.GetComponent<RectTransform>().offsetMin = AdjustListSizeBottom(a_ParentObject.GetComponent<RectTransform>(), a_ButtonsList);

        tempObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        tempObj.GetComponent<RectTransform>().localPosition = DetermineNextButtonPosition(a_ParentObject.GetComponent<RectTransform>(), a_ButtonsList);

        a_ButtonsList.Add(tempObj);

        return a_ButtonsList;
    }

    /// <summary>
    /// Returns the local position of where the next button would go.
    /// </summary>
    /// <param name="a_ListSize"></param>
    /// <param name="a_ButtonsList"></param>
    /// <returns></returns>
    static Vector2 DetermineNextButtonPosition(RectTransform a_ListSize, List<GameObject> a_ButtonsList)
    {
        float posX = a_ListSize.position.x;
        float posY = a_ListSize.position.y + ((a_ListSize.rect.height / 2) - BUTTON_OFFSET);


        foreach (GameObject button in a_ButtonsList)
        {
            posY -= BUTTON_OFFSET;
        }

        return new Vector2(posX, posY);
    }

    static Vector2 AdjustListSizeBottom(RectTransform a_ListSize, List<GameObject> a_ButtonsList)
    {
        float newBottomPosition = a_ListSize.offsetMax.y - INITIAL_CONTAINER_OFFSET;

        foreach (GameObject button in a_ButtonsList)
        {
            newBottomPosition -= CONTAINER_OFFSET;
        }

        return new Vector2(a_ListSize.offsetMin.x, newBottomPosition);
    }

}
