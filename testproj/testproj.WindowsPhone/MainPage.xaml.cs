using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Globalization.DateTimeFormatting;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using testproj.ValueConverters;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using testproj.Managers;
using Windows.UI.ViewManagement;


namespace testproj
{
    public sealed partial class MainPage : Page, IManageable
    {
        
        private IMobileServiceTable<Ritual> ritualTable = App.MobileService.GetTable<Ritual>();

        public MainPage()
        {
            this.InitializeComponent();
            myDatePicker.MinYear = DateTimeOffset.Now.AddYears(0);
        }
        
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageManager.RegisterPage(this);

            
        }

        private bool IsNameValid()
        {
            if ((goalNameTextBox.Text == "") || (goalDescriptionTextBox.Text == "") || (myDatePicker.Date.Date.Add(myTimePicker.Time) < DateTimeOffset.Now) == true)
                return false;
            else
                return true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PageManager.NavigateBack();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Disable UI buttons, show loading bar
        
            

            if (IsNameValid() == false)
            {
                await new MessageDialog("The Task Name and Description must not be empty and ensure that you have selected a valid future date & time").ShowAsync();
                return;
            }
            else
            
            {

                cancelButton.IsEnabled = false;
                saveButton.IsEnabled = false;
                tubelight.IsEnabled = false;
                coffeemachine.IsEnabled = false;
                buzzer.IsEnabled = false;
                goalNameTextBox.IsEnabled = false;
                goalDescriptionTextBox.IsEnabled = false;
                myDatePicker.IsEnabled = false;
                myTimePicker.IsEnabled = false;
                var sb = StatusBar.GetForCurrentView();
                sb.ProgressIndicator.ProgressValue = null;
                sb.ProgressIndicator.Text = "Submitting to Azure...";
                await sb.ProgressIndicator.ShowAsync();
               
                // Getting bool flags
                // TODO.

                var ritual = new Ritual();
                ritual.Description = goalDescriptionTextBox.Text;
                ritual.Name = goalNameTextBox.Text;
               // TimeSpan _gmt = new TimeSpan(5,30,0);
                ritual.EventDate = myDatePicker.Date.Date.Add(myTimePicker.Time);
                ritual.Tubelight = tubelight.IsChecked.Value;
                ritual.CoffeeMachine = coffeemachine.IsChecked.Value;
                ritual.Buzzer = buzzer.IsChecked.Value;
                bool res = await ServiceManager.TryInsertRitualAsync(ritualTable, ritual);
               // ritual.EventDate = myDatePicker.Date.Date.Add(myTimePicker.Time);
                if (res == true)
                {
                    bool chck = await CalendarManager.TryWriteAppointmentAsync(ritual);
                    if (chck == true)
                        await new MessageDialog("Appointment Set Sucessfully...").ShowAsync();
                    else
                        await new MessageDialog("Error...Appointment Not Written To Calendar..").ShowAsync();
                }
                else
                {
                    await new MessageDialog("Check Your Internet Connection!! Failed To Send To Cloud").ShowAsync();
                    PageManager.NavigateBack();
                }

                cancelButton.IsEnabled = true;
                saveButton.IsEnabled = true;
                tubelight.IsEnabled = true;
                coffeemachine.IsEnabled = true;
                buzzer.IsEnabled = true;
                goalNameTextBox.IsEnabled = true;
                goalDescriptionTextBox.IsEnabled = true;
                myDatePicker.IsEnabled = true;
                myTimePicker.IsEnabled = true;
                await sb.ProgressIndicator.HideAsync();
                sb = null;
                if (res == true)
                    PageManager.NavigateBack();
            }
        }

        public Dictionary<string, object> SaveState()
        {
            var state = new Dictionary<string, object>();
            state.Add("goalname", goalNameTextBox.Text);
            return state;
        }

        public void LoadState(Dictionary<string, object> lastState)
        {
            if (lastState == null)
                return;
            goalNameTextBox.Text = lastState["goalname"] as string;
        }

        public bool AllowAppExit()
        {
            return true;
        }
    }
}
