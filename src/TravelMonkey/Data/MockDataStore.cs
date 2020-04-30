using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelMonkey.Models;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TravelMonkey.Data
{
    public class MockDataStore
    {
		public static ObservableCollection<PictureEntry> Pictures { get; set; }
            = new ObservableCollection<PictureEntry>();

        public static List<Destination> Destinations { get; set; } = new List<Destination>();
        public static ObservableCollection<FaceResult> FaceResults { get; set; } = new ObservableCollection<FaceResult>();
	}
}