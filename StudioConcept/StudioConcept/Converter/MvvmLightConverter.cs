using GalaSoft.MvvmLight.Command;

namespace StudioConcept.Converter
{
    public class MvvmLightConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            return value;
        }
    }
}
