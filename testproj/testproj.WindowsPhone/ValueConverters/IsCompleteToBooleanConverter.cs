using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;


namespace testproj.ValueConverters
{
   
        class IsCompleteToBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, string language)
            {
                ObservableCollection<DateTime> dates = (ObservableCollection<DateTime>)value;

                if (dates.Contains(DateTime.Today))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                throw new NotImplementedException();
            }
        }
    }

