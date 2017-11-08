using System.Threading.Tasks;

namespace MyAnimeViewerInterfaces.AnimeDB
{
    public interface IAnimeDBLogin
    {
        /// <summary>
        /// The type of login this Plugin supports.
        /// </summary>
        LoginType loginType { get; }

        /// <summary>
        /// Login to the Plugin's supporting platform.
        /// </summary>
        /// <param name="loginInfo">The required login information for the task.</param>
        /// <returns>True upon success</returns>
        Task<bool> Login(LoginInformation loginInfo);

        /// <summary>
        /// Logout of the Plugin's supporting platform.
        /// </summary>
        void Logout();
    }
}
