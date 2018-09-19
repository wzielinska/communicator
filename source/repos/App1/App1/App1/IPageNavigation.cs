using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public interface IPageNavigation
    {
        #region methods

        void CreateLoginPage();

        void GoToMessenger(object bindingContext);

        void GoToChat(object bindingContext, string receiver);

        void GoBackToMessenger();

        void GoBackToLoginPage();

        #endregion
    }

}
