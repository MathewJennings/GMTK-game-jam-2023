using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputManager : MonoBehaviour
{
    private List<GameObject> clickedGameObjectsStack = new List<GameObject>();

    public void OnClick()
    {
        UpdateClickedGameObjectsStack();
    }

    public List<GameObject> GetClickedGameObjects()
    {
        return clickedGameObjectsStack;
    }

    private void UpdateClickedGameObjectsStack()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        clickedGameObjectsStack.Clear();
        results.ForEach(result => clickedGameObjectsStack.Add(result.gameObject));
    }
}
