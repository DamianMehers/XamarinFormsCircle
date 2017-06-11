using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using XamarinFormsCircle.Controls;

namespace XamarinFormsCircle.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Segment> _segments = new ObservableCollection<Segment>
        {
            new Segment {Color = Color.DarkGoldenrod, SweepAngle = 90},
            new Segment {Color = Color.DodgerBlue, SweepAngle = 180},
            new Segment {SweepAngle = 45, Color = Color.YellowGreen},
            new Segment {SweepAngle = 45, Color = Color.DarkOrange}
        };

        public Action Refresh { get; set; }

        public ObservableCollection<Segment> Segments {
            get => _segments;
            set {
                _segments = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ChangeSegmentsCommand = new Command(ChangeSegments);
        }

        private void ChangeSegments()
        {
            Segments[0].Color = Color.Black;
            Segments.RemoveAt(1);
            Refresh();
        }

        public Command ChangeSegmentsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
