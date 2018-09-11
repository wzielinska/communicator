using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    public interface IPageNavigation
    {
        #region methods

        void CreateLoginPage();

        void GoToMessenger(object bindingContext);

        void GoBackToLoginPage();

        #endregion
    }

}
