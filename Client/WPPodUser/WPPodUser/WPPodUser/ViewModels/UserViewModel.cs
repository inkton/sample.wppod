using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Inkton.Nester.Cloud;

namespace WPPodUser.ViewModels
{
    public class UserViewModel : Inkton.Nester.ViewModels.ViewModel
    {
        private WPPod.Models.User _user;

        public UserViewModel(Inkton.Nester.Models.App app = null)
            : base(app)
        {
            _user = new WPPod.Models.User();
        }

        public WPPod.Models.User User
        {
            get
            {
                return _user;
            }
        }

        async public Task<ServerStatus> CreateUserAsync(WPPod.Models.User user)
        {
            ServerStatus status = new ServerStatus(
                ServerStatus.NEST_RESULT_ERROR);

            if (IsBusy)
                return status;

            IsBusy = true;

            try
            {
                status = await ResultSingle<WPPod.Models.User>.WaitForObjectAsync(
                    false, user, new CachedHttpRequest<WPPod.Models.User>(
                        NesterControl.DeployedApp.CreateAsync), false);

                if (status.Code == 0)
                {
                    _user = status.PayloadToObject<WPPod.Models.User>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return status;
        }

        async public Task<ServerStatus> GetUserAsync(WPPod.Models.User userSeed)
        {
            ServerStatus status = new ServerStatus(
                ServerStatus.NEST_RESULT_ERROR);

            if (IsBusy)
                return status;

            IsBusy = true;

            try
            {
                status = await ResultSingle<WPPod.Models.User>.WaitForObjectAsync(
                    false, userSeed, new CachedHttpRequest<WPPod.Models.User>(
                        NesterControl.DeployedApp.QueryAsync), false);

                if (status.Code >= 0)
                {
                    _user = status.PayloadToObject<WPPod.Models.User>();
                    OnPropertyChanged("EditUser");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return status;
        }
    }
}
