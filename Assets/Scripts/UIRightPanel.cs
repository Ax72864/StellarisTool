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
            //var none = "��";
            var machine = saveData.fallen_machine_state == 0 ? "��" : saveData.fallen_machine_state == 1 ? "�õ�" : "����";
            var goo = saveData.gray_goo_stat == 0 ? "�ҷ�" : saveData.gray_goo_stat == 1 ? "�ҹƷ籩" : saveData.gray_goo_stat == 2 ? "L����" : "���׵۹�";
            AddTextLine($"L��ϵ:{goo}", $"��е��:{machine}");
            AddSpecialLine(SpecialOjbect.�������, SpecialOjbect.��̫��);
            AddSpecialLine(SpecialOjbect.�޾�����, SpecialOjbect.��η��);
            AddSpecialLine(SpecialOjbect.�������, SpecialOjbect.С���Ƿ䳲);
            AddSpecialLine(SpecialOjbect.���ǳ�, SpecialOjbect.λ�����);
            AddSpecialLine(SpecialOjbect.ʰ����, SpecialOjbect.�����ä);
            AddSpecialLine(SpecialOjbect.��ѡ��, SpecialOjbect.ˮ��֮��);
            AddSpecialLine(SpecialOjbect.������, SpecialOjbect.������);
            AddSpecialLine(SpecialOjbect.����֮��, SpecialOjbect.���ﰺ��˹);
            AddSpecialLine(SpecialOjbect.���ֿ�˹, SpecialOjbect.������˹�ĵֿ�);
            AddSpecialLine(SpecialOjbect.ղ����˹�����, SpecialOjbect.������);
            AddSpecialLine(SpecialOjbect.�����֮��, SpecialOjbect.���Ѩ);
            AddSpecialLine(SpecialOjbect.����֮��, SpecialOjbect.�¿�����);
            AddSpecialLine(SpecialOjbect.����, SpecialOjbect.������);
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
                text[0].text = $"{SpecialOjbect.GetName(leftKey)}:��";
            }
            else
            {
                text[0].text = $"{SpecialOjbect.GetName(leftKey)}:��";
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
                text[1].text = $"{SpecialOjbect.GetName(rightKey)}:��";
            }
            else
            {
                text[1].text = $"{SpecialOjbect.GetName(rightKey)}:��";
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
