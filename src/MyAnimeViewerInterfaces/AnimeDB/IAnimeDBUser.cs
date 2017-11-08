namespace MyAnimeViewerInterfaces.AnimeDB
{
    public interface IAnimeDBUser
    {
        /// <summary>
        /// The user's ID.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// The user's username.
        /// </summary>
        string Username { get; }
    }
}
