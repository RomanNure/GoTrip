﻿using System;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;

namespace GoNTrip.Util
{
    public class Camera
    {
        public enum CameraLocation
        {
            FRONT,
            REAR
        };

        public async Task<Stream> TakePhoto(CameraLocation cameraLocation, int maxSize, int quality = 100)
        {
            StoreCameraMediaOptions cameraOptions = new StoreCameraMediaOptions();

            cameraOptions.DefaultCamera = cameraLocation == CameraLocation.FRONT ? CameraDevice.Front : CameraDevice.Rear;
            cameraOptions.MaxWidthHeight = Math.Max(0, maxSize);
            cameraOptions.CompressionQuality = Math.Min(Math.Max(quality, 0), 100);

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(cameraOptions);

            return file == null ? null : file.GetStream();
        }
    }
}