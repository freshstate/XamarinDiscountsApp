using System;
using Google.Maps;

#if __UNIFIED__
using UIKit;
using CoreGraphics;
using Foundation;
using CoreLocation;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using System.Drawing;

using CGRect = global::System.Drawing.RectangleF;
using CGSize = global::System.Drawing.SizeF;
using CGPoint = global::System.Drawing.PointF;

using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace GoogleMapsAdvSample
{
	public class MyLocationViewController : UIViewController
	{
		MapView mapView;
		bool firstLocationUpdate;

		public MyLocationViewController () : base ()
		{
			Title = "My Location";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var camera = CameraPosition.FromCamera (-33.868, 151.2086, 12);
			mapView = MapView.FromCamera (CGRect.Empty, camera);
			mapView.Settings.CompassButton = true;
			mapView.Settings.MyLocationButton = true;

			// Listen to the myLocation property of GMSMapView.
			mapView.AddObserver (this, new NSString ("myLocation"), NSKeyValueObservingOptions.New, IntPtr.Zero);

			View = mapView;
			// Ask for My Location data after the map has already been added to the UI.
			InvokeOnMainThread (()=> mapView.MyLocationEnabled = true);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
			mapView.RemoveObserver (this, new NSString ("myLocation"));
		}

		public override void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
		{
			//base.ObserveValue (keyPath, ofObject, change, context);

			if (!firstLocationUpdate) {
				// If the first location update has not yet been recieved, then jump to that
				// location.
				firstLocationUpdate = true;
				var location = change.ObjectForKey (NSValue.ChangeNewKey) as CLLocation;
				mapView.Camera = CameraPosition.FromCamera (location.Coordinate, 14);
			}
		}
	}
}

