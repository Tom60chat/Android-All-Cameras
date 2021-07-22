using Android.App;
using Android.Content;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camera2Basic
{
    public static class ExtraCamera
    {
        private static Dictionary<string, string[]> KnownCameraID = new Dictionary<string, string[]>()
        {
            { "Xiaomi", new string[]{ "20", "21","22", "23", "61", "63" } }, // 20 = Macro Light, 21 = UltraWide, 22 = Macro Dark, 61 = Back
        };

        public static string[] GetAllCameraIdList(Activity activity)
        {
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            var list = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                try
                {
                   manager.GetCameraCharacteristics(i.ToString());
                    list.Add(i.ToString());
                }
                catch { }
            }

            var cameras = manager.GetCameraIdList();

            return list
                .Concat(cameras)
                .GroupBy(item => item)
                .Select(group => group.First())
                .ToArray();
        }

        public static string[] GetKnowCameraIdList(Activity activity)
        {
            var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            var camerasId = manager.GetCameraIdList();
            var list = new List<string>();

            if (KnownCameraID.TryGetValue(Build.Brand, out var knownCamerasId))
                foreach(var knownCameraId in knownCamerasId)
                    try
                    {
                        manager.GetCameraCharacteristics(knownCameraId.ToString());
                        list.Add(knownCameraId.ToString());
                    }
                    catch { }

            return list
                .Concat(camerasId)
                .GroupBy(item => item)
                .Select(group => group.First())
                .ToArray();
        }
    }
}