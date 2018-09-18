using System;
using Humanizer;
using PSC.Xamarin.MvvmHelpers;

namespace App1
{
    class Message : ObservableObject
    {
        string text;

        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        DateTime messageDateTime;

        public DateTime MessageDateTime
        {
            get { return messageDateTime; }
            set { SetProperty(ref messageDateTime, value); }
        }

        public string MessageTimeDisplay => MessageDateTime.Humanize();

        bool isIncoming;

        public bool IsIncoming
        {
            get { return isIncoming; }
            set { SetProperty(ref isIncoming, value); }
        }

    }
}
