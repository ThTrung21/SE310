using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookMK.Service
{
    public class FirebaseStorageService
    {
        private readonly string _bucketName = "bookmk-3de98.appspot.com";
        private readonly StorageClient _storageClient;
        private readonly FirebaseStorageService _firebaseStorage;
        
        public FirebaseStorageService()
        {
            
            _storageClient = StorageClient.Create();
        }
       
        public async Task<string> UploadImageAsync(string filePath, string fileName)
        {
            string contentType = "application/octet-stream";
            var extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".jpg" || extension == ".jpeg")
            {
                contentType = "image/jpeg";
            }
            else if (extension == ".png")
            {
                contentType = "image/png";
            }
            else if (extension == ".gif")
            {
                contentType = "image/gif";
            }
            else if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }


            using (var fileStream = File.OpenRead(filePath))
            {
                var uploadObject = await _storageClient.UploadObjectAsync(
                    _bucketName,
                    fileName,
                    contentType,
                    fileStream);

                var firebaseUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{Uri.EscapeDataString(fileName)}?alt=media";

                return firebaseUrl;
            }
        }


        public async Task DeleteImageAsync(int id)
        {
            // Assuming a method GetFileNameById that retrieves the filename based on the given ID


            string filename = id.ToString();

            await _storageClient.DeleteObjectAsync(_bucketName, filename);
            Console.WriteLine($"File {id} deleted successfully.");
        }

    }
}
