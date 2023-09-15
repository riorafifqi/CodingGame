using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] Image previewImage;
    [SerializeField] GameObject previewParent3d;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectedImage;
    private SkinSelector[] siblings;
    private Button iconButton;
    private bool isSelected = false;

    private void Start()
    {
        siblings = transform.parent.GetComponentsInChildren<SkinSelector>();
        iconImage.sprite = character.icon;
        iconButton = GetComponent<Button>();

        isSelected = false;
        if (PlayerPrefs.GetInt("SelectedSkin") == character.ID)
        {
            //previewImage.sprite = character.preview;
            SetPreview3DObject(character.modelUIPrefab);

            isSelected = true;
        }

        iconButton.onClick.AddListener(OnClickIcon);
    }

    private void Update()
    {
        if (isSelected)
        {
            selectedImage.SetActive(true);
        } 
        else 
        {
            selectedImage.SetActive(false);
        }
    }

    public void OnClickIcon()
    {
        foreach (var sibling in siblings)
        {
            sibling.SetSelectedStatus(false);
        }

        //previewImage.sprite = character.preview;
        SetPreview3DObject(character.modelUIPrefab);
        this.isSelected = true;
    }

    public void SetSelectedStatus(bool cond)
    {
        isSelected = cond;
    }

    public bool GetSelectedStatus()
    {
        return this.isSelected;
    }

    public Character GetCharacter()
    {
        return character;
    }

    private void SetPreview3DObject(GameObject target)
    {
        Destroy(previewParent3d.transform.GetChild(0).gameObject);      // destroy child
        Instantiate(target, previewParent3d.transform);
    }
}
