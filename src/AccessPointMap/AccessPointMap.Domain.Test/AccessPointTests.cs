using AccessPointMap.Domain.AccessPoints;
using System;
using System.Linq;
using Xunit;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.Test
{
    public class AccessPointTests
    {
        [Fact]
        public void AccessPointShouldCreate()
        {
            AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });
        }

        [Fact]
        public void AccessPointShouldDelete()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            accesspoint.Apply(new V1.AccessPointDeleted
            {
                Id = accesspoint.Id
            });

            Assert.Equal(accesspoint.DeletedAt.Value.Date, DateTime.Now.Date);
            Assert.NotNull(accesspoint.DeletedAt);
        }

        [Fact]
        public void AccessPointShouldUpdate()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            string noteExpectedBefore = string.Empty;
            Assert.Equal(noteExpectedBefore, accesspoint.Note);

            accesspoint.Apply(new V1.AccessPointUpdated
            {
                Id = accesspoint.Id,
                Note = "AccessPointMap project"
            });

            string noteExpectedAfter = "AccessPointMap project";
            Assert.Equal(noteExpectedAfter, accesspoint.Note);
        }

        [Fact]
        public void AccessPointStatusShouldChange()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            bool statusExpectedBefore = false;
            Assert.Equal(statusExpectedBefore, accesspoint.DisplayStatus);

            accesspoint.Apply(new V1.AccessPointDisplayStatusChanged
            {
                Id = accesspoint.Id,
                Status = true
            });

            bool statusExpectedAfter = true;
            Assert.Equal(statusExpectedAfter, accesspoint.DisplayStatus);
        }

        [Fact]
        public void AccessPointManufacturerShouldChange()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            string manufacturerExpectedBefore = string.Empty;
            Assert.Equal(manufacturerExpectedBefore, accesspoint.Manufacturer);

            accesspoint.Apply(new V1.AccessPointManufacturerChanged
            {
                Id = accesspoint.Id,
                Manufacturer = "NETWORK DEVICES"
            });

            string manufacturerExpectedAfter = "NETWORK DEVICES";
            Assert.Equal(manufacturerExpectedAfter, accesspoint.Manufacturer);
        }

        [Fact]
        public void AccessPointShouldMergeWithStampAndAllValuesSetToMerge()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            int updatedLowSignalLevel = -80;
            double updatedLowSignalLatitude = 49.0;
            double updatedLowSignalLongitude = 1.0;

            int updatedHighSignalLevel = -40;
            double updatedHighSignalLatitude = 47.0;
            double updatedHighSignalLongitude = 1.0;

            string updatedSsid = "Test-Hotspot-New";

            var stampCreationEvent = new V1.AccessPointStampCreated
            {
                Ssid = updatedSsid,
                Frequency = 2670,
                LowSignalLevel = updatedLowSignalLevel,
                LowSignalLatitude = updatedLowSignalLatitude,
                LowSignalLongitude = updatedLowSignalLongitude,
                HighSignalLevel = updatedHighSignalLevel,
                HighSignalLatitude = updatedHighSignalLatitude,
                HighSignalLongitude = updatedHighSignalLongitude,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            };

            accesspoint.Apply(stampCreationEvent);

            var stampId = accesspoint.Stamps.First().Id;

            accesspoint.Apply(new V1.AccessPointMergedWithStamp
            {
                Id = accesspoint.Id,
                StampId = stampId,
                MergeSsid = true,
                MergeLowSignalLevel = true,
                MergeHighSignalLevel = true,
                MergeSecurityData = true
            });

            var stamp = accesspoint.Stamps.First();

            Assert.Equal(updatedLowSignalLevel, accesspoint.Positioning.LowSignalLevel);
            Assert.Equal(updatedLowSignalLatitude, accesspoint.Positioning.LowSignalLatitude);
            Assert.Equal(updatedLowSignalLongitude, accesspoint.Positioning.LowSignalLongitude);

            Assert.NotEqual(updatedHighSignalLevel, accesspoint.Positioning.HighSignalLevel);
            Assert.NotEqual(updatedHighSignalLatitude, accesspoint.Positioning.HighSignalLatitude);
            Assert.NotEqual(updatedHighSignalLongitude, accesspoint.Positioning.HighSignalLongitude);

            Assert.Equal(updatedSsid, accesspoint.Ssid);

            Assert.Equal(stamp.CreationTimestamp.Value, accesspoint.VersionTimestamp.Value);

            Assert.True(stamp.Status);
        }

        [Fact]
        public void AccessPointShouldMergeWithStampAndTheLowSignalLevelAndSsidIsSkiped()
        {
            string initialSsid = "Test-Hotspot";

            int initialLowSignalLevel = -70;
            double initialLowSignalLatitude = 48.8583;
            double initialLowSignalLongitude = 2.2944;

            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = initialSsid,
                Frequency = 2670,
                LowSignalLevel = initialLowSignalLevel,
                LowSignalLatitude = initialLowSignalLatitude,
                LowSignalLongitude = initialLowSignalLongitude,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            int updatedLowSignalLevel = -80;
            double updatedLowSignalLatitude = 49.0;
            double updatedLowSignalLongitude = 1.0;

            int updatedHighSignalLevel = -40;
            double updatedHighSignalLatitude = 47.0;
            double updatedHighSignalLongitude = 1.0;

            string updatedSsid = "Test-Hotspot-New";

            var stampCreationEvent = new V1.AccessPointStampCreated
            {
                Ssid = updatedSsid,
                Frequency = 2670,
                LowSignalLevel = updatedLowSignalLevel,
                LowSignalLatitude = updatedLowSignalLatitude,
                LowSignalLongitude = updatedLowSignalLongitude,
                HighSignalLevel = updatedHighSignalLevel,
                HighSignalLatitude = updatedHighSignalLatitude,
                HighSignalLongitude = updatedHighSignalLongitude,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            };

            accesspoint.Apply(stampCreationEvent);

            var stampId = accesspoint.Stamps.First().Id;

            accesspoint.Apply(new V1.AccessPointMergedWithStamp
            {
                Id = accesspoint.Id,
                StampId = stampId,
                MergeSsid = false,
                MergeLowSignalLevel = false,
                MergeHighSignalLevel = true,
                MergeSecurityData = true
            });

            var stamp = accesspoint.Stamps.First();

            Assert.Equal(initialLowSignalLevel, accesspoint.Positioning.LowSignalLevel);
            Assert.Equal(initialLowSignalLatitude, accesspoint.Positioning.LowSignalLatitude);
            Assert.Equal(initialLowSignalLongitude, accesspoint.Positioning.LowSignalLongitude);

            Assert.NotEqual(updatedHighSignalLevel, accesspoint.Positioning.HighSignalLevel);
            Assert.NotEqual(updatedHighSignalLatitude, accesspoint.Positioning.HighSignalLatitude);
            Assert.NotEqual(updatedHighSignalLongitude, accesspoint.Positioning.HighSignalLongitude);

            Assert.Equal(initialSsid, accesspoint.Ssid);

            Assert.Equal(stamp.CreationTimestamp.Value, accesspoint.VersionTimestamp.Value);

            Assert.True(stamp.Status);
        }

        [Fact]
        public void AccessPointStampShouldCreate()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            var stampCreationEvent = new V1.AccessPointStampCreated
            {
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            };

            int expectedStampsCollectionCountBefore = 0;
            Assert.Equal(expectedStampsCollectionCountBefore, accesspoint.Stamps.Count);

            accesspoint.Apply(stampCreationEvent);

            int expectedStampsCollectionCountAfter = 1;
            Assert.Equal(expectedStampsCollectionCountAfter, accesspoint.Stamps.Count);

            bool expectedStampVerificationStatus = false;
            Assert.Equal(expectedStampVerificationStatus, accesspoint.Stamps.First().Status);
        }

        [Fact]
        public void AccessPointStampShouldDelete()
        {
            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = "00:00:00:00:00:00",
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            });

            var stampCreationEvent = new V1.AccessPointStampCreated
            {
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid()
            };

            int expectedStampsCollectionCountBefore = 0;
            Assert.Equal(expectedStampsCollectionCountBefore, accesspoint.Stamps.Count);

            accesspoint.Apply(stampCreationEvent);

            int expectedStampsCollectionCountAfter = 1;
            Assert.Equal(expectedStampsCollectionCountAfter, accesspoint.Stamps.Count);

            var stampId = accesspoint.Stamps.First().Id;

            accesspoint.Apply(new V1.AccessPointStampDeleted
            {
                Id = accesspoint.Id,
                StampId = stampId
            });

            int expectedStampsCollectionCountAfterAfter = 0;
            Assert.Equal(expectedStampsCollectionCountAfterAfter, accesspoint.Stamps.Count);
        }
    }
}
