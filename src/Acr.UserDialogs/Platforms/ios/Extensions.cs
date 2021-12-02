﻿using System;
#if __IOS__
using BigTed;
#endif
using Foundation;
using UIKit;
using Acr.UserDialogs.Infrastructure;


namespace Acr.UserDialogs
{
    public static class Extensions
    {
        public static void SafeInvokeOnMainThread(this UIApplication app, Action action) => app.InvokeOnMainThread(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log.Error("", ex.ToString());
            }
        });

        public static UIColor ToNative(this Microsoft.Maui.Graphics.Color This)
            => new UIColor(This.Red, This.Green, This.Blue, This.Alpha);

#if __IOS__

        public static BigTed.MaskType ToNative(this MaskType maskType)
        {
            switch (maskType)
            {
                case MaskType.Black: return BigTed.MaskType.Black;
                case MaskType.Clear: return BigTed.MaskType.Clear;
                case MaskType.Gradient: return BigTed.MaskType.Gradient;
                case MaskType.None: return BigTed.MaskType.None;
                default:
                    throw new ArgumentException("Invalid mask type");
            }
        }
#endif

		public static DateTime ToDateTime(this NSDate nsDate)
		{
			if (nsDate == null)
                return new DateTime();

			var cal = NSCalendar.CurrentCalendar;
			var year = (int)cal.GetComponentFromDate(NSCalendarUnit.Year, nsDate);
			var month = (int)cal.GetComponentFromDate(NSCalendarUnit.Month, nsDate);
			var day = (int)cal.GetComponentFromDate(NSCalendarUnit.Day, nsDate);
			var hour = (int)cal.GetComponentFromDate(NSCalendarUnit.Hour, nsDate);
			var minute = (int)cal.GetComponentFromDate(NSCalendarUnit.Minute, nsDate);
			var second = (int)cal.GetComponentFromDate(NSCalendarUnit.Second, nsDate);
			var nanosecond = (int)cal.GetComponentFromDate(NSCalendarUnit.Nanosecond, nsDate);
            var millisecond = (nanosecond / 1000000);

			return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Local);
		}


		public static NSDate ToNSDate(this DateTime dt)
		{
			if (dt == DateTime.MinValue)
                return null;

			var ldt = dt.ToLocalTime();
			var components = new NSDateComponents
			{
			    Year = ldt.Year,
                Month = ldt.Month,
                Day = ldt.Day,
                Hour = ldt.Hour,
                Minute = ldt.Minute,
                Second = ldt.Second,
                Nanosecond = (ldt.Millisecond * 1000000)
			};
			return NSCalendar.CurrentCalendar.DateFromComponents(components);
		}
    }
}
