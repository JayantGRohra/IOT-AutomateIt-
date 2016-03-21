using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace testproj.Managers
{
    public interface IManageable
    {
        /// <summary>
        /// This method should return any page specific state that is required to be stored. For correct behaviour, it is required that the state dictionary only contain primitive types.
        /// </summary>
        /// <remarks>
        /// This method is called by the PageManager on navigating away to another page (Not on going back).
        /// </remarks>
        /// <returns></returns>
        Dictionary<string, object> SaveState();
        /// <summary>
        /// This method is called by the PageManager when this page becomes the current page.
        /// </summary>
        /// <param name="lastState">
        /// The page state when it was last navigated to (as provided by the page).
        /// </param>
        /// <remarks>
        /// This method is always invoked when the page is loaded, and hence null checking is advisable to ensure a last state actually exists.
        /// </remarks>
        void LoadState(Dictionary<string, object> lastState);
        /// <summary>
        /// This method is called when the user presses the back button to navigate out of the app. Return false to cancel this behaviour.
        /// </summary>
        bool AllowAppExit();
    }

    /// <summary>
    /// Types of navigation available when navigating to a page.
    /// </summary>
    public enum NavigationType
    {
        /// <summary>
        /// Simply navigates to the requested page and stores the last page's session.
        /// </summary>
        Default,
        /// <summary>
        /// Navigates to the requested page and resets all history and saved state. Use this mode even for a first time navigation.
        /// </summary>
        FreshStart
    }
}

