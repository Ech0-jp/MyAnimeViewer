// 

namespace MyAnimeViewer.MyAnimeList.API
{
    public class MAL_LoginResult
    {
        public MAL_LoginResult(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
    }
}
