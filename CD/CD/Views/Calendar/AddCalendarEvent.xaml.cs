﻿using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using System;
using CD.Helper;
using System.Drawing;
using Syncfusion.XForms.Buttons;

namespace CD.Views.Calendar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCalendarEvent
    {
        private DateTime start_Date;
        private DateTime end_Date;
        readonly FireBaseHelperCalendarEvents fireBaseHelper = new FireBaseHelperCalendarEvents();
        private int color = 0;

        public AddCalendarEvent(DateTime selectedDate)
        {
            InitializeComponent();
            string[] theDate = SimplePage.parseDate(selectedDate);

            // date selected displayed in the pop-up form
            startDate.Date = selectedDate;
            endDate.Date = selectedDate;
            start_Date = selectedDate;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void Save_Event(object sender, System.EventArgs e)
        {
            save_button.IsEnabled = false;
            string name = event_name.Text;
            string desc = event_description.Text;
            DateTime start_Date = new DateTime(startDate.Date.Year, startDate.Date.Month, startDate.Date.Day, startTimePicker.Time.Hours, startTimePicker.Time.Minutes, startTimePicker.Time.Seconds);
            DateTime end_Date = new DateTime(endDate.Date.Year, endDate.Date.Month, endDate.Date.Day, endTimePicker.Time.Hours, endTimePicker.Time.Minutes, endTimePicker.Time.Seconds);
            Color colorEvent = colorSelected(color);
            if (!string.IsNullOrEmpty(name))
            {
                await fireBaseHelper.AddEvent(name, desc, start_Date, end_Date, colorEvent);
                await PopupNavigation.RemovePageAsync(this);
            }
            else
                await DisplayAlert("Failed", "Please add a name to the event", "OK");
            save_button.IsEnabled = true;

            // repopulating the calendar
            await SimplePage.Instance.refreshCalendar();
        }

        [Obsolete]
        private async void Cancel_Event(object sender, System.EventArgs e)
        {
            cancel_button.IsEnabled = false;
            await PopupNavigation.RemovePageAsync(this);
            cancel_button.IsEnabled = true;
        }

        private void OnTimePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void Handle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // tap a color on the selection line 
            segmentedControl.SelectionChanged += Handle_SelectionChanged;
            color = segmentedControl.SelectedIndex;
        }

        private Color colorSelected(int theColor)
        {
            if (theColor == 0)
                return Color.Red;
            else if (theColor == 1)
                return Color.Orange;
            else if (theColor == 2)
                return Color.DeepPink;
            else if (theColor == 3)
                return Color.Blue;
            else if (theColor == 4)
                return Color.MediumSeaGreen;
            else if (theColor == 5)
                return Color.BlueViolet;
            else
                return Color.Blue;
        }
    }
}