using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using testproj.Managers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace testproj
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    

   
    public sealed partial class ListPage : Page,IManageable
    {

        public MobileServiceCollection<Ritual, Ritual> RitualCollection 
        { get; 
          set; }
        private IMobileServiceTable<Ritual>ritualTable = App.MobileService.GetTable<Ritual>();

        public ListPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PageManager.RegisterPage(this);
        }

        public Dictionary<string, object> SaveState()
        {
            return null;
        }

        public async void LoadState(Dictionary<string, object> lastState)
        {
            // disable
            AddRitual.IsEnabled = false;
            AddAlert.IsEnabled = false;
            ViewAbout.IsEnabled = false;
            var sb = StatusBar.GetForCurrentView();
            sb.ProgressIndicator.ProgressValue = null;
            sb.ProgressIndicator.Text = "Syncing...";
            await sb.ProgressIndicator.ShowAsync();
          
            
            RitualCollection = await ServiceManager.SelectAllAsync(ritualTable);
            if (RitualCollection == null)
                await new MessageDialog("Check your Internet Connection.").ShowAsync();
            this.DataContext = this;
          
            AddRitual.IsEnabled = true;
            AddAlert.IsEnabled = true;
            ViewAbout.IsEnabled = true;
            await sb.ProgressIndicator.HideAsync();
            sb = null;

        }

        public bool AllowAppExit()
        {
            return true;
        }

        private void AddRitual_Click(object sender, RoutedEventArgs e)
        {
            PageManager.NavigateTo(typeof(MainPage), null, NavigationType.Default);
        }

        private void AlertPage_Click(object sender, RoutedEventArgs e)
        {
            PageManager.NavigateTo(typeof(AlertPage), null, NavigationType.Default);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            PageManager.NavigateTo(typeof(AboutPage), null, NavigationType.Default);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            bool res, chck;
            AddRitual.IsEnabled = false;
            AddAlert.IsEnabled = false;
            ViewAbout.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            var sb = StatusBar.GetForCurrentView();
            sb.ProgressIndicator.ProgressValue = null;
            sb.ProgressIndicator.Text = "Deleting...";
            await sb.ProgressIndicator.ShowAsync();
            
            var r = (sender as FrameworkElement).DataContext as Ritual;
            res = await ServiceManager.TryDeleteRitualAsync(ritualTable, r);
            if (res == false)
                await new MessageDialog("Check Your Network Connection! \n Failed To Delete").ShowAsync();
            else
            {
                RitualCollection.Remove(r);
                chck = await CalendarManager.TryDeleteAppointmentAsync(r);
                if (chck == true)
                    await new MessageDialog("Appointment Deleted Sucessfully...").ShowAsync();
                else
                    await new MessageDialog("Appointment Not Deleted Successfully...").ShowAsync();
            }

            AddRitual.IsEnabled = true;
            AddAlert.IsEnabled = true;
            ViewAbout.IsEnabled = true;
            DeleteButton.IsEnabled = true;


            await sb.ProgressIndicator.HideAsync();
            sb = null;
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            deleteMenuFlyout.ShowAt(sender as FrameworkElement);
        }

    }
}
