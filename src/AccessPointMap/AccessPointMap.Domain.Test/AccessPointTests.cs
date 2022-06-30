using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Exceptions;
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now.AddDays(2)
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now.AddDays(2)
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now.AddDays(2)
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now.AddDays(2)
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

        [Fact]
        public void AccessPointAdnnotationShouldCreate()
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            Assert.Empty(accesspoint.Adnnotations);

            string adnnotationTitle = "New adnnotation";
            string adnnotationContent = "New adnnotation content!";

            accesspoint.Apply(new V1.AccessPointAdnnotationCreated
            {
                Id = accesspoint.Id,
                Title = adnnotationTitle,
                Content = adnnotationContent
            });

            Assert.NotEmpty(accesspoint.Adnnotations);

            var adnnotation = accesspoint.Adnnotations.Single();

            Assert.Equal(adnnotationTitle, adnnotation.Title);
            Assert.Equal(adnnotationContent, adnnotation.Content);
        }

        [Fact]
        public void AccessPointAdnnotationShouldDelete()
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
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            Assert.Empty(accesspoint.Adnnotations);

            string adnnotationTitle = "New adnnotation";
            string adnnotationContent = "New adnnotation content!";

            accesspoint.Apply(new V1.AccessPointAdnnotationCreated
            {
                Id = accesspoint.Id,
                Title = adnnotationTitle,
                Content = adnnotationContent
            });

            Assert.NotEmpty(accesspoint.Adnnotations);

            var adnnotationId = accesspoint.Adnnotations.Single().Id;

            accesspoint.Apply(new V1.AccessPointAdnnotationDeleted
            {
                Id = accesspoint.Id,
                AdnnotationId = adnnotationId
            });

            Assert.Empty(accesspoint.Adnnotations);
        }

        [Fact]
        public void AccessPointPacketShouldCreate()
        {
            var bssid = "00:00:00:00:00:00";

            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = bssid,
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            var packetData = Convert.ToBase64String(new byte[] { 1 });
            var packetFrameType = "Beacon Request";
            var packetDestination = "11:11:11:11:11:11";

            accesspoint.Apply(new V1.AccessPointPacketCreated
            {
                Id = accesspoint.Id,
                Data = packetData,
                FrameType = packetFrameType,
                SourceAddress = bssid,
                DestinationAddress = packetDestination
            });

            Assert.NotEmpty(accesspoint.Packets);

            var packet = accesspoint.Packets.Single();

            Assert.Equal(packetData, packet.Data);
            Assert.Equal(packetDestination, packet.DestinationAddress);
        }

        [Fact]
        public void AccessPointPacketCreateShouldThrowOnNotMatchingHardwareAddress()
        {
            var accessPointBssid = "00:00:00:00:00:00";

            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = accessPointBssid,
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            var packetData = Convert.ToBase64String(new byte[] { 1 });
            var packetFrameType = "Beacon request";
            var packetDestination = "11:11:11:11:11:11";
            var invalidHardwareAddress = "22:22:22:22:22:22";

            Assert.Throws<BusinessLogicException>(() =>
            {
                accesspoint.Apply(new V1.AccessPointPacketCreated
                {
                    Id = accesspoint.Id,
                    Data = packetData,
                    FrameType = packetFrameType,
                    SourceAddress = invalidHardwareAddress,
                    DestinationAddress = packetDestination
                });
            });
        }

        [Fact]
        public void AccessPointPacketCreateShouldThrowOnEmptyFrameType()
        {
            var accessPointBssid = "00:00:00:00:00:00";

            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = accessPointBssid,
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            var packetData = Convert.ToBase64String(new byte[] { 1 });
            string packetFrameType = null;
            var packetDestination = string.Empty;

            Assert.Throws<ValueObjectValidationException>(() =>
            {
                accesspoint.Apply(new V1.AccessPointPacketCreated
                {
                    Id = accesspoint.Id,
                    Data = packetData,
                    FrameType = packetFrameType,
                    SourceAddress = accessPointBssid,
                    DestinationAddress = packetDestination
                });
            });
        }

        [Fact]
        public void AccessPointPacketShouldDelete()
        {
            var bssid = "00:00:00:00:00:00";

            var accesspoint = AccessPoint.Factory.Create(new V1.AccessPointCreated
            {
                Bssid = bssid,
                Ssid = "Test-Hotspot",
                Frequency = 2670,
                LowSignalLevel = -70,
                LowSignalLatitude = 48.8583,
                LowSignalLongitude = 2.2944,
                HighSignalLevel = -30,
                HighSignalLatitude = 48.86,
                HighSignalLongitude = 2.30,
                RawSecurityPayload = "[WPA2][WEP]",
                UserId = Guid.NewGuid(),
                ScanDate = DateTime.Now
            });

            var packetData = Convert.ToBase64String(new byte[] { 1 });
            var packetFrameType = "Beacon request";
            var packetDestination = "11:11:11:11:11:11";

            accesspoint.Apply(new V1.AccessPointPacketCreated
            {
                Id = accesspoint.Id,
                Data = packetData,
                FrameType = packetFrameType,
                SourceAddress = bssid,
                DestinationAddress = packetDestination
            });

            Assert.NotEmpty(accesspoint.Packets);

            var packetId = accesspoint.Packets.Single().Id;

            accesspoint.Apply(new V1.AccessPointPacketDeleted
            {
                Id = accesspoint.Id,
                PacketId = packetId
            });

            Assert.Empty(accesspoint.Packets);
        }
    }
}
