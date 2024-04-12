using System.Collections;
using System.Collections.Generic;
using LitFramework;
using UnityEngine;
using UnityEngine.UI;

namespace StellarisTool
{
    public class UIRightPanel : MonoBehaviour
    {
        private Animator animator;

        public Text lbl_name;
        public Text lbl_time;
        public RectTransform info_list;

        public Slider sld_move;
        public Slider sld_zoom;


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Show();

            sld_move.value = OrthoCameraController.panSpeed;
            sld_zoom.value = -OrthoCameraController.zoomSpeed;
        }

        public void Show()
        {
            animator.Play("right_panel_in");
        }

        public void Hide()
        {
            animator.Play("right_panel_out");
        }

        private StellarisSave saveData;
        public void SetSaveData(StellarisSave save)
        {
            saveData = save;
            SetInfo();
        }

        public void SetInfo()
        {
            lbl_name.text = saveData.meta.name;
            lbl_time.text = saveData.meta.date;
            //var none = "无";
            var machine = saveData.fallen_machine_state == 0 ? "无" : saveData.fallen_machine_state == 1 ? "好的" : "坏的";
            var goo = saveData.gray_goo_stat == 0 ? "灰风" : saveData.gray_goo_stat == 1 ? "灰蛊风暴" : saveData.gray_goo_stat == 2 ? "L星龙" : "纳米帝国";
            AddTextLine($"L星系:{goo}", $"机械爹:{machine}");
            AddSpecialLine(SpecialOjbect.无限神机, SpecialOjbect.以太龙);
            AddSpecialLine(SpecialOjbect.无尽堡垒, SpecialOjbect.无畏舰);
            AddSpecialLine(SpecialOjbect.提杨凯长老, SpecialOjbect.小行星蜂巢);
            AddSpecialLine(SpecialOjbect.噬星虫, SpecialOjbect.位面异怪);
            AddSpecialLine(SpecialOjbect.拾荒者, SpecialOjbect.虚空文盲);
            AddSpecialLine(SpecialOjbect.天选民, SpecialOjbect.水晶之国);
            AddSpecialLine(SpecialOjbect.避难所, SpecialOjbect.极域警哨);
            AddSpecialLine(SpecialOjbect.联邦之终, SpecialOjbect.拉里昂内斯);
            AddSpecialLine(SpecialOjbect.泽沃克斯, SpecialOjbect.芬拉克斯的抵抗);
            AddSpecialLine(SpecialOjbect.詹若克斯的休憩, SpecialOjbect.阿法尔);
            AddSpecialLine(SpecialOjbect.缇杨娜之巢, SpecialOjbect.缇云穴);
            AddSpecialLine(SpecialOjbect.爱神之槽, SpecialOjbect.温科沃特);
            AddSpecialLine(SpecialOjbect.哈尔, SpecialOjbect.贺利托);
        }

        void AddTextLine(string left, string right)
        {
            var gameObject = AssetManager.GetObject("InfoLine");
            var text = gameObject.GetComponentsInChildren<Text>();
            text[0].text = left;
            text[1].text = right;
            gameObject.transform.SetParent(info_list, false);
        }


        void AddSpecialLine(string leftKey,string rightKey)
        {
            var gameObject = AssetManager.GetObject("InfoLine");
            var text = gameObject.GetComponentsInChildren<Text>();
            var leftId = saveData.SpecialSystem[leftKey];
            if (leftId > 0)
            {
                text[0].text = $"{SpecialOjbect.GetName(leftKey)}:{saveData.GetStar(leftId).name}";
                text[0].GetComponent<Button>().onClick.AddListener(() =>
                {
                    GoFocusSystem(leftId);
                });
            } else if (leftId == 0)
            {
                text[0].text = $"{SpecialOjbect.GetName(leftKey)}:有";
            }
            else
            {
                text[0].text = $"{SpecialOjbect.GetName(leftKey)}:无";
            }

            var rightId = saveData.SpecialSystem[rightKey];
            if (rightId > 0)
            {
                text[1].text = $"{SpecialOjbect.GetName(rightKey)}:{saveData.GetStar(rightId).name}";
                text[1].GetComponent<Button>().onClick.AddListener(() =>
                {
                    GoFocusSystem(rightId);
                });
            }
            else if (rightId == 0)
            {
                text[1].text = $"{SpecialOjbect.GetName(rightKey)}:有";
            }
            else
            {
                text[1].text = $"{SpecialOjbect.GetName(rightKey)}:无";
            }
            gameObject.transform.SetParent(info_list, false);

        }

        void GoFocusSystem(long e)
        {
            LitEventSystem.Global.Send(new EventFocusSystem()
            {
                id = e,
                type = 4,
            });
        }

        public void OnMoveSpeedSlider()
        {
            OrthoCameraController.panSpeed = sld_move.value;
        }

        public void OnZoomSpeedSlider()
        {
            OrthoCameraController.zoomSpeed = -sld_zoom.value;
        }
    }
}
