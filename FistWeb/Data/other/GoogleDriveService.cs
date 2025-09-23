using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace FistWeb.Data.other
{
    public static class GoogleDriveService
    {
        private static DriveService? driveService;

        public static void Init(string credentialsPath)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.DriveFile);
            }

            driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BlazorAppUpload",
            });
        }

        public static async Task UploadFileAsync(string fileName, Stream stream, string contentType)
        {
            if (driveService == null) throw new Exception("GoogleDriveService chưa được khởi tạo.");

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                Parents = new[] { "FOLDER_ID_CỦA_BẠN" } // ID thư mục Google Drive muốn lưu
            };

            var request = driveService.Files.Create(fileMetadata, stream, contentType);
            request.Fields = "id";
            var file = await request.UploadAsync();

            if (file.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw new Exception($"Upload thất bại: {file.Exception}");
            }
        }
    }
}
