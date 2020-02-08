using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.Connectivity;

namespace RuntimeComponent1
{
    public sealed class TaskRunner :IBackgroundTask
    {
        BackgroundTaskDeferral _Deferral;
        const string NetworkChangeTaskName = "NetworkChangedTask";
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();

            ExploreNetworkInfo();

            _Deferral.Complete();
        }

        private void ExploreNetworkInfo()
        {
            var Profiles = NetworkInformation.GetConnectionProfiles();
            bool FoundInternet = false;
            foreach (var P in Profiles)
            {
                var Level = P.GetNetworkConnectivityLevel();
                var wiring = (P.IsWlanConnectionProfile || P.IsWwanConnectionProfile) ? "Wireless" : "Adapter";
                if(Level == NetworkConnectivityLevel.InternetAccess)
                {
                    FoundInternet = true;
                    System.Diagnostics.Debug.WriteLine($"{P.ProfileName} {wiring}");
                }
            }

            if(FoundInternet == false)
            {
                System.Diagnostics.Debug.WriteLine("No internet access profile");
            }
        }
    }
}
