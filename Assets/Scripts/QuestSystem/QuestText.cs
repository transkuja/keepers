using System;
using UnityEngine;

namespace QuestSystem
{
    [Serializable]
    public class QuestText
    {
        [SerializeField]
        private string title;
        [SerializeField]
        private string descriptionSummary;
        [SerializeField]
        private string dialog;
        [SerializeField]
        private string hint;

        public string DescriptionSummary
        {
            get
            {
                return descriptionSummary;
            }
        }

        public string Dialog
        {
            get
            {
                return dialog;
            }
        }

        public string Hint
        {
            get
            {
                return hint;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public QuestText(string _title, string _descriptionSummary, string _dialog, string _hint)
        {
            title = _title;
            descriptionSummary = _descriptionSummary;
            dialog = _dialog;
            hint = _hint;
        }
    }

}
