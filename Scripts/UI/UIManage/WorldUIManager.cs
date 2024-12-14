using Barterta.ToolScripts;
using Barterta.UI.WorldUI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.UI.UIManage
{
    public class WorldUIManager : Singleton<WorldUIManager>
    {
        public UIObject fractionUIPrefab;
        public UIObject dialogUIPrefab;
        public BackpackUI backpackPrefab;

        public static void ChangeFractionUI(UIObject ui, int numerator, int denominator)
        {
            ui.GetComponentInChildren<TextMeshProUGUI>().text = "<bounce>" + numerator + "/" + denominator + "<bounce>";
        }

        public UIObject GenerateUI(UIObject prefab, Vector3 pos, float height = 0)
        {
            return Instantiate(prefab,
                Methods.YtoZero(pos) + new Vector3(0, Constant.UI.FixedWorldUIHeight + height, 0),
                Quaternion.Euler(45, 0, 0), transform);
        }

        public UIObject GenerateUI(UIObject prefab, Transform follow, float height = 0)
        {
            var ui = Instantiate(prefab, Vector3.zero, Quaternion.Euler(45, 0, 0), transform);
            var followObject = ui.gameObject.AddComponent<FollowObject>();
            followObject.offset = new Vector3(0, Constant.UI.FixedWorldUIHeight + height, 0);
            followObject.followTran = follow;
            return ui;
        }

        public static void ChangeUILayer(GameObject ui,Mark.Mark playerMark, bool isWorldUI = true)
        {
            //Unity problem, need a canvas on it to get culling mask
            if(isWorldUI) ui.AddComponent<Canvas>();
            ui.layer = LayerMask.NameToLayer("Player" + playerMark.id);
            foreach (Transform tran in ui.transform)
            {
                tran.gameObject.layer = LayerMask.NameToLayer("Player" + playerMark.id);
            }
        }

        public Sequence DestroyUI(UIObject ui)
        {
            return HideUI(ui).OnComplete(() => Destroy(ui.gameObject));
        }

        public Sequence HideUI(UIObject ui)
        {
            var sq = DOTween.Sequence();
            sq.Join(ui.transform.DOScale(new Vector3(0, 0, 0), .2f).SetEase(Ease.InSine));

            foreach (var text in ui.GetComponentsInChildren<TextMeshProUGUI>()) sq.Join(text.DOFade(0, .4f));

            foreach (var image in ui.GetComponentsInChildren<Image>()) sq.Join(image.DOFade(0, .4f));

            return sq;
        }

        public Sequence AppealUI(UIObject ui)
        {
            var sq = DOTween.Sequence();

            sq.Join(ui.transform.DOScale(new Vector3(ui.originScale, ui.originScale, ui.originScale), .2f)
                .SetEase(Ease.OutSine));

            foreach (var text in ui.GetComponentsInChildren<TextMeshProUGUI>())
            {
                float alpha = 1;
                if (ui.TextOriginalColorDict.ContainsKey(text))
                {
                    alpha = ui.TextOriginalColorDict[text].a;
                }

                text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                sq.Join(text.DOFade(alpha, .3f));
            }

            foreach (var image in ui.GetComponentsInChildren<Image>())
            {
                float alpha = 1;
                if (ui.ImageOriginalColorDict.ContainsKey(image))
                {
                    alpha = ui.ImageOriginalColorDict[image].a;
                }

                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                sq.Join(image.DOFade(alpha, .3f));
            }

            return sq;
        }
    }
}