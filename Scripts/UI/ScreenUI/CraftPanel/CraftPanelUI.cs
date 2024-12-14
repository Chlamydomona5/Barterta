using System.Linq;
using Barterta.Craft;
using Barterta.ItemGrid;
using Barterta.UI.ScreenUI;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CraftPanelUI : UIObject
{
    [SerializeField] private CraftTableUI craftTableUI;
    [SerializeField] private KilnRecipeUI kilnRecipeUI;
    [SerializeField] private GridLayoutGroup resultGridParent;
    [SerializeField] private Transform categoryButtonParent;

    [SerializeField] private CraftResultButton craftResultButtonPrefab;
    [SerializeField] private KilnResultButton kilnResultButtonPrefab;
    
    private CraftValidater _craftValidater;
    private KilnValidater _kilnValidater;
    private IconInfo _iconInfo;
    private CraftRecipeContainer _craftRecipeContainer;
    private CraftRecipeType _currentType;
    private RecipeType _currentRecipeType;
    private RecipeType CurrentRecipeType
    {
        get => _currentRecipeType;
        set
        {
            _currentRecipeType = value;
            switch (value)
            {
                case RecipeType.Craft:
                    craftTableUI.gameObject.SetActive(true);
                    kilnRecipeUI.gameObject.SetActive(false);
                    break;
                case RecipeType.Kiln:
                    craftTableUI.gameObject.SetActive(false);
                    kilnRecipeUI.gameObject.SetActive(true);
                    break;
            }
        }
    }
    
    private ResultButton _currentButton;
    
    public void Init(CraftRecipeContainer craftRecipeContainer)
    {
        _craftRecipeContainer = craftRecipeContainer;
        _craftValidater = Resources.Load<CraftValidater>("Crafting/CraftValidater");
        _kilnValidater = Resources.Load<KilnValidater>("Crafting/KilnValidater");
        _iconInfo = Resources.Load<IconInfo>("IconInfo");

        foreach (Transform button in categoryButtonParent)
        {
            button.GetComponent<CategoryButton>().Init(this);
        }
        
        ChooseCategory(RecipeType.Craft,CraftRecipeType.Tool);
        ChangeCurrent(resultGridParent.transform.GetChild(0).GetComponent<ResultButton>());
    }

    public void SetCraftTable(string id)
    {
        craftTableUI.ChangeToNewTable(_craftValidater.GetRecipeByResult(id));
    }
    
    public void SetKilnRecipe(string id)
    {
        kilnRecipeUI.ChangeToNewRecipe(_kilnValidater.GetRecipeByResult(id));
    }

    public void ChooseCategory(RecipeType recipeType, CraftRecipeType craftType)
    {
        CurrentRecipeType = recipeType;
        _currentType = craftType;
        
        //Clear all children at first
        foreach (Transform button in resultGridParent.transform)
        {
            Destroy(button.gameObject);
        }
        
        if(recipeType == RecipeType.Craft)
        {
            //Generate new buttons
            foreach (var recipe in _craftValidater.GetAllRecipeByType(craftType)
                         .Where(recipe => _craftRecipeContainer.unlockedRecipes.Contains(recipe)))
            {
                GenerateResultButtons(CurrentRecipeType, recipe.craftResult);
            }
        }
        else if(recipeType == RecipeType.Kiln)
        {
            //Generate new buttons
            foreach (var recipe in _kilnValidater.GetAllRecipeWithoutSameResult())
            {
                GenerateResultButtons(CurrentRecipeType, recipe.res);
            }
        }
    }

    public void Refresh()
    {
        ChooseCategory(CurrentRecipeType, _currentType);
    }
    
    private void GenerateResultButtons(RecipeType type,GroundableWithCount result)
    {
        switch (type)
        {
            case RecipeType.Craft:
                Instantiate(craftResultButtonPrefab, resultGridParent.transform).Init(result, _iconInfo, this);
                break;
            case RecipeType.Kiln:
                Instantiate(kilnResultButtonPrefab, resultGridParent.transform).Init(result, _iconInfo, this);
                break;
        }
    }

    public void ChangeCurrent(ResultButton button)
    {
        if (_currentButton)
        {
            _currentButton.Select(false);
        }
        _currentButton = button;
        _currentButton.Select(true);
    }
}

public enum RecipeType
{
    Craft,
    Kiln
}