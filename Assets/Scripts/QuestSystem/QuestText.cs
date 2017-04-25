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
        [SerializeField]
        private string endDialog;

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

        public string HintDialog
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

        public string EndDialog
        {
            get
            {
                return endDialog;
            }
        }

        public QuestText(string _title, string _descriptionSummary, string _dialog, string _endDialog, string _hint)
        {
            title = _title;
            descriptionSummary = _descriptionSummary;
            dialog = _dialog;
            endDialog = _endDialog;
            hint = _hint;
        }

        public QuestText(QuestText qt)
        {
            title = qt.title;
            descriptionSummary = qt.descriptionSummary;
            dialog = qt.dialog;
            endDialog = qt.endDialog;
            hint = qt.hint;
        }
    }

}
