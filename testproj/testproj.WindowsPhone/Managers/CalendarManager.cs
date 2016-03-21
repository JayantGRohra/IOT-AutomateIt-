using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;

namespace testproj.Managers
{
    
   
        class CalendarManager
        {
           
            #region Internal Management

            private static AppointmentStore _store;
            private static AppointmentCalendar _calendar;

            private static async Task EnsureAvailabilityAsync()
            {
                if (_store == null)
                    _store = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
            }

            private static async Task CreateNewCalendarAsync()
            {
                await EnsureAvailabilityAsync();

                var appCalendars = await _store.FindAppointmentCalendarsAsync();
                foreach (AppointmentCalendar cal in appCalendars)
                    await cal.DeleteAsync();

                AppointmentCalendar calendar = await _store.CreateAppointmentCalendarAsync("Home Automation Calendar");
                calendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.SystemOnly;
                calendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.None;
                await calendar.SaveAsync();
                _calendar = calendar;
            }

            private static async Task<bool> TryLoadCalendarAsync()
            {
                if (_calendar != null)
                    return true;
                try
                {
                    await EnsureAvailabilityAsync();
                    _calendar = (await _store.FindAppointmentCalendarsAsync())[0];
                    return true;
                }
                catch
                {
                    _calendar = null;
                    return false;
                }
            }

            public static async Task InitializeAsync()
            {
                bool result = await CalendarManager.TryLoadCalendarAsync();
                if (result == false)
                    await CalendarManager.CreateNewCalendarAsync();
            }

            #endregion

            public static async Task<bool> TryWriteAppointmentAsync(Ritual r)
            {

                
                try{
                    Appointment appt = new Appointment();
                   
                    appt.Subject = r.Name;
                    appt.StartTime = r.EventDate;
                    appt.Details = r.Description;

                  var reminderTime = new TimeSpan();                
                  appt.Reminder = reminderTime;//vinay
             
                    await _calendar.SaveAppointmentAsync(appt);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public static async Task<bool> TryDeleteAppointmentAsync(Ritual r)
            {
                try
                {
                    
                    TimeSpan ts = new TimeSpan(0,0,2);
                    
                    var _list=  await _calendar.FindAppointmentsAsync(r.EventDate,ts);
                        foreach (Appointment a in _list)
                            if(a.Subject == r.Name)
                                await _calendar.DeleteAppointmentAsync(a.LocalId);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
          
        }
    }

