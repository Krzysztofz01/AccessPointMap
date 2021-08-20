using AccessPointMap.Domain.Common;
using AccessPointMap.Domain.Extensions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AccessPointMap.Domain
{
    public class AccessPoint : BaseEntity
    {
        //Properties
        public string Bssid { get; private set; }
        public string Ssid { get; private set; }
        public string Fingerprint { get; private set; }
        public double Frequency { get; private set; }
        public int MaxSignalLevel { get; private set; }
        public double MaxSignalLongitude { get; private set; }
        public double MaxSignalLatitude { get; private set; }
        public int MinSignalLevel { get; private set; }
        public double MinSignalLongitude { get; private set; }
        public double MinSignalLatitude { get; private set; }
        public double SignalRadius { get; private set; }
        public double SignalArea { get; private set; }
        public string FullSecurityData { get; private set; }
        public string SerializedSecurityData { get; private set; }
        public bool IsSecure { get; private set; }
        public bool IsHidden { get; private set; }
        public string Manufacturer { get; private set; }
        public string DeviceType { get; private set; }
        public bool MasterGroup { get; private set; }
        public bool Display { get; private set; }
        public string Note { get; private set; }
        public long? UserAddedId { get; private set; }
        public virtual User UserAdded { get; private set; }
        public long? UserModifiedId { get; private set; }
        public virtual User UserModified { get; private set; }


        //Fields
        private const string _hiddenNetworkName = "Hidden network";


        //Public value set methods
        public void UpdateLocation(int minSignalLevel, double minSignalLatitude, double minSignalLongitude, int maxSignalLevel, double maxSignalLatitude, double maxSignalLongitude)
        {
            bool changes = false;

            if (minSignalLevel < MinSignalLevel)
            {
                MinSignalLevel = minSignalLevel;
                MinSignalLatitude = minSignalLatitude;
                MinSignalLongitude = minSignalLongitude;
                changes = true;
            }

            if (maxSignalLevel > MaxSignalLevel)
            {
                MaxSignalLevel = maxSignalLevel;
                MaxSignalLatitude = maxSignalLatitude;
                MaxSignalLongitude = maxSignalLongitude;
                changes = true;
            }

            if (changes)
            {
                CalculateSignalRadiusAndArea();

                GenerateFingerprint();
            }
        }


        //Private value set methods
        private void SetBssid(string bssid)
        {
            if (bssid.IsEmpty()) 
                throw new ArgumentNullException(nameof(bssid), "Accesspoint bssid can not be empty.");

            Bssid = bssid.Trim();
        }

        public void SetSsid(string ssid)
        {
            if (ssid.IsEmpty())
            {
                Ssid = _hiddenNetworkName;
                IsHidden = true;
                return;
            }
                
            Ssid = ssid.Trim();
            IsHidden = false;
        }

        private void SetMinimalSignalPosition(int signalLevel, double latitude, double longitude)
        {
            if (signalLevel > 100 || signalLevel < 0)
                throw new ArgumentException(nameof(signalLevel), "Invalid signal strength values.");

            MinSignalLevel = signalLevel;

            MinSignalLatitude = Math.Round(latitude, 7);
            MinSignalLongitude = Math.Round(longitude, 7);
        }

        private void SetMaximalSignalPosition(int signalLevel, double latitude, double longitude)
        {
            if (signalLevel > 100 || signalLevel < 0)
                throw new ArgumentException(nameof(signalLevel), "Invalid signal strength values.");

            MaxSignalLevel = signalLevel;

            MaxSignalLatitude = Math.Round(latitude, 7);
            MaxSignalLongitude = Math.Round(longitude, 7);
        }

        public void SetSecurityData(string data)
        {
            if (data.IsEmpty())
                throw new ArgumentNullException(nameof(data), "Security type data payload can not be empty.");

            FullSecurityData = $"{ FullSecurityData } ${ data.Trim() }";
        }

        public void SetManufacturer(string name)
        {
            if (!name.IsEmpty())
            {
                Manufacturer = name;
            }
        }

        public void SetDeviceType(string type)
        {
            if (!type.IsEmpty())
            {
                DeviceType = type;
            }
        }

        public void SetSerializedSecurityData(string value)
        {
            if (value.IsEmpty())
                throw new ArgumentNullException(nameof(value), "Serialized security data can not be null.");

            SerializedSecurityData = value;
        }

        private void SetFrequency(double frequency) =>
            Frequency = frequency;

        public void SetSecurityStatus(bool status) =>
            IsSecure = status;

        public void SetMasterGroup(bool status) =>
            MasterGroup = status;

        public void SetDisplay(bool status) =>
            Display = status;

        public void SetNote(string value) =>
            Note = value.Trim();

        private void SetUserAdded(long userId) =>
            UserAddedId = userId;

        public void SetUserModified(long userId) =>
            UserModifiedId = userId;

        private void GenerateFingerprint()
        {
            string latFactor = Math.Round((MinSignalLatitude + MaxSignalLatitude) / 2.0, 4).ToString();
            string lonFactor = Math.Round((MinSignalLongitude + MaxSignalLongitude) / 2.0, 4).ToString();

            using (var sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{latFactor}{lonFactor}"));

                var sb = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                Fingerprint = sb.ToString();
            }
        }

        private void CalculateSignalRadiusAndArea()
        {
            double o1 = MinSignalLatitude * 3.1415 / 180.0;
            double o2 = MaxSignalLatitude * 3.1415 / 180.0;
            double so = (MaxSignalLatitude - MinSignalLatitude) * 3.1415 / 180.0;
            double sl = (MaxSignalLongitude - MinSignalLongitude) * 3.1415 / 180.0;
            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            
            double radius =  Math.Round(6371e3 * c, 2);
            double area = Math.Round(3.1415 * Math.Pow(radius, 2.0), 2);

            SignalRadius = radius;
            SignalArea = area;
        }

        //Implement the validation method
        public override void Validate()
        {
            if (MinSignalLevel > MaxSignalLevel)
                throw new InvalidOperationException($"Id:{Id} - The minial signal level can not be greater than the max signal level");

            if ((Ssid == _hiddenNetworkName && !IsHidden) || (Ssid.IsEmpty()))
                throw new InvalidOperationException($"Id:{Id} - The network ssid is invalid");
        }


        //Constructors
        private AccessPoint() {}


        //Factory
        public static class Factory
        {
            public static AccessPoint Create(
                string bssid,
                string ssid,
                double frequency,
                int maxSignalLevel,
                double maxSignalLatitude,
                double maxSignalLongitude,
                int minSignalLevel,
                double minSignalLatitude,
                double minSignalLongitude,
                string securityData,
                long userAddedId)
            {
                var accessPoint = new AccessPoint();

                accessPoint.SetBssid(bssid);
                accessPoint.SetSsid(ssid);
                accessPoint.SetFrequency(frequency);
                accessPoint.SetMaximalSignalPosition(maxSignalLevel, maxSignalLatitude, maxSignalLongitude);
                accessPoint.SetMinimalSignalPosition(minSignalLevel, minSignalLatitude, minSignalLongitude);
                accessPoint.SetSecurityData(securityData);

                accessPoint.GenerateFingerprint();
                accessPoint.CalculateSignalRadiusAndArea();

                accessPoint.SetUserAdded(userAddedId);
                accessPoint.SetUserModified(userAddedId);

                accessPoint.SetDisplay(false);
                accessPoint.SetMasterGroup(false);

                return accessPoint;
            }
        }
    }
}
